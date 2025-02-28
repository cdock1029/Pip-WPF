using JetBrains.Annotations;
using Microsoft.SemanticKernel;

namespace Pip.Console.Plugins;

[UsedImplicitly]
public sealed class UtilitiesPlugin
{
    [KernelFunction]
    [UsedImplicitly]
    public string GetCurrentUtcTime()
    {
        return DateTime.UtcNow.ToString("R");
    }
}