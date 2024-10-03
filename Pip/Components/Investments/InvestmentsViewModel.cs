using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Grid;
using Pip.UI.Messages;
using Pip.UI.Services;
using Pip.UI.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace Pip.UI.Components.Investments;

public partial class InvestmentsViewModel(
	ITreasuryDataProvider treasuryDataProvider,
	INavigationService navigation,
	Dispatcher dispatcher)
	: ViewModelBase, IRecipient<AfterInsertInvestmentMessage>, IRecipient<AfterTreasuryDeleteMessage>
{
	[ObservableProperty] private InvestmentItemViewModel? _selectedInvestment;

	public ObservableCollection<InvestmentItemViewModel> Investments { get; } = [];

	public void Receive(AfterInsertInvestmentMessage message)
	{
		Task.Run(() => HandleMessage(message));
	}

	public void Receive(AfterTreasuryDeleteMessage message)
	{
		Investments.Clear();
		Task.Run(LoadAsync);
	}

	[RelayCommand]
	public override async Task LoadAsync()
	{
		if (Investments.Any()) return;
		var investments = await treasuryDataProvider.GetInvestmentsAsync();
		foreach (var investment in investments) Investments.Add(new InvestmentItemViewModel(investment));
	}

	[RelayCommand]
	private async Task DataSourceRefresh(DataSourceRefreshArgs args)
	{
		Investments.Clear();
		await LoadAsync();
	}

	[RelayCommand]
	private async Task ValidateRow(GridRowValidationEventArgs args)
	{
		var investmentItem = (InvestmentItemViewModel)args.Row;
		if (args.IsNewItem)
			treasuryDataProvider.Add(investmentItem.Investment);
		await treasuryDataProvider.SaveAsync();
	}

	[RelayCommand]
	private async Task ValidateRowDeletion(GridValidateRowDeletionEventArgs args)
	{
		try
		{
			var rows = Array.ConvertAll(args.Rows, o => ((InvestmentItemViewModel)o).Investment);
			await treasuryDataProvider.DeleteInvestmentsAsync(rows);
		}
		catch (Exception e)
		{
			args.ResultAsync = Task.FromResult(new ValidationErrorInfo($"Error Deleting:\n{e.Message}"));
		}
	}

	private async Task HandleMessage(AfterInsertInvestmentMessage message)
	{
		Investments.Clear();
		await LoadAsync();
		var insertedId = message.Value.Id;
		var found = Investments.FirstOrDefault(i => i.Id == insertedId);
		await dispatcher.BeginInvoke(() => SelectedInvestment = found);
		await navigation.NavigateToAsync<InvestmentsViewModel>();
	}
}
