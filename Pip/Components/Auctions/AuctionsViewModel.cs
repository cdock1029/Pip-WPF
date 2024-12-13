using System.Collections.ObjectModel;
using DevExpress.Mvvm.CodeGenerators;
using Pip.DataAccess.Services;
using Pip.Model;
using Pip.UI.ViewModel;

namespace Pip.UI.Components.Auctions;

[GenerateViewModel]
public partial class AuctionsViewModel(ITreasuryDataProvider treasuryDataProvider) : PipViewModel
{
	[GenerateProperty] private Treasury? _selectedTreasuryRecent;

	[GenerateProperty] private Treasury? _selectedTreasuryUpcoming;

	public ObservableCollection<Treasury> TreasuriesRecent { get; } = [];
	public ObservableCollection<Treasury> TreasuriesUpcoming { get; } = [];

	public override Task LoadAsync()
	{
		Dispatcher.BeginInvoke(LoadRecent);
		Dispatcher.BeginInvoke(LoadUpcoming);

		return Task.CompletedTask;
	}

	private async Task LoadRecent()
	{
		if (TreasuriesRecent.Any()) return;
		var recent = await treasuryDataProvider.GetAuctionsAsync();
		if (recent is not null)
			foreach (var treasury in recent)
				TreasuriesRecent.Add(treasury);
	}

	private async Task LoadUpcoming()
	{
		if (TreasuriesUpcoming.Any()) return;
		var upcoming = await treasuryDataProvider.GetUpcomingAsync();
		if (upcoming is not null)
			foreach (var treasury in upcoming)
				TreasuriesUpcoming.Add(treasury);
	}
}