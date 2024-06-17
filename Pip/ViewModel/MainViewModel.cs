using Pip.Command;

namespace Pip.ViewModel;

public class MainViewModel : ViewModelBase
{
    private ViewModelBase? _selectedViewModel;

    public MainViewModel(SearchViewModel searchViewModel,
        TreasuriesViewModel treasuriesViewModel,
        AnnouncedViewModel announcedViewModel,
        AuctionsViewModel auctionsViewModel)
    {
        SearchViewModel = searchViewModel;
        TreasuriesViewModel = treasuriesViewModel;
        AnnouncedViewModel = announcedViewModel;
        AuctionsViewModel = auctionsViewModel;

        SelectedViewModel = SearchViewModel;
        SelectViewModelCommand = new DelegateCommand(SelectViewModel);
    }

    public ViewModelBase? SelectedViewModel
    {
        get => _selectedViewModel;
        set => SetField(ref _selectedViewModel, value);
    }

    public SearchViewModel SearchViewModel { get; }

    public TreasuriesViewModel TreasuriesViewModel { get; }

    public AnnouncedViewModel AnnouncedViewModel { get; }
    public AuctionsViewModel AuctionsViewModel { get; }

    public DelegateCommand SelectViewModelCommand { get; }

    public override async Task LoadAsync()
    {
        if (SelectedViewModel is not null) await SelectedViewModel.LoadAsync();
    }

    private async void SelectViewModel(object? parameter)
    {
        SelectedViewModel = parameter as ViewModelBase;
        await LoadAsync();
    }
}
