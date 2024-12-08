using System.Collections.ObjectModel;
using System.Windows.Threading;
using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Mvvm.Xpf;
using Pip.DataAccess.Services;
using Pip.UI.Components.Details;
using Pip.UI.Messages;
using Pip.UI.ViewModel;

namespace Pip.UI.Components.Investments;

[GenerateViewModel]
public partial class InvestmentsViewModel : PipViewModel
{
	private readonly Dispatcher _dispatcher;
	private readonly ITreasuryDataProvider _treasuryDataProvider;
	[GenerateProperty] private bool _isWaitIndicatorVisible;
	[GenerateProperty] private InvestmentItemViewModel? _selectedInvestment;

	public InvestmentsViewModel(ITreasuryDataProvider treasuryDataProvider,
		Dispatcher dispatcher, DetailsViewModel detailsViewModel)
	{
		_treasuryDataProvider = treasuryDataProvider;
		_dispatcher = dispatcher;
		DetailsViewModel = detailsViewModel;
		Messenger.Default.Register<AfterInsertInvestmentMessage>(this, Receive);
	}

	public ObservableCollection<InvestmentItemViewModel> Investments { get; } = [];

	public DetailsViewModel DetailsViewModel { get; }

	public override Task LoadAsync()
	{
		if (Investments.Any()) return Task.CompletedTask;

		var investments = _treasuryDataProvider.GetInvestments();
		foreach (var investment in investments)
			Investments.Add(new InvestmentItemViewModel(investment));

		return Task.CompletedTask;
	}

	private void Receive(AfterInsertInvestmentMessage message)
	{
		_dispatcher.BeginInvoke(async () =>
		{
			Investments.Clear();
			await LoadAsync();
			var insertedId = message.Value.Id;
			var found = Investments.FirstOrDefault(i => i.Id == insertedId);
			SelectedInvestment = found;
		});
	}

	[GenerateCommand]
	private async Task DataSourceRefresh(DataSourceRefreshArgs args)
	{
		Investments.Clear();
		await LoadAsync();
	}

	[GenerateCommand]
	private async Task ValidateRow(RowValidationArgs args)
	{
		if (args.IsNewItem)
		{
			var investmentItem = (InvestmentItemViewModel)args.Item;
			_treasuryDataProvider.Add(investmentItem.Investment);
		}

		await _treasuryDataProvider.SaveAsync();
	}

	[GenerateCommand]
	private async Task ValidateRowDeletion(ValidateRowDeletionArgs args)
	{
		try
		{
			var investmentItem = (InvestmentItemViewModel)args.Items.Single();
			await _treasuryDataProvider.DeleteInvestmentAsync(investmentItem.Investment);
		}
		catch (Exception e)
		{
			args.ResultAsync = Task.FromResult(new ValidationErrorInfo($"Error Deleting:\n{e.Message}"));
		}
	}
}