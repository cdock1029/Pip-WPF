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
	.AddFromType<TreasuryPlugin>();

builder.Services
	.AddHttpClient<ITreasuryDataProvider, TreasuryDataProvider>();

builder.Services
	//.AddSingleton(loggerFactory)
	.AddMemoryCache()
	.AddSingleton<ITreasuryDataProvider, TreasuryDataProvider>()
	.AddDbContext<PipDbContext>(ServiceLifetime.Transient);

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
	Be succint, clear, and feel free to call functions available to you to give the user information he desires. Show internal database ID's when displaying results, so user can modify entries.
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
		"Adds an investment to the portfolio for the treasury with given CUSIP and issue date. Par value is optional and defaults to 0 if not passed in.")]
	public async Task AddTreasuryInvestmentToPorfolio(
		[Description("Unique number Treasury Dept. uses to identify securities maturing on a specific date")]
		string cusip,
		[Description("When the Treasury puts the security into the buyer's account")]
		DateOnly issueDate,
		[Description("The stated $ value of a security on its original issue date, defaults to 0")]
		int parValue = 0)
	{
		var t = await dataProvider.LookupTreasuryAsync(cusip, issueDate);
		ArgumentNullException.ThrowIfNull(t);
		await Task.Run(() =>
		{
			dataProvider.Insert(new Investment
			{
				Cusip = cusip,
				IssueDate = issueDate,
				AuctionDate = t.AuctionDate,
				Par = parValue,
				MaturityDate = t.MaturityDate,
				Type = t.Type,
				SecurityTerm = t.SecurityTerm
			});
		});
	}

	[KernelFunction]
	[Description("Returns a list of treasuries that have the given CUSIP identifier")]
	public async Task<List<TreasuryData>> LookupTreasuriesByCusip(string cusip)
	{
		var treasuries = await dataProvider.SearchTreasuriesAsync(cusip);
		return treasuries?.Select(t => new TreasuryData
		{
			Cusip = cusip,
			AuctionDate = t.AuctionDate,
			IssueDate = t.IssueDate,
			Type = t.Type.ToString(),
			Term = t.SecurityTerm
		}).ToList() ?? [];
	}

	[KernelFunction]
	[Description("Deletes the investment with given id from user's portfolio")]
	public async Task DeleteInvestmentById(int id)
	{
		await dataProvider.DeleteInvesmentByIdAsync(id);
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