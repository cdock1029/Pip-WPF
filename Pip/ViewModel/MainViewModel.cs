using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Pip.UI.Messages;

namespace Pip.UI.ViewModel;

public partial class MainViewModel(
    SearchViewModel searchViewModel,
    TreasuriesViewModel treasuriesViewModel,
    UpcomingAuctionsViewModel upcomingAuctionsViewModel,
    AuctionsViewModel auctionsViewModel)
    : ViewModelBase, IRecipient<AfterTreasuryInsertMessage>
{
    [ObservableProperty] private ViewModelBase _selectedViewModel = searchViewModel;


    public SearchViewModel SearchViewModel { get; } = searchViewModel;

    public TreasuriesViewModel TreasuriesViewModel { get; } = treasuriesViewModel;

    public UpcomingAuctionsViewModel UpcomingAuctionsViewModel { get; } = upcomingAuctionsViewModel;
    public AuctionsViewModel AuctionsViewModel { get; } = auctionsViewModel;

    public void Receive(AfterTreasuryInsertMessage message)
    {
        SelectViewModelCommand.Execute(TreasuriesViewModel);
    }


    public override async Task LoadAsync()
    {
        SelectedViewModel.IsActive = true;
        await SelectedViewModel.LoadAsync();
    }

    [RelayCommand]
    private void SelectViewModel(ViewModelBase viewModelBase)
    {
        SelectedViewModel = viewModelBase;
    }
}
