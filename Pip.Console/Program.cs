using System.ComponentModel;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureAIInference;
using Microsoft.SemanticKernel.Connectors.Google;
using Microsoft.SemanticKernel.Connectors.Ollama;
using Pip.DataAccess;
using Pip.DataAccess.Services;
using Pip.Model;

#pragma warning disable SKEXP0070

//var builder = Host.CreateApplicationBuilder(args);

//using var listener = AzureEventSourceListener.CreateConsoleLogger(EventLevel.Verbose);
//using ILoggerFactory loggerFactory = CreateLoggerFactory();

IKernelBuilder builder = Kernel.CreateBuilder();

AzureAiConfig azureConfig =
    new ConfigurationBuilder()
        .AddUserSecrets<Program>()
        .Build()
        .GetSection("AzureAI").Get<AzureAiConfig>() ?? throw new ArgumentNullException();

builder.Plugins
    .AddFromType<Utilities>()
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


Kernel kernel = builder.Build();

{
    PipDbContext ctx = kernel.GetRequiredService<PipDbContext>();
    ctx.Database.Migrate();
}

//kernel.AutoFunctionInvocationFilters.Add(new AddReturnTypeSchemaFilter());


//kernel.ImportPluginFromFunctions("HelperFunctions", [
//	kernel.CreateFunctionFromMethod(GetCurrentWeather)
//]);


ChatHistory chatHistory = new(
    """
    You're a financial support agent that can provide information on US treasuries, the user's personal investment portfolio of saved treasuries, and other assorted financial knowlege and info.
    Be succint, clear, call functions available to you as needed. Show internal database ID's when displaying results, so user can modify entries.
    Do not use markdown style. Rather, format your output for a terminal console, with any data list shown in a table.
    """);

StringBuilder sb = new();

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


