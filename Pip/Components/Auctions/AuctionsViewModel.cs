using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Xpf.Docking.Base;
using JetBrains.Annotations;
using Pip.DataAccess.Services;
using Pip.Model;
using Pip.UI.Components.Details;
using Pip.UI.ViewModel;

namespace Pip.UI.Components.Auctions;

[GenerateViewModel]
public partial class AuctionsViewModel(ITreasuryDataProvider treasuryDataProvider, DetailsViewModel detailsViewModel)
	: PipViewModel
{
	[GenerateProperty] private Treasury? _selectedTreasuryRecent;

	[GenerateProperty] private Treasury? _selectedTreasuryUpcoming;

	[GenerateProperty] private IEnumerable<Treasury> _treasuriesRecent = [];

	[GenerateProperty] private IEnumerable<Treasury> _treasuriesUpcoming = [];

	public DetailsViewModel DetailsViewModel => detailsViewModel;

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
}