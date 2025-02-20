using System.Diagnostics;
using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Xpf.Core.Native;
using Pip.UI.Components.Auctions;
using Pip.UI.Components.Details;
using Pip.UI.Components.Home;
using Pip.UI.Components.Investments;
using Pip.UI.Components.Search;
using Pip.UI.Messages;

namespace Pip.UI.ViewModel;

[GenerateViewModel]
public partial class MainViewModel : PipViewModel
{
	[GenerateProperty] private IPipPage? _selectedPage;

	public MainViewModel(InvestmentsViewModel investmentsViewModel,
		SearchViewModel searchViewModel,
		AuctionsViewModel auctionsViewModel,
		DetailsViewModel detailsViewModel,
		HomeViewModel homeViewModel)
	{
		SearchViewModel = searchViewModel;
		DetailsViewModel = detailsViewModel;
		InvestmentsViewModel = investmentsViewModel;
		HomeViewModel = homeViewModel;

		Messenger.Default.Register<AfterInsertInvestmentMessage>(this, ReceiveAfterInvestmentMessage);

		SelectedPage = homeViewModel;

		RootPipPageWrapper = new PipPageWrapper(homeViewModel, auctionsViewModel, investmentsViewModel);
	}

	public HomeViewModel HomeViewModel { get; }

	private InvestmentsViewModel InvestmentsViewModel { get; }

	public IEnumerable<IPipPage> RootItems => [RootPipPageWrapper];

	public PipPageWrapper RootPipPageWrapper { get; }

	public SearchViewModel SearchViewModel { get; }


	public DetailsViewModel DetailsViewModel { get; }


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
	private void NavigateToSelected()
	{
		NavigationService.Navigate(SelectedPage!.View, SelectedPage);
	}

	private void ReceiveAfterInvestmentMessage(AfterInsertInvestmentMessage msg)
	{
		SelectedPage = InvestmentsViewModel;
	}

	public class PipPageWrapper(
		params IEnumerable<IPipPage> pages) : IPipPage
	{
		public IEnumerable<IPipPage> Pages => pages;

		public string? View { get; } = null!;
		public string Title { get; set; } = "Navigation";
		public Uri? Image { get; } = null!;
	}
}