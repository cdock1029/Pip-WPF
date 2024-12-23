using Pip.Model;
using Pip.UI.ViewModel;

namespace Pip.UI.Components.Investments;

public class InvestmentItemViewModel : PipViewModel
{
	public int Id => Investment.Id;
	public string Cusip => Investment.Cusip;
	public DateOnly IssueDate => Investment.IssueDate;
	public DateOnly? MaturityDate => Investment.MaturityDate;

	public required Investment Investment { get; set; }

	public string? Confirmation
	{
		get => Investment.Confirmation;
		set
		{
			Investment.Confirmation = value;
			RaisePropertyChanged();
		}
	}

	public int Reinvestments
	{
		get => Investment.Reinvestments;
		set
		{
			Investment.Reinvestments = value;
			RaisePropertyChanged();
		}
	}

	public string? Term => Investment.SecurityTerm;
	public TreasuryType Type => Investment.Type;


	public int TermSpan => MaturityDate is null ? 0 : MaturityDate.Value.DayNumber - IssueDate.DayNumber;

	public int Par
	{
		get => Investment.Par;
		set
		{
			Investment.Par = value;
			RaisePropertyChanged();
		}
	}
}