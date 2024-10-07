using DevExpress.Mvvm.CodeGenerators;
using Pip.UI.Components.Auctions;
using Pip.UI.Components.Investments;
using Pip.UI.Components.Search;
using Pip.UI.Services;

namespace Pip.UI.ViewModel;

[GenerateViewModel]
public partial class MainViewModel(
	INavigationService navigationService)
	: PipViewModel
{
	public INavigationService NavigationService { get; } = navigationService;

	[GenerateCommand]
	private async Task NavigateSearch()
	{
		await NavigationService.NavigateToAsync<SearchViewModel>();
	}

	[GenerateCommand]
	private async Task NavigateInvestments()
	{
		await NavigationService.NavigateToAsync<InvestmentsViewModel>();
	}

	[GenerateCommand]
	private async Task NavigateAuctions()
	{
		await NavigationService.NavigateToAsync<AuctionsViewModel>();
	}
}