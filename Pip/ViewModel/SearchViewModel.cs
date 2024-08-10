using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pip.UI.Data;

namespace Pip.UI.ViewModel;

public partial class SearchViewModel(ITreasuryDataProvider treasuryDataProvider) : ViewModelBase
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
}
