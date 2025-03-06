using JetBrains.Annotations;
using Microsoft.SemanticKernel;

namespace Pip.DataAccess.Plugins;

[UsedImplicitly]
public sealed class UtilitiesPlugin
{
    [KernelFunction("get_current_utc_time")]
    [UsedImplicitly]
    public string GetCurrentUtcTime()
    {
        return DateTime.UtcNow.ToString("R");
    }
}