using CommunityToolkit.Mvvm.ComponentModel;
using Pip.Model;

namespace Pip.UI.ViewModel;

internal partial class InvestmentItemViewModel(Investment investment) : ViewModelBase
{
	[ObservableProperty] private string _confirmation = investment.Confirmation;
	[ObservableProperty] private string _cusip = investment.TreasuryCusip;
	[ObservableProperty] private int _id = investment.Id;

	[ObservableProperty] private DateOnly _issueDate = investment.TreasuryIssueDate;
	[ObservableProperty] private DateOnly? _maturityDate = investment.Treasury.MaturityDate;

	[ObservableProperty] private int _reinvestments = investment.Reinvestments;
	[ObservableProperty] private string _term = investment.Treasury.SecurityTerm;
	[ObservableProperty] private TreasuryType _type = investment.Treasury.Type;

	public Investment Investment => investment;

	public int Par
	{
		get => investment.Par;
		set => SetProperty(investment.Par, value, investment, (i, n) => i.Par = n);
	}
}
