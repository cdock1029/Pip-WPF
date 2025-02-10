using System.ComponentModel;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureAIInference;
using Microsoft.SemanticKernel.Connectors.Google;
using Microsoft.SemanticKernel.Connectors.Ollama;
using Pip.DataAccess;
using Pip.DataAccess.Services;

#pragma warning disable SKEXP0070

//var builder = Host.CreateApplicationBuilder(args);

/*
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

using var loggerFactory = LoggerFactory.Create(builder =>
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
*/

var builder = Kernel.CreateBuilder();

var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
var azureConfig = config.GetSection("AzureAI").Get<AzureAiConfig>() ?? throw new ArgumentNullException();

builder.Services
	//.AddSingleton(loggerFactory)
	//.AddAzureAIInferenceChatCompletion(azureConfig.ModelId, endpoint: new Uri(azureConfig.ModelInferenceEndpoint), apiKey: azureConfig.AzureKeyCredential)
	.AddGoogleAIGeminiChatCompletion(apiKey: azureConfig.GeminiKey, modelId: "gemini-2.0-flash")
	//.AddOllamaChatCompletion("qwen2.5", new Uri(azureConfig.OllamaEndpoint))
	.AddMemoryCache()
	.AddSingleton<ITreasuryDataProvider, TreasuryDataProvider>()
	.AddDbContext<PipDbContext>(ServiceLifetime.Singleton)
	.AddHttpClient<ITreasuryDataProvider, TreasuryDataProvider>();

builder.Plugins
	//.AddFromType<WeatherPlugin3>()
	.AddFromType<TreasuryPlugin>();

var kernel = builder.Build();
//kernel.AutoFunctionInvocationFilters.Add(new AddReturnTypeSchemaFilter());


//kernel.ImportPluginFromFunctions("HelperFunctions", [
//	kernel.CreateFunctionFromMethod(GetCurrentWeather)
//]);


var chatService = kernel.GetRequiredService<IChatCompletionService>();

var dataProvider = kernel.GetRequiredService<ITreasuryDataProvider>();

var chatHistory = new ChatHistory();

var sb = new StringBuilder();

#pragma warning disable SKEXP0070
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
#pragma warning restore SKEXP0070


while (true)
{
	Console.Write("You: ");
	var prompt = Console.ReadLine() ?? throw new ArgumentNullException();
	if (prompt == "q") break;

	chatHistory.AddUserMessage(prompt);
	Console.WriteLine();

	Console.Write("AI: ");
	await foreach (var update in chatService.GetStreamingChatMessageContentsAsync(chatHistory,
		               googlePromptExecutionSettings,
		               kernel))
	{
		sb.Append(update);
		Console.Write(update);
	}

	chatHistory.AddAssistantMessage(sb.ToString());
	sb.Clear();


	//var msg = await chatService.GetChatMessageContentAsync(chatHistory, ollamaPromptExecutionSettings, kernel);
	//chatHistory.Add(msg);
	//Console.WriteLine($"AI: {msg.Content}");

	Console.WriteLine();
	Console.WriteLine("-------------------------------------");
	Console.WriteLine();
}

return;

[Description("Gets the current weather")]
string GetCurrentWeather()
{
	//return Random.Shared.NextDouble() > 0.5 ? "It's sunny" : "It's raining";
	return "It's raining";
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

public sealed class TreasuryPlugin(ITreasuryDataProvider dataProvider)
{
	[KernelFunction]
	[Description(
		"Returns a list of the upcoming US treasury auction securities of various terms and types, i.e 4-week T-Bills, 2-year Notes, 20-year Bonds etc.")]
	public async Task<IEnumerable<TreasuryData>> ListUpcomingAuctions()
	{
		var treasuries = await dataProvider.GetUpcomingAsync();

		return treasuries?.Select(t => new TreasuryData
		{
			Cusip = t.Cusip,
			IssueDate = t.IssueDate,
			AuctionDate = t.AuctionDate,
			Term = t.SecurityTerm,
			Type = t.Type.ToString()
		}) ?? [];
	}


	[Description("A US government issued treasury security")]
	public sealed class TreasuryData
	{
		[Description("Nine digit identification number for a treasury security")]
		public string Cusip { get; set; } = null!;

		[Description("Date when treasury will be sold at auction")]
		public DateOnly? AuctionDate { get; set; }

		[Description("Date when treasury will be issued to buyers")]
		public DateOnly? IssueDate { get; set; }

		[Description("The length of time the security earns interest")]
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