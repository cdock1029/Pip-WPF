using Pip.Web.Client.Components;

namespace Pip.Web.Client.Utils;

public static class Extensions
{
    public static async Task<DialogResult> OpenCreateInvestmentDialogAsync(this IDialogService dialogService,
        Treasury treasury)
    {
        Investment investment = new()
        {
            Cusip = treasury.Cusip!,
            IssueDate = treasury.IssueDate!.Value,
            MaturityDate = treasury.MaturityDate,
            AuctionDate = treasury.AuctionDate,
            Type = treasury.Type,
            SecurityTerm = treasury.SecurityTerm
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
