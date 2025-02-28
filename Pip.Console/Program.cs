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
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Pip.Console;
using Pip.Console.Plugins;
using Pip.DataAccess;
using Pip.DataAccess.Services;

#pragma warning disable SKEXP0070

//using ILoggerFactory loggerFactory = CreateLoggerFactory();

IKernelBuilder builder = Kernel.CreateBuilder();

AzureAiConfig azureConfig =
    new ConfigurationBuilder()
        .AddUserSecrets<Program>()
        .Build()
        .GetSection("AzureAI").Get<AzureAiConfig>() ?? throw new ArgumentNullException();

builder.Plugins
    .AddFromType<UtilitiesPlugin>()
    .AddFromType<TreasuryPlugin>();

builder.Services
    .AddHttpClient<ITreasuryDataProvider, TreasuryDataProvider>();

builder.Services
    //.AddSingleton(loggerFactory)
    .AddMemoryCache()
    .AddSingleton<ITreasuryDataProvider, TreasuryDataProvider>()
    .AddDbContext<PipDbContext>();

builder
    .AddGoogleAIGeminiChatCompletion(apiKey: azureConfig.GeminiKey, modelId: "gemini-2.0-flash", serviceId: "gemini")
    //.AddVertexAIGeminiChatCompletion("claude-3-7-sonnet@20250219", azureConfig.VertexBearerKey, "us-central1",
    //    "gen-lang-client-0833800389",
    //    serviceId: "claude")
    .AddAzureAIInferenceChatCompletion("gpt-4o", endpoint: new Uri(azureConfig.ModelInferenceEndpoint),
        apiKey: azureConfig.AzureKeyCredential, serviceId: "azure")
    .AddOpenAIChatCompletion(apiKey: azureConfig.OpenAiApiKey, modelId: "gpt-4o", serviceId: "openai")
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
    Do not use markdown style. Rather, format your output for a terminal console, with any lists of data shown in a table.
    """);

StringBuilder sb = new();

OpenAIPromptExecutionSettings openAiPromptExecutionSettings =
    new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };

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


IChatCompletionService chatService = kernel.GetRequiredService<IChatCompletionService>("openai");
var agentLabel = "OpenAI";
PromptExecutionSettings settings = openAiPromptExecutionSettings;
Console.WriteLine($@"Using {agentLabel}. Switch service: \openai \gemini, \azure, or \ollama");
while (true)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("You: ");
    Console.ResetColor();

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
            case @"\openai":
                chatService = kernel.GetRequiredService<IChatCompletionService>("openai");
                settings = openAiPromptExecutionSettings;
                agentLabel = "OpenAI";
                break;
            case @"\clear":
            case @"\c":
                Console.Clear();
                Console.WriteLine("clearing message context...\n");
                chatHistory.Clear();
                continue;
            //case @"\claude":
            //    chatService = kernel.GetRequiredService<IChatCompletionService>("claude");
            //    settings = googlePromptExecutionSettings;
            //    agentLabel = "Claude";
            //    break;
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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{agentLabel}: ");
            Console.ResetColor();
            await foreach (StreamingChatMessageContent update in chatService.GetStreamingChatMessageContentsAsync(
                               chatHistory,
                               settings,
                               kernel))
            {
                sb.Append(update);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(update);
                Console.ResetColor();
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