using CommunityToolkit.Mvvm.Input;
using Pip.UI.Components.Auctions;
using Pip.UI.Components.Investments;
using Pip.UI.Components.SavedTreasuries;
using Pip.UI.Components.Search;
using Pip.UI.Services;

namespace Pip.UI.ViewModel;

public partial class MainViewModel(
	INavigationService navigationService)
	: ViewModelBase
{
	public INavigationService Navigation { get; } = navigationService;


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
	private void NavigateInvestments()
	{
		Navigation.NavigateTo<InvestmentsViewModel>();
	}

	[RelayCommand]
	private void NavigateAuctions()
	{
		Navigation.NavigateTo<AuctionsViewModel>();
	}

	/*
	[RelayCommand]
	private Task NavigateSearch()
	{
		await Navigation.NavigateToAsync<SearchViewModel>();
	}

	[RelayCommand]
	private async Task NavigateSaved()
	{
		await Navigation.NavigateToAsync<SavedTreasuriesViewModel>();
	}

	[RelayCommand]
	private async Task NavigateInvestments()
	{
		await Navigation.NavigateToAsync<InvestmentsViewModel>();
	}

	[RelayCommand]
	private async Task NavigateAuctions()
	{
		await Navigation.NavigateToAsync<AuctionsViewModel>();
	}
	*/
}
