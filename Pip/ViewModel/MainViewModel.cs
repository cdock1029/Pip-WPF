using Pip.UI.Components.Auctions;
using Pip.UI.Components.Details;
using Pip.UI.Components.Investments;
using Pip.UI.Components.Search;

namespace Pip.UI.ViewModel;

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
}