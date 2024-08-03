using System.Collections.ObjectModel;
using Pip.UI.Command;
using Pip.UI.Data;

namespace Pip.UI.ViewModel;

public class SearchViewModel : ViewModelBase
{
    private readonly ITreasuryDataProvider _treasuryDataProvider;
    private ObservableCollection<TreasuryItemViewModel> _searchResults = [];
    private string? _searchText;

    public SearchViewModel(ITreasuryDataProvider treasuryDataProvider)
    {
        _treasuryDataProvider = treasuryDataProvider;
        GetDataCommand = new DelegateCommand(GetData, CanGetData);
    }

    public string? SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            SearchResults = [];
            GetDataCommand.RaiseCanExecuteChanged();
        }
    }

    public ObservableCollection<TreasuryItemViewModel> SearchResults
    {
        get => _searchResults;
        private set => SetProperty(ref _searchResults, value);
    }

    public DelegateCommand GetDataCommand { get; }

    private bool CanGetData(object? parameter)
    {
        return !string.IsNullOrWhiteSpace(SearchText);
    }

    private async void GetData(object? parameter)
    {
        ArgumentNullException.ThrowIfNull(parameter);
        var cusip = parameter as string;

        var treasuries = await _treasuryDataProvider.SearchTreasuriesAsync(cusip!);

        if (treasuries == null) return;
        foreach (var treasury in treasuries)
            SearchResults.Add(new TreasuryItemViewModel(treasury));
    }
}
