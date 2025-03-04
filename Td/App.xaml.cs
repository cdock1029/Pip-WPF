using System.Windows;
using System.Windows.Threading;
using DevExpress.AIIntegration;
using DevExpress.AIIntegration.Extensions;
using DevExpress.AIIntegration.Services.Chat;
using DevExpress.Blazor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Pip.Console.Plugins;
using Pip.DataAccess.Plugins;
using MessageBox = System.Windows.MessageBox;

#pragma warning disable SKEXP0001

namespace Td;

public partial class App
{
	protected override void OnStartup(StartupEventArgs e)
	{
		ServiceCollection serviceCollection = new();
		IConfiguration configuration =
			new ConfigurationBuilder()
				.AddUserSecrets<App>()
				.Build();
		ConfigureServices(serviceCollection, configuration);

		ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
		Resources.Add("services", serviceProvider);

		PipDbContext dbContext = serviceProvider.GetRequiredService<PipDbContext>();
		dbContext.Database.Migrate();

		MainWindow mainWindow = serviceProvider.GetRequiredService<MainWindow>();
		mainWindow.Show();
	}

	private static void ConfigureServices(ServiceCollection serviceCollection,
		IConfiguration config)
	{
		serviceCollection.AddSingleton<MainWindow>();

		// TODO: determine what lifetime is ok, prefer scoped for semantic kernel
		serviceCollection.AddDbContext<PipDbContext>(ServiceLifetime.Scoped);
		serviceCollection.AddWpfBlazorWebView();

		serviceCollection.AddDevExpressBlazor(configure =>
		{
			configure.SizeMode = SizeMode.Medium;
			//configure.BootstrapVersion = BootstrapVersion.v5;
		});


		serviceCollection.AddMemoryCache();
		serviceCollection.AddHttpClient<ITreasuryDataProvider, TreasuryDataProvider>();
		serviceCollection.AddSingleton<ReloadNotifierService>();
		serviceCollection.AddSingleton<Settings>();
		serviceCollection.AddSingleton<AppState>();
		serviceCollection.AddFluentUIComponents();

		ChatConfig? chatConfig = config.GetSection("ChatConfig").Get<ChatConfig>();
		ArgumentNullException.ThrowIfNull(chatConfig);


		serviceCollection
			.AddKernel()
			.AddOpenAIChatCompletion(apiKey: chatConfig.OpenAiApiKey, modelId: "gpt-4o-mini");

		serviceCollection.AddSingleton<KernelPlugin>(
			sp => KernelPluginFactory.CreateFromType<UtilitiesPlugin>(serviceProvider: sp));
		serviceCollection.AddSingleton<KernelPlugin>(
			sp => KernelPluginFactory.CreateFromType<TreasuryPlugin>(serviceProvider: sp));

		serviceCollection.AddSingleton<OpenAIPromptExecutionSettings>(sp => new OpenAIPromptExecutionSettings
			{ FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() });


		serviceCollection.AddChatClient(builder =>
		{
			Kernel kernel = builder.Services.GetRequiredService<Kernel>();
			OpenAIPromptExecutionSettings promptSettings =
				builder.Services.GetRequiredService<OpenAIPromptExecutionSettings>();
			IChatCompletionService completionService = kernel.GetRequiredService<IChatCompletionService>();
			IChatClient client = completionService.AsChatClient();
			return builder
				.UseFunctionInvocation()
				.UseLogging()
				.UseChatOptions(options =>
				{
					ChatOptions newOptions = options?.Clone() ?? new ChatOptions();

					if (promptSettings.FunctionChoiceBehavior
						    ?.GetConfiguration(new FunctionChoiceBehaviorConfigurationContext([]) { Kernel = kernel })
						    .Functions is { Count: > 0 } funcs)
					{
						newOptions.Tools = funcs.Select(f => f.AsAIFunction(kernel)).Cast<AITool>().ToList();
						newOptions.ToolMode = promptSettings.FunctionChoiceBehavior is RequiredFunctionChoiceBehavior
							? ChatToolMode.RequireAny
							: ChatToolMode.Auto;
					}

					return options;
				})
				.Use(client);
		});

		serviceCollection.AddDevExpressAI(settings =>
		{
			//Kernel kernel = settings.GetRequiredService<Kernel>();
			//OpenAIPromptExecutionSettings promptSettings = settings.GetRequiredService<OpenAIPromptExecutionSettings>();
			//IChatCompletionService completionService = kernel.GetRequiredService<IChatCompletionService>();

			//IChatClient client = new ChatClientBuilder(settings)
			//    .UseFunctionInvocation()
			//    .UseLogging()
			//    .UseChatOptions(options =>
			//    {
			//        ChatOptions newOptions = options?.Clone() ?? new ChatOptions();

			//        if (promptSettings.FunctionChoiceBehavior
			//                ?.GetConfiguration(new FunctionChoiceBehaviorConfigurationContext([]) { Kernel = kernel })
			//                .Functions is { Count: > 0 } funcs)
			//        {
			//            newOptions.Tools = funcs.Select(f => f.AsAIFunction(kernel)).Cast<AITool>().ToList();
			//            newOptions.ToolMode = promptSettings.FunctionChoiceBehavior is RequiredFunctionChoiceBehavior
			//                ? ChatToolMode.RequireAny
			//                : ChatToolMode.Auto;
			//        }

			//        return options;
			//    })
			//    .Use(completionService.AsChatClient());

			//settings.RegisterChatClient(client);
			//settings.RegisterAIChatClientCustomizeMessageRequest(new PipMessageHandler());
			settings.RegisterAIExceptionHandler(new PipAiExceptionHandler());
		});


#if DEBUG
		serviceCollection.AddBlazorWebViewDeveloperTools();
		serviceCollection.AddLogging(logging =>
		{
			ResourceBuilder resourceBuilder = ResourceBuilder
				.CreateDefault()
				.AddService("TelemetryConsoleQuickstart");

			// Enable model diagnostics with sensitive data.
			AppContext.SetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnosticsSensitive", true);

			using TracerProvider traceProvider = Sdk.CreateTracerProviderBuilder()
				.SetResourceBuilder(resourceBuilder)
				.AddSource("Microsoft.SemanticKernel*")
				.AddConsoleExporter()
				.Build();

			using MeterProvider meterProvider = Sdk.CreateMeterProviderBuilder()
				.SetResourceBuilder(resourceBuilder)
				.AddMeter("Microsoft.SemanticKernel*")
				.AddConsoleExporter()
				.Build();

			/* old logging */
			logging.AddFilter("Microsoft.AspNetCore.Components.WebView", LogLevel.Trace);
			logging.AddDebug();
			/* old logging */

			// Add OpenTelemetry as a logging provider
			logging.AddOpenTelemetry(options =>
			{
				options.SetResourceBuilder(resourceBuilder);
				options.AddConsoleExporter();
				// Format log messages. This is default to false.
				options.IncludeFormattedMessage = true;
				options.IncludeScopes = true;
			});
			logging.SetMinimumLevel(LogLevel.Information);
		});
#endif
	}

	private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
#if DEBUG
		MessageBox.Show(e.Exception.ToString(), "Error");
#else
		MessageBox.Show("An error has occurred.", "Error");
#endif
		Debug.WriteLine($"Unhandled Exception: {e.Exception}");
	}
}

file sealed class ChatConfig
{
	public string OpenAiApiKey { get; init; } = null!;
}

file class PipAiExceptionHandler : IAIExceptionHandler
{
	public Exception ProcessException(Exception e)
	{
		Debug.WriteLine($"AI Exception in PIP: {e}");
		return e;
	}
}

file class PipMessageHandler : IAIChatClientCustomizeMessageRequest
{
	public void Customize(ChatMessageRequest messageRequest, BaseRequest originalRequest, RequestContext context)
	{
		messageRequest.Options.Tools =
		[
			//AIFunctionFactory.Create()
		];
	}
}