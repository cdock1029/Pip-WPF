using CommunityToolkit.Mvvm.Input;
using Pip.UI.View.Services;

namespace Pip.UI.ViewModel;

public partial class MainViewModel(
	INavigationService navigationService,
	SearchViewModel searchViewModel,
	SavedTreasuriesViewModel savedTreasuriesViewModel,
	AuctionsViewModel auctionsViewModel)
	: ViewModelBase
{
	public INavigationService Navigation { get; } = navigationService;

	public SearchViewModel SearchViewModel { get; } = searchViewModel;
	public SavedTreasuriesViewModel SavedTreasuriesViewModel { get; } = savedTreasuriesViewModel;
	public AuctionsViewModel AuctionsViewModel { get; } = auctionsViewModel;


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
	private void NavigateAuctions()
	{
		Navigation.NavigateTo<AuctionsViewModel>();
	}
}
