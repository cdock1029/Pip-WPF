using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Grid;
using Pip.Model;
using Pip.UI.Messages;
using Pip.UI.Services;
using Pip.UI.ViewModel;
using System.Collections.ObjectModel;
using INavigationService = Pip.UI.Services.INavigationService;

namespace Pip.UI.Components.SavedTreasuries;

[GenerateViewModel]
public partial class SavedTreasuriesViewModel : PipViewModel
{
	private readonly INavigationService _navigationService;
	private readonly ITreasuryDataProvider _treasuryDataProvider;
	[GenerateProperty] private bool _isWaitIndicatorVisible;
	[GenerateProperty] private Treasury? _selectedTreasury;

	public SavedTreasuriesViewModel(ITreasuryDataProvider treasuryDataProvider,
		INavigationService navigationService)
	{
		_treasuryDataProvider = treasuryDataProvider;
		_navigationService = navigationService;
		Messenger.Default.Register<AfterInsertInvestmentMessage>(this, Receive);
		Messenger.Default.Register<AfterTreasuryInsertMessage>(this, Receive);
	}

	public ObservableCollection<Treasury> Treasuries { get; } = [];

	public override async Task LoadAsync()
	{
		if (Treasuries.Any()) return;
		IsWaitIndicatorVisible = true;

		var treasuriesTask = _treasuryDataProvider.GetSavedAsync();
		var delay = Task.Delay(1200);
		await Task.WhenAll(treasuriesTask, delay);
		foreach (var treasury in treasuriesTask.Result) Treasuries.Add(treasury);
		IsWaitIndicatorVisible = false;
	}

	private void Receive(AfterInsertInvestmentMessage message)
	{
		Treasuries.Clear();
		Task.Run(LoadAsync);
	}

	private void Receive(AfterTreasuryInsertMessage message)
	{
		Task.Run(() => HandleMessage(message));
	}

	[GenerateCommand]
	private async Task DataSourceRefresh(DataSourceRefreshArgs args)
	{
		Treasuries.Clear();
		await LoadAsync();
	}

	[GenerateCommand]
	private async Task ValidateRowDeletion(GridValidateRowDeletionEventArgs args)
	{
		try
		{
			var rows = Array.ConvertAll(args.Rows, o => (Treasury)o);
			await _treasuryDataProvider.DeleteTreasuriesAsync(rows);
			// Todo: Make this method handle one or many correctly.
			var deletedTreasury = rows[0];
			Messenger.Default.Send(new AfterTreasuryDeleteMessage(new AfterTreasuryDeleteArgs(deletedTreasury.Cusip,
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
		await _navigationService.NavigateToAsync<SavedTreasuriesViewModel>();
	}
}
