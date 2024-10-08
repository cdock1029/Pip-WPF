using DevExpress.Mvvm.CodeGenerators;
using Pip.UI.Components.Auctions;
using Pip.UI.Components.Investments;
using Pip.UI.Components.Search;

namespace Pip.UI.ViewModel;

[GenerateViewModel]
public partial class MainViewModel(
	InvestmentsViewModel investmentsViewModel,
	SearchViewModel searchViewModel,
	AuctionsViewModel auctionsViewModel
)
	: PipViewModel
{
	public AuctionsViewModel AuctionsViewModel => auctionsViewModel;
	public SearchViewModel SearchViewModel => searchViewModel;
	public InvestmentsViewModel InvestmentsViewModel => investmentsViewModel;
}