using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Pip.Model;
using Pip.UI.Data;
using Pip.UI.Messages;
using Pip.UI.View.Services;

namespace Pip.UI.ViewModel;

public partial class SavedTreasuriesViewModel(
	ITreasuryDataProvider treasuryDataProvider,
	INavigationService navigationService)
	: ViewModelBase, IRecipient<AfterTreasuryInsertMessage>,
		ITreasuriesViewModel
{
	[ObservableProperty] private Treasury? _selectedTreasury;

	public async void Receive(AfterTreasuryInsertMessage message)
	{
		await HandleMessage(message);
	}

	public ObservableCollection<Treasury> Treasuries { get; } = [];

	[RelayCommand]
	public override async Task LoadAsync()
	{
		if (Treasuries.Any()) return;
		var treasuries = await treasuryDataProvider.GetSavedAsync();
		foreach (var treasury in treasuries) Treasuries.Add(treasury);
	}

	private async Task HandleMessage(AfterTreasuryInsertMessage message)
	{
		Treasuries.Clear();
		await LoadAsync();
		var ust = message.Value;
		var found = await Task.Run(() =>
			Treasuries.FirstOrDefault(t => t.Cusip == ust.Cusip && t.IssueDate == ust.IssueDate));
		if (found is not null) SelectedTreasury = found;
		await navigationService.NavigateToAsync<SavedTreasuriesViewModel>();
	}
}
