using System.Collections.ObjectModel;

namespace Pip.ViewModel;

public class SearchViewModel : ViewModelBase
{
    private string? _searchText;
    private ObservableCollection<TreasuryItemViewModel> _searchResults = [];

    public string? SearchText
    {
        get => _searchText;
        set => SetField(ref _searchText, value);
    }

    public ObservableCollection<TreasuryItemViewModel> SearchResults
    {
        get => _searchResults;
        set => SetField(ref _searchResults, value);
    }
}