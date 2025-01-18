using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking.Base;
using JetBrains.Annotations;
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
	public Uri Image { get; } = DXImageHelper.GetImageUri("SvgImages/Business Objects/BO_Sale.svg");

	[UsedImplicitly]
	public async Task LoadRecent()
	{
		var recent = await treasuryDataProvider.GetRecentAsync().ConfigureAwait(false);
		Dispatcher.BeginInvoke(() => { TreasuriesRecent = recent ?? []; });
	}

	[UsedImplicitly]
	public async Task LoadUpcoming()
	{
		var upcoming = await treasuryDataProvider.GetUpcomingAsync().ConfigureAwait(false);
		Dispatcher.BeginInvoke(() => { TreasuriesUpcoming = upcoming ?? []; });
	}

	[UsedImplicitly]
	public async Task HandleSelectedItem(SelectedItemChangedEventArgs args)
	{
		var name = args.Item.Name;

		switch (name)
		{
			case "RecentTab":
				await LoadRecent();
				break;

			case "UpcomingTab":
				await LoadUpcoming();
				break;
			default:
				throw new ArgumentException("Unknown tab name");
		}
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