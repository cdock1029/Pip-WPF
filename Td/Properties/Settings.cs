using System.Configuration;

namespace Td.Properties;

public class Settings : ApplicationSettingsBase
{
	[UserScopedSetting]
	[SettingsSerializeAs(SettingsSerializeAs.Xml)]
	public WindowPlacement WindowPlacement
	{
		get => (WindowPlacement)this[nameof(WindowPlacement)];
		set => this[nameof(WindowPlacement)] = value;
	}

	[UserScopedSetting]
	public bool IsNavExpanded
	{
		get
		{
			try
			{
				var stored = this[nameof(IsNavExpanded)] as bool?;
				return stored is null || stored.Value;
			}
			catch
			{
				return true;
			}
		}
		set => this[nameof(IsNavExpanded)] = value;
	}
}