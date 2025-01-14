using System.Collections.ObjectModel;
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
	private readonly ITreasuryDataProvider _treasuryDataProvider;
	[GenerateProperty] private bool _isWaitIndicatorVisible;
	[GenerateProperty] private InvestmentItemViewModel? _selectedInvestment;

	public InvestmentsViewModel(ITreasuryDataProvider treasuryDataProvider, DetailsViewModel detailsViewModel)
	{
		_treasuryDataProvider = treasuryDataProvider;
		DetailsViewModel = detailsViewModel;
		Messenger.Default.Register<AfterInsertInvestmentMessage>(this, Receive);
	}

	public ObservableCollection<InvestmentItemViewModel> Investments { get; } = [];

	public DetailsViewModel DetailsViewModel { get; }

	public override void Load()
	{
		LoadData();
	}

	private void Receive(AfterInsertInvestmentMessage message)
	{
		Investments.Clear();
		LoadData();
		var insertedId = message.Value.Id;
		var found = Investments.FirstOrDefault(i => i.Id == insertedId);
		SelectedInvestment = found;
	}

	[GenerateCommand]
	private void DataSourceRefresh(DataSourceRefreshArgs args)
	{
		Investments.Clear();
		LoadData();
	}

	[GenerateCommand]
	private void ValidateRow(RowValidationArgs args)
	{
		var investmentItem = (InvestmentItemViewModel)args.Item;
		if (investmentItem.HasErrors) return;

		if (args.IsNewItem)
			_treasuryDataProvider.Add(investmentItem.AsInvestment());
		else
			_treasuryDataProvider.Update(investmentItem.AsInvestment());

		_treasuryDataProvider.Save();
	}

	[GenerateCommand]
	private void ValidateRowDeletion(ValidateRowDeletionArgs args)
	{
		try
		{
			var investmentItem = (InvestmentItemViewModel)args.Items.Single();
			_treasuryDataProvider.Delete(investmentItem.AsInvestment());
		}
		catch (Exception e)
		{
			args.ResultAsync = Task.FromResult(new ValidationErrorInfo($"Error Deleting:\n{e.Message}"));
		}
	}

	private void LoadData()
	{
		if (Investments.Any()) return;

		var investments = _treasuryDataProvider.GetInvestments();
		foreach (var investment in investments)
			Investments.Add(InvestmentItemViewModel.FromModel(investment));
	}
}