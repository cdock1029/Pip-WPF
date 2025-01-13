namespace Td.Components;

public partial class SiteSettings
{
	private IDialogReference? _dialog;

	private async Task OpenSiteSettingsAsync()
	{
		_dialog = await DialogService.ShowPanelAsync<SiteSettingsPanel>(new DialogParameters
		{
			ShowTitle = true,
			Title = "Site settings",
			Alignment = HorizontalAlignment.Right,
			PrimaryAction = "OK",
			SecondaryAction = null,
			ShowDismiss = true
		});

		var _ = await _dialog.Result;
	}
}