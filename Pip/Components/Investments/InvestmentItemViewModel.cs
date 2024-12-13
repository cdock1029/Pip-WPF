using Pip.Model;
using Pip.UI.ViewModel;

namespace Pip.UI.Components.Investments;

public class InvestmentItemViewModel(Investment investment) : PipViewModel
{
	public int Id { get; } = investment.Id;
	public string Cusip { get; } = investment.Cusip;
	public DateOnly IssueDate { get; } = investment.IssueDate;
	public DateOnly? MaturityDate { get; } = investment.MaturityDate;


	public string? Confirmation
	{
		get => investment.Confirmation;
		set
		{
			investment.Confirmation = value;
			RaisePropertyChanged();
		}
	}

	public int Reinvestments
	{
		get => investment.Reinvestments;
		set
		{
			investment.Reinvestments = value;
			RaisePropertyChanged();
		}
	}

	public string? Term { get; } = investment.SecurityTerm;
	public TreasuryType Type { get; } = investment.Type;

	public Investment Investment => investment;

	public int TermSpan => MaturityDate is null ? 0 : MaturityDate.Value.DayNumber - IssueDate.DayNumber;

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