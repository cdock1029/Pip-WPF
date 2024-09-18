using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Grid;
using Pip.UI.Data;

namespace Pip.UI.ViewModel;

internal partial class InvestmentsViewModel(ITreasuryDataProvider treasuryDataProvider) : ViewModelBase
{
	[ObservableProperty] private InvestmentItemViewModel? _selectedInvestment;

	public ObservableCollection<InvestmentItemViewModel> Investments { get; } = [];

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
		_ = await treasuryDataProvider.SaveAsync();
	}
}
