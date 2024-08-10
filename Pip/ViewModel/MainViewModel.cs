using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Pip.UI.ViewModel;

public partial class MainViewModel(
    SearchViewModel searchViewModel,
    TreasuriesViewModel treasuriesViewModel,
    UpcomingAuctionsViewModel upcomingAuctionsViewModel,
    AuctionsViewModel auctionsViewModel)
    : ViewModelBase
{
    [ObservableProperty] private ViewModelBase _selectedViewModel = searchViewModel;


    public SearchViewModel SearchViewModel { get; } = searchViewModel;

    public TreasuriesViewModel TreasuriesViewModel { get; } = treasuriesViewModel;

    public UpcomingAuctionsViewModel UpcomingAuctionsViewModel { get; } = upcomingAuctionsViewModel;
    public AuctionsViewModel AuctionsViewModel { get; } = auctionsViewModel;


    public override async Task LoadAsync()
    {
        await SelectedViewModel.LoadAsync();
    }

    [RelayCommand]
    private void SelectViewModel(ViewModelBase viewModelBase)
    {
        SelectedViewModel = viewModelBase;
    }
}
