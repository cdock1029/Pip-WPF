using System.Collections.ObjectModel;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pip.Model;
using Pip.UI.Data;

namespace Pip.UI.ViewModel;

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
		var curr = Dispatcher.CurrentDispatcher;
		await Task.WhenAll(Task.Run(async () =>
		{
			var recent = await treasuryDataProvider.GetAuctionsAsync().ConfigureAwait(false);
			if (recent is not null)
				MaybeDispatchAsync(curr, () =>
				{
					foreach (var treasury in recent)
						TreasuriesRecent.Add(treasury);
				});
		}), Task.Run(async () =>
		{
			var upcoming = await treasuryDataProvider.GetUpcomingAsync().ConfigureAwait(false);
			if (upcoming is not null)
				MaybeDispatchAsync(curr, () =>
				{
					foreach (var treasury in upcoming)
						TreasuriesUpcoming.Add(treasury);
				});
		})).ConfigureAwait(false);
	}

	private static void MaybeDispatchAsync(Dispatcher dispatcher, Action action)
	{
		if (dispatcher.CheckAccess())
			action.Invoke();
		else
			dispatcher.InvokeAsync(action);
	}
}
