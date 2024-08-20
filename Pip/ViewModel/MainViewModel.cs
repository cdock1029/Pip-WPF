using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Pip.UI.Messages;
using Pip.UI.View.Services;

namespace Pip.UI.ViewModel;

public partial class MainViewModel(
    INavigationService navigationService,
    SearchViewModel searchViewModel,
    SavedTreasuriesViewModel savedTreasuriesViewModel,
    UpcomingAuctionsViewModel upcomingAuctionsViewModel)
    : ViewModelBase, IRecipient<AfterTreasuryInsertMessage>
{
    public INavigationService Navigation { get; } = navigationService;

    public SearchViewModel SearchViewModel { get; } = searchViewModel;
    public SavedTreasuriesViewModel SavedTreasuriesViewModel { get; } = savedTreasuriesViewModel;
    public UpcomingAuctionsViewModel UpcomingAuctionsViewModel { get; } = upcomingAuctionsViewModel;

    public void Receive(AfterTreasuryInsertMessage message)
    {
        //SelectViewModel(SavedTreasuriesViewModel);
    }


    [RelayCommand]
    private void NavigateSearch()
    {
        Navigation.NavigateTo<SearchViewModel>();
    }

    [RelayCommand]
    private void NavigateSaved()
    {
        Navigation.NavigateTo<SavedTreasuriesViewModel>();
    }

    [RelayCommand]
    private void NavigateUpcoming()
    {
        Navigation.NavigateTo<UpcomingAuctionsViewModel>();
    }

    [RelayCommand]
    private void NavigateAuctions()
    {
        Navigation.NavigateTo<AuctionsViewModel>();
    }
}
