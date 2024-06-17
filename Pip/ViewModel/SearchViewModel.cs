using System.Collections.ObjectModel;

namespace Pip.ViewModel;

public class SearchViewModel : ViewModelBase
{
    private ObservableCollection<TreasuryItemViewModel> _searchResults = [];
    private string? _searchText;

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
