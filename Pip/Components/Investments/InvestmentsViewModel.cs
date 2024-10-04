using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Grid;
using Pip.UI.Messages;
using Pip.UI.Services;
using Pip.UI.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using INavigationService = Pip.UI.Services.INavigationService;

namespace Pip.UI.Components.Investments;

[GenerateViewModel]
public partial class InvestmentsViewModel : PipViewModel
{
	private readonly Dispatcher _dispatcher;
	private readonly INavigationService _navigation;
	private readonly ITreasuryDataProvider _treasuryDataProvider;
	[GenerateProperty] private bool _isWaitIndicatorVisible;
	[GenerateProperty] private InvestmentItemViewModel? _selectedInvestment;

	public InvestmentsViewModel(ITreasuryDataProvider treasuryDataProvider,
		INavigationService navigation,
		Dispatcher dispatcher)
	{
		_treasuryDataProvider = treasuryDataProvider;
		_navigation = navigation;
		_dispatcher = dispatcher;
		Messenger.Default.Register<AfterInsertInvestmentMessage>(this, Receive);
		Messenger.Default.Register<AfterTreasuryDeleteMessage>(this, Receive);
	}

	public ObservableCollection<InvestmentItemViewModel> Investments { get; } = [];

	public override async Task LoadAsync()
	{
		if (Investments.Any()) return;
		IsWaitIndicatorVisible = true;

		var investmentsTask = _treasuryDataProvider.GetInvestmentsAsync();
		var delay = Task.Delay(1200);
		await Task.WhenAll(investmentsTask, delay);
		foreach (var investment in investmentsTask.Result) Investments.Add(new InvestmentItemViewModel(investment));
		IsWaitIndicatorVisible = false;
	}

	private void Receive(AfterInsertInvestmentMessage message)
	{
		Task.Run(() => HandleMessage(message));
	}

	private void Receive(AfterTreasuryDeleteMessage message)
	{
		Investments.Clear();
		Task.Run(LoadAsync);
	}

	[GenerateCommand]
	private async Task DataSourceRefresh(DataSourceRefreshArgs args)
	{
		Investments.Clear();
		await LoadAsync();
	}

	[GenerateCommand]
	private async Task ValidateRow(GridRowValidationEventArgs args)
	{
		var investmentItem = (InvestmentItemViewModel)args.Row;
		if (args.IsNewItem)
			_treasuryDataProvider.Add(investmentItem.Investment);
		await _treasuryDataProvider.SaveAsync();
	}

	[GenerateCommand]
	private async Task ValidateRowDeletion(GridValidateRowDeletionEventArgs args)
	{
		try
		{
			var rows = Array.ConvertAll(args.Rows, o => ((InvestmentItemViewModel)o).Investment);
			await _treasuryDataProvider.DeleteInvestmentsAsync(rows);
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
		await _dispatcher.BeginInvoke(() => SelectedInvestment = found);
		await _navigation.NavigateToAsync<InvestmentsViewModel>();
	}
}
