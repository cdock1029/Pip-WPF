using System.Collections.ObjectModel;
using System.Diagnostics;
using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using Pip.UI.Components.Details;
using Pip.UI.Components.Investments.Messages;
using Pip.UI.Components.Shared;
using Pip.UI.Data.Services;
using Pip.UI.Models;

namespace Pip.UI.Components.Search;

[GenerateViewModel]
public partial class SearchViewModel(
    ITreasuryDataProvider treasuryDataProvider,
    DetailsViewModel detailsViewModel)
    : PipViewModel
{
    [GenerateProperty] private ObservableCollection<TreasuryItemViewModel>? _searchResults;
    [GenerateProperty] private TreasuryItemViewModel? _selectedTreasuryItem;

    public string? SearchText
    {
        get;
        set
        {
            field = value;
            if (string.IsNullOrWhiteSpace(field) && SearchResults != null && SearchResults.Any()) SearchResults = null;

            RaisePropertyChanged();
        }
    }

    public DetailsViewModel DetailsViewModel { get; } = detailsViewModel;

    [GenerateCommand]
    private async Task Search()
    {
        Debug.WriteLine("\nSearching..");
        ArgumentException.ThrowIfNullOrWhiteSpace(SearchText);

        IEnumerable<Treasury>? treasuries = await treasuryDataProvider.SearchTreasuriesAsync(SearchText.Trim());

        SearchResults = [];
        if (treasuries == null) return;
        foreach (Treasury treasury in treasuries)
            SearchResults.Add(new TreasuryItemViewModel(treasury));

        Debug.WriteLine("\nSearch complete");
    }

    private bool CanSearch()
    {
        string? trimmed = SearchText?.Trim();

        if (string.IsNullOrEmpty(trimmed)) return false;

        return trimmed.Length == 9;
    }

    [GenerateCommand]
    private void CreateInvestment()
    {
        ArgumentNullException.ThrowIfNull(SelectedTreasuryItem);

        Treasury treasury = SelectedTreasuryItem.Treasury;

        Investment investment = new()
        {
            Cusip = treasury.Cusip,
            IssueDate = treasury.IssueDate!.Value,
            MaturityDate = treasury.MaturityDate,
            SecurityTerm = treasury.SecurityTerm,
            Type = treasury.Type
        };
        treasuryDataProvider.Insert(investment);
        Messenger.Default.Send(
            new AfterInsertInvestmentMessage(new AfterInsertInvestmentArgs(investment.Id)));
    }

    private bool CanCreateInvestment()
    {
        return SelectedTreasuryItem is not null;
    }

    [GenerateCommand]
    private void Clear()
    {
        SearchText = null;
        SearchResults = null;
    }
}