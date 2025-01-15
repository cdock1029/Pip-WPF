using System.Diagnostics;
using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Xpf.Accordion;
using DevExpress.Xpf.Core.Native;
using Pip.UI.Components.Auctions;
using Pip.UI.Components.Details;
using Pip.UI.Components.Home;
using Pip.UI.Components.Investments;
using Pip.UI.Components.Search;

namespace Pip.UI.ViewModel;

[GenerateViewModel]
public partial class MainViewModel(
	InvestmentsViewModel investmentsViewModel,
	SearchViewModel searchViewModel,
	AuctionsViewModel auctionsViewModel,
	DetailsViewModel detailsViewModel,
	HomeViewModel homeViewModel
)
	: PipViewModel
{
	private AuctionsViewModel AuctionsViewModel => auctionsViewModel;
	public SearchViewModel SearchViewModel => searchViewModel;
	private InvestmentsViewModel InvestmentsViewModel => investmentsViewModel;
	public DetailsViewModel DetailsViewModel => detailsViewModel;
	public HomeViewModel HomeViewModel => homeViewModel;

	private INavigationService NavigationService => GetService<INavigationService>();


	[GenerateCommand]
	private void ShowForm()
	{
		var model = new InvestmentItemViewModel
		{
			Cusip = "",
			IssueDate = DateTime.Now.ToDateOnly()
		};

		var result = DialogService.ShowDialog(MessageButton.OKCancel, "Investment form", nameof(InvestmentForm), model);

		Debug.WriteLine(
			$"result: {result}, model par: {model.Par}, confirmation: {model.Confirmation}, re-investments: {model.Reinvestments}");
	}

	[GenerateCommand]
	private void NavigateHome()
	{
		NavigationService.Navigate(nameof(HomeView), HomeViewModel);
	}

	[GenerateCommand]
	private void NavigateAuctions()
	{
		NavigationService.Navigate(nameof(AuctionsView), AuctionsViewModel);
	}

	[GenerateCommand]
	private void NavigateInvestments()
	{
		NavigationService.Navigate(nameof(InvestmentsView), InvestmentsViewModel);
	}

	[GenerateCommand]
	private void HandleSelectedItemChanged(AccordionSelectedItemChangedEventArgs args)
	{
		Debug.WriteLine($"triggered {args.NewItem}");
	}
}