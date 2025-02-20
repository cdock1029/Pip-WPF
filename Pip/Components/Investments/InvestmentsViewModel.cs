using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Core;
using Pip.DataAccess.Services;
using Pip.UI.Components.Details;
using Pip.UI.Messages;
using Pip.UI.ViewModel;

namespace Pip.UI.Components.Investments;

[GenerateViewModel]
public partial class InvestmentsViewModel : PipViewModel, IPipPage
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

	public string View => "InvestmentsView";

	public string Title => "Investments";

	//public Uri Image { get; } = DXImageHelper.GetImageUri("SvgImages/Spreadsheet/Financial.svg");
	public Uri Image { get; } = DXImageHelper.GetImageUri("Images/Spreadsheet/FunctionsFinancial_32x32.png");

	public override async Task LoadAsync()
	{
		await Task.Run(LoadDataAsync).ConfigureAwait(false);
	}

	private void Receive(AfterInsertInvestmentMessage message)
	{
		Dispatcher.BeginInvoke(async () =>
		{
			await Task.Delay(1000);
			Investments.Clear();
			await LoadDataAsync();
			SelectedInvestment = Investments.FirstOrDefault(i => i.Id == message.Value.Id);
		});
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

		var inv = investmentItem.SyncToInvestment();
		if (args.IsNewItem)
			_treasuryDataProvider.Add(inv);

		_treasuryDataProvider.Save();
	}

	[GenerateCommand]
	private void ValidateRowDeletion(ValidateRowDeletionArgs args)
	{
		try
		{
			var investmentItem = (InvestmentItemViewModel)args.Items.Single();
			_treasuryDataProvider.Delete(investmentItem.SyncToInvestment());
		}
		catch (Exception e)
		{
			args.ResultAsync = Task.FromResult(new ValidationErrorInfo($"Error Deleting:\n{e.Message}"));
		}
	}

	private void LoadData()
	{
		if (Investments.Any()) return;

		foreach (var investment in _treasuryDataProvider.GetInvestments())
			Investments.Add(new InvestmentItemViewModel(investment));
	}

	private async Task LoadDataAsync()
	{
		if (Investments.Any()) return;
		var investments = _treasuryDataProvider.GetInvestments();
		await Dispatcher.InvokeAsync(() =>
		{
			foreach (var investment in investments)
				Investments.Add(new InvestmentItemViewModel(investment));
		});
	}
}