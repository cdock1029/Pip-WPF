namespace Pip.Console;

public class AzureAiConfig
{
    public string VertexBearerKey { get; set; } = null!;

    public string OpenAiApiKey { get; init; } = null!;

    public string AzureKeyCredential { get; init; } = null!;

    public string GeminiKey { get; init; } = null!;

    public string ModelInferenceEndpoint { get; init; } = null!;

    public string ModelId { get; init; } = null!;

    public string OllamaEndpoint { get; init; } = null!;

    public string AnthropicSecretKey { get; init; } = null!;
}