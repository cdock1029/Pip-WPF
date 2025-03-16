using System.Windows;
using System.Windows.Threading;
using DevExpress.Blazor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using MessageBox = System.Windows.MessageBox;

#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0070

namespace Td;

public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
	    AppDomain.CurrentDomain.UnhandledException += App_DomainUnhandledException;
	    Current.DispatcherUnhandledException += App_OnDispatcherUnhandledException;

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
        IConfiguration _)
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

        serviceCollection
            .AddHttpClient<ITreasuryDataProvider, TreasuryDataProvider>()
            .AddStandardResilienceHandler();

        serviceCollection.AddSingleton<ReloadNotifierService>();
        serviceCollection.AddSingleton<Settings>();
        serviceCollection.AddSingleton<AppState>();
        serviceCollection.AddFluentUIComponents();

        //ChatConfig? chatConfig = config.GetSection("ChatConfig").Get<ChatConfig>();

        //serviceCollection
        //    .AddKernel()
        //    .AddOpenAIChatCompletion(apiKey: chatConfig!.OpenAiApiKey!, modelId: "gpt-4o-mini")
        //    .Plugins
        //    .AddFromType<UtilitiesPlugin>()
        //    .AddFromType<TreasuryPlugin>();

        //serviceCollection.AddChatClient(serviceProvider =>
        //{
        //    Kernel kernel = serviceProvider.GetRequiredService<Kernel>();
        //    IChatCompletionService completionService = kernel.GetRequiredService<IChatCompletionService>();
        //    IChatClient client = completionService.AsChatClient();
        //    ChatClientBuilder builder = new ChatClientBuilder(client)
        //        .UseFunctionInvocation()
        //        .ConfigureOptions(options =>
        //        {
        //            IEnumerable<AIFunction> aiFunctions =
        //                kernel.Plugins.SelectMany(kp => kp.AsAIFunctions());
        //            options.Tools = [..aiFunctions];
        //            options.ToolMode = ChatToolMode.Auto;
        //        });
        //    return builder.Build();
        //});

        //serviceCollection.AddDevExpressAI(settings =>
        //{
        //    settings.RegisterAIExceptionHandler(new PipAiExceptionHandler());
        //});


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

    private static void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
#if DEBUG
	    MessageBox.Show($"UI Dispatcher error: {e.Exception.Message}", "Error", MessageBoxButton.OK,
		    MessageBoxImage.Error);
#else
        MessageBox.Show($"A User Interface error has occurred. {e.Exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
#endif
	    Debug.WriteLine($"Unhandled Dispatcher Exception: {e.Exception.Message}");
	    e.Handled = true;
    }

    private static void App_DomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
	    MessageBox.Show($"App error: {e.ExceptionObject}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
	    Debug.WriteLine($"Unhandled Domain Exception: {e.ExceptionObject}");
    }
}

//file sealed class ChatConfig
//{
//    public string? OpenAiApiKey { get; init; }
//    public string? AzureKeyCredential { get; init; }
//}

//file class PipAiExceptionHandler : IAIExceptionHandler
//{
//    public Exception ProcessException(Exception e)
//    {
//        Debug.WriteLine($"\n\nAI Exception MESSAGE in PIP: {e.Message}\n");

//        Debug.WriteLine($"\nInner Exception MESSAGE in PIP: {e.InnerException?.Message}");

//        Debug.WriteLine($"\nAI Exception in PIP: {e}");

//        Debug.WriteLine($"\nInner Exception in PIP: {e.InnerException}");
//        return e;
//    }
//}