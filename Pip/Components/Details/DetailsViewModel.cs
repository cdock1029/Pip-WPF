using System.Diagnostics;
using DevExpress.Mvvm.CodeGenerators;
using Pip.UI.Components.Shared;
using Pip.UI.Data.Services;
using Pip.UI.Models;

namespace Pip.UI.Components.Details;

[GenerateViewModel]
public partial class DetailsViewModel(ITreasuryDataProvider treasuryDataProvider) : PipViewModel
{
    private CancellationTokenSource? _tokenSource;
    [GenerateProperty] private Treasury? _treasuryDetailsSelected;

    [GenerateCommand]
    private void HandleSelectionChanged(Treasury? selectedItem)
    {
        TreasuryDetailsSelected = selectedItem;
    }

    [GenerateCommand]
    private void HandleSelectedInvestmentChanged(Investment? selected)
    {
        _tokenSource?.Cancel();

        if (selected?.Cusip is null)
        {
            TreasuryDetailsSelected = null;
            return;
        }

        _tokenSource = new CancellationTokenSource();
        try
        {
            CancellationToken ct = _tokenSource.Token;
            ValueTask<Treasury?> treasuryTask =
                treasuryDataProvider.LookupTreasuryAsync(selected.Cusip, selected.IssueDate, ct);
            if (treasuryTask.IsCompleted)
                TreasuryDetailsSelected = treasuryTask.Result;
            else
                Dispatcher.BeginInvoke(async () =>
                {
                    Treasury? treasury = await treasuryTask;
                    if (!ct.IsCancellationRequested) TreasuryDetailsSelected = treasury;
                });
        }
        catch (OperationCanceledException)
        {
            Debug.WriteLine($"Operation canceled for cusip {selected.Cusip}");
        }
    }
}