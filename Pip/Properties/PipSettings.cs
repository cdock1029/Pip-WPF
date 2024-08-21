using System.Configuration;
using Pip.UI.View.Types;

namespace Pip.UI.Properties;

public class PipSettings : ApplicationSettingsBase
{
    [UserScopedSetting]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public WindowPlacement WindowPlacement
    {
        get => (WindowPlacement)this[nameof(WindowPlacement)];
        set => this[nameof(WindowPlacement)] = value;
    }
}
