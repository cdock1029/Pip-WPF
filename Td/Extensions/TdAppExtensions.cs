using DialogResult = Microsoft.FluentUI.AspNetCore.Components.DialogResult;

namespace Td.Extensions;

public static class TdAppExtensions
{
	public static async Task<DialogResult> OpenCreateInvestmentDialogAsync(this IDialogService dialogService,
		TreasuryItemViewModel treasuryItem)
	{
		Investment investment = new()
		{
			Cusip = treasuryItem.Cusip!,
			IssueDate = treasuryItem.IssueDate!.Value,
			MaturityDate = treasuryItem.MaturityDate,
			AuctionDate = treasuryItem.AuctionDate,
			Type = treasuryItem.Type!.Value,
			SecurityTerm = treasuryItem.Term
		};

		IDialogReference dialog = await dialogService.ShowDialogAsync<InvestmentFormDialog>(investment,
			new DialogParameters
			{
				Title = "Create Investment",
				PrimaryAction = "Save",
				PrimaryActionEnabled = true,
				SecondaryAction = "Cancel",
				Width = "500px",
				TrapFocus = true,
				Modal = true,
				PreventScroll = true
			});
		DialogResult result = await dialog.Result;
		return result;
	}
}