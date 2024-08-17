using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Pip.Model;
using Pip.UI.Data;
using Pip.UI.Messages;
using Pip.UI.View.Services;

namespace Pip.UI.ViewModel;

public partial class SearchViewModel : ViewModelBase
{
    private readonly IMessageDialogService _messageDialogService;

    private readonly ITreasuryDataProvider _treasuryDataProvider;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
    private string? _searchText;

    public SearchViewModel(ITreasuryDataProvider treasuryDataProvider,
        IMessageDialogService messageDialogService)
    {
        _treasuryDataProvider = treasuryDataProvider;
        _messageDialogService = messageDialogService;
        SearchResults.CollectionChanged += SearchResults_CollectionChanged;
    }

    public ObservableCollection<Treasury> SearchResults { get; } = [];

    private void SearchResults_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        ClearResultsCommand.NotifyCanExecuteChanged();
    }


    [RelayCommand(CanExecute = nameof(CanSearch))]
    private async Task Search(string? cusip)
    {
        ArgumentNullException.ThrowIfNull(cusip);

        var treasuries = await _treasuryDataProvider.SearchTreasuriesAsync(cusip.Trim());

        SearchResults.Clear();
        if (treasuries == null) return;
        foreach (var treasury in treasuries)
            SearchResults.Add(treasury);
    }

    private bool CanSearch()
    {
        return !string.IsNullOrWhiteSpace(SearchText);
    }

    [RelayCommand]
    private async Task SaveTreasury(Treasury? treasury)
    {
        ArgumentNullException.ThrowIfNull(treasury);
        var result = _messageDialogService.ShowOkCancelDialog(
            $"CUSIP: [{treasury.Cusip}]\nIssue Date: [{treasury.IssueDate.ToLongDateString()}]\nMaturity Date: [{treasury.MaturityDate?.ToLongDateString()}]",
            "Do you want to save Treasury?");

        if (result == MessageDialogResult.OK)
        {
            await _treasuryDataProvider.InsertAsync(treasury);
            WeakReferenceMessenger.Default.Send(
                new AfterTreasuryInsertMessage(new AfterTreasuryInsertArgs(treasury.Cusip, treasury.IssueDate)));
        }
    }

    [RelayCommand(CanExecute = nameof(CanClearResults))]
    private void ClearResults()
    {
        SearchResults.Clear();
    }

    private bool CanClearResults()
    {
        return SearchResults.Count > 0;
    }
}
