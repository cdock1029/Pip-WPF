using System.Collections.ObjectModel;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pip.Model;
using Pip.UI.Services;
using Pip.UI.ViewModel;

namespace Pip.UI.Components.Auctions;

public partial class AuctionsViewModel(ITreasuryDataProvider treasuryDataProvider) : ViewModelBase
{
	[ObservableProperty] private Treasury? _selectedTreasuryRecent;

	[ObservableProperty] private Treasury? _selectedTreasuryUpcoming;

	public ObservableCollection<Treasury> TreasuriesRecent { get; } = [];
	public ObservableCollection<Treasury> TreasuriesUpcoming { get; } = [];

	[RelayCommand]
	public override async Task LoadAsync()
	{
		if (TreasuriesRecent.Any() || TreasuriesUpcoming.Any()) return;
		var dispatcher = Dispatcher.CurrentDispatcher;

		var taskRecent = Task.Run(async () =>
		{
			var recent = await treasuryDataProvider.GetAuctionsAsync().ConfigureAwait(false);
			if (recent is not null)
				dispatcher.BeginInvoke(() =>
				{
					foreach (var treasury in recent)
						TreasuriesRecent.Add(treasury);
				});
		});
		var taskUpcoming = Task.Run(async () =>
		{
			var upcoming = await treasuryDataProvider.GetUpcomingAsync().ConfigureAwait(false);
			if (upcoming is not null)
				dispatcher.BeginInvoke(() =>
				{
					foreach (var treasury in upcoming)
						TreasuriesUpcoming.Add(treasury);
				});
		});
		await Task.WhenAll(taskRecent, taskUpcoming).ConfigureAwait(false);
	}
}
