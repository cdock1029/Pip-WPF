using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Pip.Model;
using Pip.UI.Data;

namespace Pip.UI.ViewModel;

public partial class AuctionsViewModel(ITreasuryDataProvider treasuryDataProvider) : ViewModelBase
{
	public ObservableCollection<Treasury> TreasuriesRecent { get; } = [];
	public Treasury? SelectedTreasuryRecent { get; set; }
	public ObservableCollection<Treasury> TreasuriesUpcoming { get; } = [];
	public Treasury? SelectedTreasuryUpcoming { get; set; }

	[RelayCommand]
	public override async Task LoadAsync()
	{
		if (TreasuriesRecent.Any() || TreasuriesUpcoming.Any()) return;

		var recentTask = treasuryDataProvider.GetAuctionsAsync();
		var upcomingTask = treasuryDataProvider.GetUpcomingAsync();
		var tasks = await Task.WhenAll(recentTask, upcomingTask);
		var recent = tasks[0];
		var upcoming = tasks[1];

		if (recent is not null)
			foreach (var auction in recent)
				TreasuriesRecent.Add(auction);
		if (upcoming is not null)
			foreach (var auction in upcoming)
				TreasuriesUpcoming.Add(auction);
	}
}
