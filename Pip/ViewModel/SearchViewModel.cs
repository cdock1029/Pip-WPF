using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using Pip.UI.Command;
using Pip.UI.Model;

namespace Pip.UI.ViewModel;

public class SearchViewModel : ViewModelBase
{
    private ObservableCollection<TreasuryItemViewModel> _searchResults = [];
    private string? _searchText;

    public SearchViewModel()
    {
        GetDataCommand = new DelegateCommand(GetData, CanGetData);
    }

    public string? SearchText
    {
        get => _searchText;
        set
        {
            SetField(ref _searchText, value);
            SearchResults = [];
            GetDataCommand.RaiseCanExecuteChanged();
        }
    }

    public ObservableCollection<TreasuryItemViewModel> SearchResults
    {
        get => _searchResults;
        private set => SetField(ref _searchResults, value);
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

        using HttpClient client = new();
        client.BaseAddress = new Uri("https://www.treasurydirect.gov/TA_WS/");

        var treasuries =
            await client.GetFromJsonAsync<IEnumerable<Treasury>>(
                $"securities/search/?format=json&cusip={cusip}");

        if (treasuries is not null)
            foreach (var treasury in treasuries)
                SearchResults.Add(new TreasuryItemViewModel(treasury));
    }
}
