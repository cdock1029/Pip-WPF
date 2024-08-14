using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Pip.UI.Data;
using Pip.UI.Messages;
using Pip.UI.View.Services;

namespace Pip.UI.ViewModel;

public partial class SearchViewModel(
    ITreasuryDataProvider treasuryDataProvider,
    IMessageDialogService messageDialogService) : ViewModelBase
{
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(GetDataCommand))]
    private string? _searchText;

    public ObservableCollection<TreasuryItemViewModel> SearchResults { get; } = [];


    [RelayCommand(CanExecute = nameof(CanGetData))]
    private async Task GetData(string? cusip)
    {
        ArgumentNullException.ThrowIfNull(cusip);

        var treasuries = await treasuryDataProvider.SearchTreasuriesAsync(cusip);

        SearchResults.Clear();
        if (treasuries == null) return;
        foreach (var treasury in treasuries)
            SearchResults.Add(new TreasuryItemViewModel(treasury));
    }

    private bool CanGetData()
    {
        return !string.IsNullOrWhiteSpace(SearchText);
    }

    [RelayCommand]
    private async Task SaveTreasury(TreasuryItemViewModel? treasury)
    {
        ArgumentNullException.ThrowIfNull(treasury);
        var result = messageDialogService.ShowOkCancelDialog(
            $"CUSIP: [{treasury.Cusip}]\nIssue Date: [{treasury.IssueDate.ToLongDateString()}]\nMaturity Date: [{treasury.MaturityDate?.ToLongDateString()}]",
            "Do you want to save Treasury?");

        if (result == MessageDialogResult.OK)
        {
            await treasuryDataProvider.InsertAsync(treasury.Model);
            WeakReferenceMessenger.Default.Send(
                new AfterTreasuryInsertMessage(new AfterTreasuryInsertArgs(treasury.Cusip, treasury.IssueDate)));
        }
    }
}
