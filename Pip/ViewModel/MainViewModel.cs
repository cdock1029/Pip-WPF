using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Pip.UI.Messages;
using Pip.UI.View.Services;

namespace Pip.UI.ViewModel;

public partial class MainViewModel(
    INavigationService navigationService)
    : ViewModelBase, IRecipient<AfterTreasuryInsertMessage>
{
    public INavigationService Navigation { get; } = navigationService;

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
