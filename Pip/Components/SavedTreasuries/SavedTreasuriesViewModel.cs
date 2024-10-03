using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Grid;
using Pip.Model;
using Pip.UI.Messages;
using Pip.UI.Services;
using Pip.UI.ViewModel;
using System.Collections.ObjectModel;

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
			// Todo: Make this method handle one or many correctly.
			var deletedTreasury = rows[0];
			WeakReferenceMessenger.Default.Send(
				new AfterTreasuryDeleteMessage(new AfterTreasuryDeleteArgs(deletedTreasury.Cusip,
					deletedTreasury.IssueDate)));
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
		SelectedTreasury = Treasuries
			.FirstOrDefault(t => t.Cusip == ust.Cusip && t.IssueDate == ust.IssueDate);
		await navigationService.NavigateToAsync<SavedTreasuriesViewModel>();
	}
}
