using Microsoft.Extensions.Logging;

namespace Td.Components;

[UsedImplicitly]
public partial class SiteSettingsPanel
{
	private bool _ltr = true;
	private bool _popVisible;
	private string? _status;
	private FluentDesignTheme? _theme;

	[Inject] public required ILogger<SiteSettingsPanel> Logger { get; set; }

	[Inject] public required GlobalState GlobalState { get; set; }

	private DesignThemeModes Mode { get; set; }

	private OfficeColor? OfficeColor { get; set; }

	private LocalizationDirection? Direction { get; set; }

	private static IEnumerable<DesignThemeModes> AllModes => Enum.GetValues<DesignThemeModes>();


	protected override void OnAfterRender(bool firstRender)
	{
		if (!firstRender) return;
		Direction = GlobalState.Dir;
		_ltr = Direction is null or LocalizationDirection.LeftToRight;
	}

	protected void HandleDirectionChanged(bool isLeftToRight)
	{
		_ltr = isLeftToRight;
		Direction = isLeftToRight ? LocalizationDirection.LeftToRight : LocalizationDirection.RightToLeft;
	}

	private Task ResetSiteAsync()
	{
		const string msg = "Site settings reset and cache cleared!";

		//await CacheStorageAccessor.RemoveAllAsync();
		_theme?.ClearLocalStorageAsync();

		Logger.LogInformation(msg);
		_status = msg;

		OfficeColor = OfficeColorUtilities.GetRandom();
		Mode = DesignThemeModes.System;
		return Task.CompletedTask;
	}

	private static string? GetCustomColor(OfficeColor? color)
	{
		return color switch
		{
			null => OfficeColorUtilities.GetRandom().ToAttributeValue(),
			Microsoft.FluentUI.AspNetCore.Components.OfficeColor.Default => "#036ac4",
			_ => color.ToAttributeValue()
		};
	}
}