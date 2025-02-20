using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Xpf.Core;
using Pip.DataAccess.Services;
using Pip.Model;
using Pip.UI.Components.Details;
using Pip.UI.Messages;
using Pip.UI.ViewModel;

namespace Pip.UI.Components.Auctions;

[GenerateViewModel]
public partial class AuctionsViewModel(ITreasuryDataProvider treasuryDataProvider, DetailsViewModel detailsViewModel)
	: PipViewModel, IPipPage
{
	[GenerateProperty] private Treasury? _selectedTreasuryRecent;

	[GenerateProperty] private Treasury? _selectedTreasuryUpcoming;

	[GenerateProperty] private IEnumerable<Treasury> _treasuriesRecent = [];

	[GenerateProperty] private IEnumerable<Treasury> _treasuriesUpcoming = [];

	public DetailsViewModel DetailsViewModel => detailsViewModel;

	public string View => "AuctionsView";

	public string Title => "Auctions";

	//public Uri Image { get; } = DXImageHelper.GetImageUri("SvgImages/Business Objects/BO_Sale.svg");
	public Uri Image { get; } = DXImageHelper.GetImageUri("Images/Business Objects/BOSale_32x32.png");

	public override async Task LoadAsync()
	{
		var tasks = new[] { LoadRecent(), LoadUpcoming() };

		await foreach (var task in Task.WhenEach(tasks).ConfigureAwait(false))
		{
			var (treasuries, name) = task.Result;
			switch (name)
			{
				case "recent":
					Dispatcher.BeginInvoke(() => { TreasuriesRecent = treasuries ?? []; });
					break;
				case "upcoming":
					Dispatcher.BeginInvoke(() => { TreasuriesUpcoming = treasuries ?? []; });
					break;
			}
		}
	}

	private async Task<(IEnumerable<Treasury>?, string)> LoadRecent()
	{
		var recent = await treasuryDataProvider.GetRecentAsync().ConfigureAwait(false);
		return (recent, "recent");
	}

	private async Task<(IEnumerable<Treasury>?, string)> LoadUpcoming()
	{
		var upcoming = await treasuryDataProvider.GetUpcomingAsync().ConfigureAwait(false);
		return (upcoming, "upcoming");
	}


	[GenerateCommand]
	private void SaveRecentToInvestments()
	{
		ArgumentNullException.ThrowIfNull(SelectedTreasuryRecent);
		var investment = new Investment
		{
			Cusip = SelectedTreasuryRecent.Cusip,
			IssueDate = SelectedTreasuryRecent.IssueDate!.Value,
			MaturityDate = SelectedTreasuryRecent.MaturityDate,
			SecurityTerm = SelectedTreasuryRecent.SecurityTerm,
			Type = SelectedTreasuryRecent.Type
		};
		treasuryDataProvider.Insert(investment);
		Messenger.Default.Send(new AfterInsertInvestmentMessage(new AfterInsertInvestmentArgs(investment.Id)));
	}


	[GenerateCommand]
	private void SaveUpcomingtToInvestments()
	{
		ArgumentNullException.ThrowIfNull(SelectedTreasuryUpcoming);
		var investment = new Investment
		{
			Cusip = SelectedTreasuryUpcoming.Cusip,
			IssueDate = SelectedTreasuryUpcoming.IssueDate!.Value,
			MaturityDate = SelectedTreasuryUpcoming.MaturityDate,
			SecurityTerm = SelectedTreasuryUpcoming.SecurityTerm,
			Type = SelectedTreasuryUpcoming.Type
		};
		treasuryDataProvider.Insert(investment);
		Messenger.Default.Send(new AfterInsertInvestmentMessage(new AfterInsertInvestmentArgs(investment.Id)));
	}
}