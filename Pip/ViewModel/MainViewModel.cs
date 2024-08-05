using Pip.UI.Command;

namespace Pip.UI.ViewModel;

public class MainViewModel : ViewModelBase
{
    private ViewModelBase? _selectedViewModel;

    public MainViewModel(SearchViewModel searchViewModel,
        TreasuriesViewModel treasuriesViewModel,
        UpcomingAuctionsViewModel upcomingAuctionsViewModel,
        AuctionsViewModel auctionsViewModel)
    {
        SearchViewModel = searchViewModel;
        TreasuriesViewModel = treasuriesViewModel;
        UpcomingAuctionsViewModel = upcomingAuctionsViewModel;
        AuctionsViewModel = auctionsViewModel;

        SelectedViewModel = SearchViewModel;
        SelectViewModelCommand = new DelegateCommand(SelectViewModel);
    }

    public ViewModelBase? SelectedViewModel
    {
        get => _selectedViewModel;
        private set => SetProperty(ref _selectedViewModel, value);
    }

    public SearchViewModel SearchViewModel { get; }

    public TreasuriesViewModel TreasuriesViewModel { get; }

    public UpcomingAuctionsViewModel UpcomingAuctionsViewModel { get; }
    public AuctionsViewModel AuctionsViewModel { get; }

    public DelegateCommand SelectViewModelCommand { get; }

    public override async Task LoadAsync()
    {
        if (SelectedViewModel is not null) await SelectedViewModel.LoadAsync();
    }

    private void SelectViewModel(object? parameter)
    {
        SelectedViewModel = parameter as ViewModelBase;
        //await LoadAsync();
    }
}
