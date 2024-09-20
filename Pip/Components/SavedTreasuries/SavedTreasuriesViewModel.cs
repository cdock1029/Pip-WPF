using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Grid;
using Pip.Model;
using Pip.UI.Messages;
using Pip.UI.Services;
using Pip.UI.ViewModel;

namespace Pip.UI.Components.SavedTreasuries;

public partial class SavedTreasuriesViewModel(
	ITreasuryDataProvider treasuryDataProvider,
	INavigationService navigationService)
	: ViewModelBase, IRecipient<AfterTreasuryInsertMessage>, IRecipient<AfterInsertInvestmentMessage>
{
	[ObservableProperty] private Treasury? _selectedTreasury;

	public ObservableCollection<Treasury> Treasuries { get; } = [];

	public void Receive(AfterInsertInvestmentMessage message)
	{
		Treasuries.Clear();
		Task.Run(LoadAsync);
	}


	public void Receive(AfterTreasuryInsertMessage message)
	{
		Task.Run(() => HandleMessage(message));
	}

	[RelayCommand]
	public override async Task LoadAsync()
	{
		if (Treasuries.Any()) return;
		var treasuries = await treasuryDataProvider.GetSavedAsync();
		foreach (var treasury in treasuries) Treasuries.Add(treasury);
	}

	[RelayCommand]
	private async Task DataSourceRefresh(DataSourceRefreshArgs args)
	{
		Treasuries.Clear();
		await LoadAsync();
	}

	[RelayCommand]
	private async Task ValidateRowDeletion(GridValidateRowDeletionEventArgs args)
	{
		try
		{
			var rows = Array.ConvertAll(args.Rows, o => (Treasury)o);
			await treasuryDataProvider.DeleteTreasuriesAsync(rows);
		}
		catch (Exception e)
		{
			args.ResultAsync = Task.FromResult(new ValidationErrorInfo($"Error Deleting:\n{e.Message}"));
		}
	}

	private async Task HandleMessage(AfterTreasuryInsertMessage message)
	{
		Treasuries.Clear();
		await LoadAsync();
		var ust = message.Value;
		// Todo: unnecessary task.run?
		var found = await Task.Run(() =>
			Treasuries.FirstOrDefault(t => t.Cusip == ust.Cusip && t.IssueDate == ust.IssueDate));
		if (found is not null) SelectedTreasury = found;
		await navigationService.NavigateToAsync<SavedTreasuriesViewModel>();
	}
}
