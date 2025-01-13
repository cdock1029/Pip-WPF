namespace Td.Extensions;

public static class TdAppExtensions
{
	public static async Task<DialogResult> OpenCreateInvestmentDialogAsync(this IDialogService dialogService,
		TreasuryItemViewModel treasuryItem)
	{
		var investment = new Investment
		{
			Cusip = treasuryItem.Cusip,
			IssueDate = treasuryItem.IssueDate,
			MaturityDate = treasuryItem.MaturityDate,
			AuctionDate = treasuryItem.AuctionDate,
			Type = treasuryItem.Type,
			SecurityTerm = treasuryItem.Term
		};

		var dialog = await dialogService.ShowDialogAsync<InvestmentFormDialog>(investment, new DialogParameters
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
		var result = await dialog.Result;
		return result;
	}
}