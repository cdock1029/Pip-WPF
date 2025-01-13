using System.Diagnostics;
using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Xpf.Core.Native;
using Pip.UI.Components.Auctions;
using Pip.UI.Components.Details;
using Pip.UI.Components.Investments;
using Pip.UI.Components.Search;

namespace Pip.UI.ViewModel;

[GenerateViewModel]
public partial class MainViewModel(
	InvestmentsViewModel investmentsViewModel,
	SearchViewModel searchViewModel,
	AuctionsViewModel auctionsViewModel,
	DetailsViewModel detailsViewModel
)
	: PipViewModel
{
	public AuctionsViewModel AuctionsViewModel => auctionsViewModel;
	public SearchViewModel SearchViewModel => searchViewModel;
	public InvestmentsViewModel InvestmentsViewModel => investmentsViewModel;
	public DetailsViewModel DetailsViewModel => detailsViewModel;

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
}