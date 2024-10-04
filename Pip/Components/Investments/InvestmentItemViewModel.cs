using DevExpress.Mvvm.CodeGenerators;
using Pip.Model;
using Pip.UI.ViewModel;

namespace Pip.UI.Components.Investments;

[GenerateViewModel]
public partial class InvestmentItemViewModel(Investment investment) : PipViewModel
{
	[GenerateProperty] private string? _confirmation = investment.Confirmation;
	[GenerateProperty] private string _cusip = investment.TreasuryCusip;
	[GenerateProperty] private int _id = investment.Id;

	[GenerateProperty] private DateOnly _issueDate = investment.TreasuryIssueDate;
	[GenerateProperty] private DateOnly? _maturityDate = investment.Treasury.MaturityDate;

	[GenerateProperty] private int _reinvestments = investment.Reinvestments;
	[GenerateProperty] private string _term = investment.Treasury.SecurityTerm;
	[GenerateProperty] private TreasuryType _type = investment.Treasury.Type;

	public Investment Investment => investment;

	public int Par
	{
		get => investment.Par;
		set
		{
			investment.Par = value;
			RaisePropertyChanged();
		}
	}
}
