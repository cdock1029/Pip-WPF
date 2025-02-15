using System.ComponentModel;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureAIInference;
using Microsoft.SemanticKernel.Connectors.Google;
using Microsoft.SemanticKernel.Connectors.Ollama;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Pip.DataAccess;
using Pip.DataAccess.Services;
using Pip.Model;

#pragma warning disable SKEXP0070

//var builder = Host.CreateApplicationBuilder(args);

using var loggerFactory = CreateLoggerFactory();

var builder = Kernel.CreateBuilder();

var azureConfig =
	new ConfigurationBuilder()
		.AddUserSecrets<Program>()
		.Build()
		.GetSection("AzureAI").Get<AzureAiConfig>() ?? throw new ArgumentNullException();

builder.Plugins
	//.AddFromType<WeatherPlugin3>()
	.AddFromType<TreasuryPlugin>();

builder.Services
	.AddHttpClient<ITreasuryDataProvider, TreasuryDataProvider>();

builder.Services
	//.AddSingleton(loggerFactory)
	.AddMemoryCache()
	.AddSingleton<ITreasuryDataProvider, TreasuryDataProvider>()
	.AddDbContext<PipDbContext>(ServiceLifetime.Singleton);

builder
	.AddGoogleAIGeminiChatCompletion(apiKey: azureConfig.GeminiKey, modelId: "gemini-2.0-flash", serviceId: "gemini")
	.AddAzureAIInferenceChatCompletion(azureConfig.ModelId, endpoint: new Uri(azureConfig.ModelInferenceEndpoint),
		apiKey: azureConfig.AzureKeyCredential, serviceId: "azure")
	.AddOllamaChatCompletion("qwen2.5", new Uri(azureConfig.OllamaEndpoint), "ollama");


var kernel = builder.Build();

{
	var ctx = kernel.GetRequiredService<PipDbContext>();
	ctx.Database.Migrate();
}

//kernel.AutoFunctionInvocationFilters.Add(new AddReturnTypeSchemaFilter());


//kernel.ImportPluginFromFunctions("HelperFunctions", [
//	kernel.CreateFunctionFromMethod(GetCurrentWeather)
//]);


var chatHistory = new ChatHistory(
	"""
	You're a financial support agent that can provide information on US treasuries, the user's personal investment portfolio of saved treasuries, and other assorted financial knowlege and info.
	Be succint, clear, and feel free to call functions available to you to give the user information he desires.
	""");

var sb = new StringBuilder();

AzureAIInferencePromptExecutionSettings azureAiInferencePromptExecutionSettings =
	new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };

OllamaPromptExecutionSettings ollamaPromptExecutionSettings =
	new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };

GeminiPromptExecutionSettings googlePromptExecutionSettings =
	new()
	{
		ToolCallBehavior = GeminiToolCallBehavior.AutoInvokeKernelFunctions,
		FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
	};


var chatService = kernel.GetRequiredService<IChatCompletionService>("azure");
var agentLabel = "Azure";
PromptExecutionSettings settings = azureAiInferencePromptExecutionSettings;
Console.WriteLine(@"Using azure. Switch service: \gemini, \azure, or \ollama");
while (true)
{
	Console.Write("You: ");

	var prompt = Console.ReadLine() ?? throw new ArgumentNullException();
	if (string.IsNullOrEmpty(prompt)) continue;
	if (prompt == "q")
	{
		Console.WriteLine("\nGoodbye!\n");
		break;
	}

	if (prompt.StartsWith("\\"))
	{
		switch (prompt)
		{
			case @"\gemini":
				chatService = kernel.GetRequiredService<IChatCompletionService>("gemini");
				settings = googlePromptExecutionSettings;
				agentLabel = "Gemini";
				break;
			case @"\azure":
				chatService = kernel.GetRequiredService<IChatCompletionService>("azure");
				settings = azureAiInferencePromptExecutionSettings;
				agentLabel = "Azure";
				break;
			case @"\ollama":
				chatService = kernel.GetRequiredService<IChatCompletionService>("ollama");
				settings = ollamaPromptExecutionSettings;
				agentLabel = "Ollama";
				break;
			case @"\clear":
			case @"\c":
				Console.Clear();
				Console.WriteLine("clearing message context...\n");
				chatHistory.Clear();
				continue;
			default:
				Console.WriteLine("\nUnrecognized service. No change.\n");
				continue;
		}

		Console.WriteLine($"\nSwitched to {prompt[1..]}\n");
		continue;
	}

	chatHistory.AddUserMessage(prompt);
	Console.WriteLine();


	if (settings is OllamaPromptExecutionSettings ollamaSettings)
		try
		{
			var msg = await chatService.GetChatMessageContentAsync(chatHistory, ollamaSettings, kernel);
			chatHistory.Add(msg);
			Console.WriteLine($"{agentLabel}: {msg.Content}");
		}
		catch (HttpRequestException e)
		{
			Console.WriteLine($"There's a problem connecting: {e.Message}\n");
			continue;
		}
	else
		try
		{
			Console.Write($"{agentLabel}: ");
			await foreach (var update in chatService.GetStreamingChatMessageContentsAsync(chatHistory,
				               settings,
				               kernel))
			{
				sb.Append(update);
				Console.Write(update);
			}

			chatHistory.AddAssistantMessage(sb.ToString());
		}
		catch (HttpRequestException e)
		{
			Console.WriteLine($"There's a problem connecting: {e.Message}\n");
			continue;
		}
		finally
		{
			sb.Clear();
		}

	Console.WriteLine("\n-------------------------------------\n");
}