IChatCompletionService chatService = kernel.GetRequiredService<IChatCompletionService>("azure");
var agentLabel = "Azure";
PromptExecutionSettings settings = azureAiInferencePromptExecutionSettings;
Console.WriteLine(@"Using azure. Switch service: \gemini, \azure, or \ollama");
while (true)
{
    Console.Write("You: ");

    string prompt = Console.ReadLine() ?? throw new ArgumentNullException();
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
            ChatMessageContent msg = await chatService.GetChatMessageContentAsync(chatHistory, ollamaSettings, kernel);
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
            await foreach (StreamingChatMessageContent update in chatService.GetStreamingChatMessageContentsAsync(
                               chatHistory,
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

//static ILoggerFactory CreateLoggerFactory()
//{
//    ResourceBuilder resourceBuilder = ResourceBuilder
//        .CreateDefault()
//        .AddService("TelemetryConsoleQuickstart");

//// Enable model diagnostics with sensitive data.
//    AppContext.SetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnosticsSensitive", true);

//    using TracerProvider traceProvider = Sdk.CreateTracerProviderBuilder()
//        .SetResourceBuilder(resourceBuilder)
//        .AddSource("Microsoft.SemanticKernel*")
//        .AddConsoleExporter()
//        .Build();

//    using MeterProvider meterProvider = Sdk.CreateMeterProviderBuilder()
//        .SetResourceBuilder(resourceBuilder)
//        .AddMeter("Microsoft.SemanticKernel*")
//        .AddConsoleExporter()
//        .Build();

//    return LoggerFactory.Create(builder =>
//    {
//        // Add OpenTelemetry as a logging provider
//        builder.AddOpenTelemetry(options =>
//        {
//            options.SetResourceBuilder(resourceBuilder);
//            options.AddConsoleExporter();
//            // Format log messages. This is default to false.
//            options.IncludeFormattedMessage = true;
//            options.IncludeScopes = true;
//        });
//        builder.SetMinimumLevel(LogLevel.Information);
//    });
//}

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
    [KernelFunction("list_upcoming_treasury_auctions")]
    [Description(
        "Returns a list of the upcoming US treasury auction securities of various terms and types, i.e 4-week T-Bills, 2-year Notes, 20-year Bonds etc.")]
    public async Task<List<TreasuryData>> ListUpcomingTreasuryAuctions()
    {
        IEnumerable<Treasury>? treasuries = await dataProvider.GetUpcomingAsync();

        return treasuries?.Select(t => new TreasuryData
        {
            Cusip = t.Cusip,
            IssueDate = t.IssueDate,
            AuctionDate = t.AuctionDate,
            Term = t.SecurityTerm,
            Type = t.Type.ToString()
        }).ToList() ?? [];
    }

    [KernelFunction("list_saved_invested_treasuries_in_portfolio")]
    [Description(
        "Returns a list of saved investments of US treasuries in my portfolio")]
    public async Task<List<Investment>> ListSavedInvestedTreasuriesInPortfolio()
    {
        IEnumerable<Investment> usts = await Task.Run(dataProvider.GetInvestments);

        return usts.ToList();
    }

    [KernelFunction("add_treasury_investment_to_portfolio")]
    [Description(
        "Adds an investment to the portfolio for the treasury with given CUSIP and issue date. Par value is optional and defaults to 0 if not passed in.")]
    public async Task AddTreasuryInvestmentToPorfolio(
        [Description("Unique number Treasury Dept. uses to identify securities maturing on a specific date")]
        string cusip,
        [Description(
            "When the Treasury is issued to the buyer. ISO 8601 extended format date-only string (example: 2024-01-31)")]
        string issueDate,
        [Description("The stated $ value of a security on its original issue date, defaults to 0")]
        int parValue = 0)
    {
        DateOnly issueDateOnly = DateOnly.Parse(issueDate);
        Treasury? t = await dataProvider.LookupTreasuryAsync(cusip, issueDateOnly);
        ArgumentNullException.ThrowIfNull(t);
        await Task.Run(() =>
        {
            dataProvider.Insert(new Investment
            {
                Cusip = cusip,
                IssueDate = issueDateOnly,
                AuctionDate = t.AuctionDate,
                Par = parValue,
                MaturityDate = t.MaturityDate,
                Type = t.Type,
                SecurityTerm = t.SecurityTerm
            });
        });
    }

    [KernelFunction("lookup_treasuries_by_cusip")]
    [Description("Returns a list of treasuries that have the given CUSIP identifier")]
    public async Task<List<TreasuryData>> LookupTreasuriesByCusip(
        [Description("Unique number Treasury Dept. uses to identify securities maturing on a specific date")]
        string cusip)
    {
        IEnumerable<Treasury>? treasuries = await dataProvider.SearchTreasuriesAsync(cusip);
        return treasuries?.Select(t => new TreasuryData
        {
            Cusip = cusip,
            AuctionDate = t.AuctionDate,
            IssueDate = t.IssueDate,
            Type = t.Type.ToString(),
            Term = t.SecurityTerm
        }).ToList() ?? [];
    }

    [KernelFunction("delete_investment_by_id")]
    [Description(
        "Deletes the investment with given id from user's portfolio, returns a boolean signaling success or failure")]
    public async Task<bool> DeleteInvestmentById(int id)
    {
        await dataProvider.DeleteInvesmentByIdAsync(id);
        return true;
    }

    [KernelFunction("get_investments_total")]
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

[UsedImplicitly]
public sealed class Utilities
{
    [KernelFunction]
    public string GetCurrentUtcTime()
    {
        return DateTime.UtcNow.ToString("R");
    }
}

[UsedImplicitly]
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
            Schema = context.Function.Metadata.ReturnParameter.Schema // Get the function return type schema
        };

        // Return the result with the schema instead of the original one
        context.Result = new FunctionResult(context.Result, resultWithSchema);
    }

    private sealed class FunctionResultWithSchema
    {
        public object? Value { [UsedImplicitly] get; set; }

        public KernelJsonSchema? Schema { [UsedImplicitly] get; set; }
    }
}