return;

[Description("Gets the current weather")]
string GetCurrentWeather()
{
	//return Random.Shared.NextDouble() > 0.5 ? "It's sunny" : "It's raining";
	return "It's raining";
}

static ILoggerFactory CreateLoggerFactory()
{
	var resourceBuilder = ResourceBuilder
		.CreateDefault()
		.AddService("TelemetryConsoleQuickstart");

// Enable model diagnostics with sensitive data.
	AppContext.SetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnosticsSensitive", true);

	using var traceProvider = Sdk.CreateTracerProviderBuilder()
		.SetResourceBuilder(resourceBuilder)
		.AddSource("Microsoft.SemanticKernel*")
		.AddConsoleExporter()
		.Build();

	using var meterProvider = Sdk.CreateMeterProviderBuilder()
		.SetResourceBuilder(resourceBuilder)
		.AddMeter("Microsoft.SemanticKernel*")
		.AddConsoleExporter()
		.Build();

	return LoggerFactory.Create(builder =>
	{
		// Add OpenTelemetry as a logging provider
		builder.AddOpenTelemetry(options =>
		{
			options.SetResourceBuilder(resourceBuilder);
			options.AddConsoleExporter();
			// Format log messages. This is default to false.
			options.IncludeFormattedMessage = true;
			options.IncludeScopes = true;
		});
		builder.SetMinimumLevel(LogLevel.Information);
	});
}

public class AzureAiConfig
{
	public string AzureKeyCredential { get; set; } = null!;

	public string GeminiKey { get; set; } = null!;

	public string ModelInferenceEndpoint { get; set; } = null!;

	public string ModelId { get; set; } = null!;

	public string OllamaEndpoint { get; set; } = null!;
}

public sealed class WeatherPlugin3
{
	[KernelFunction]
	public WeatherData GetWeatherData(string city, string state)
	{
		return new WeatherData
		{
			Data1 = 35.0, // Temperature in degrees Celsius  
			Data2 = 20.0, // Humidity in percentage  
			Data3 = 10.0, // Dew point in degrees Celsius  
			Data4 = 15.0 // Wind speed in kilometers per hour
		};
	}

	public sealed class WeatherData
	{
		[Description("Temp (°C)")] public double Data1 { get; set; }

		[Description("Humidity (%)")] public double Data2 { get; set; }

		[Description("Dew point (°C)")] public double Data3 { get; set; }

		[Description("Wind speed (km/h)")] public double Data4 { get; set; }
	}
}

[UsedImplicitly]
public sealed class TreasuryPlugin(ITreasuryDataProvider dataProvider)
{
	[KernelFunction]
	[Description(
		"Returns a list of the upcoming US treasury auction securities of various terms and types, i.e 4-week T-Bills, 2-year Notes, 20-year Bonds etc.")]
	public async Task<List<TreasuryData>> ListUpcomingAuctions()
	{
		var treasuries = await dataProvider.GetUpcomingAsync();

		return treasuries?.Select(t => new TreasuryData
		{
			Cusip = t.Cusip,
			IssueDate = t.IssueDate,
			AuctionDate = t.AuctionDate,
			Term = t.SecurityTerm,
			Type = t.Type.ToString()
		}).ToList() ?? [];
	}

	[KernelFunction]
	[Description(
		"Returns a list of saved investments of US treasuries in my portfolio")]
	public async Task<List<Investment>> ListSavedPortfolioInvestedTreasuries()
	{
		var usts = await Task.Run(dataProvider.GetInvestments);

		return usts.ToList();
	}

	[KernelFunction]
	[Description(
		"Total par dollar value of all treasuries invested in my portfolio")]
	public int GetInvestmentsTotal()
	{
		return dataProvider.GetInvestments().Sum(i => i.Par);
	}


	[Description("A US government issued treasury security")]
	public sealed class TreasuryData
	{
		[Description("Nine digit identifier for a treasury security")]
		public string Cusip { get; set; } = null!;

		[Description("Date when treasury will be sold at auction")]
		public DateOnly? AuctionDate { get; set; }

		[Description("Date when treasury will be issued to buyers")]
		public DateOnly? IssueDate { get; set; }

		[Description("The length of time the security earns interest, from issue date to maturity date")]
		public string? Term { get; set; }

		[Description("Which type of treasury: Bill, Note, Bond, CMB, TIPS, or FRN")]
		public string? Type { get; set; }
	}
}

public class AddReturnTypeSchemaFilter : IAutoFunctionInvocationFilter
{
	public async Task OnAutoFunctionInvocationAsync(AutoFunctionInvocationContext context,
		Func<AutoFunctionInvocationContext, Task> next)
	{
		// Invoke the function
		await next(context);

		// Crete the result with the schema
		FunctionResultWithSchema resultWithSchema = new()
		{
			Value = context.Result.GetValue<object>(), // Get the original result
			Schema = context.Function.Metadata.ReturnParameter?.Schema // Get the function return type schema
		};

		// Return the result with the schema instead of the original one
		context.Result = new FunctionResult(context.Result, resultWithSchema);
	}

	private sealed class FunctionResultWithSchema
	{
		public object? Value { get; set; }

		public KernelJsonSchema? Schema { get; set; }
	}
}