using System.ComponentModel.DataAnnotations;
using DevExpress.Mvvm.CodeGenerators;
using JetBrains.Annotations;
using Pip.Model;
using Pip.UI.ViewModel;

namespace Pip.UI.Components.Investments;

[GenerateViewModel]
public partial class InvestmentItemViewModel(Investment investment) : PipViewModel
{
	public InvestmentItemViewModel() : this(new Investment())
	{
	}

	public int Id
	{
		get => investment.Id;
		set
		{
			investment.Id = value;
			RaisePropertiesChanged();
		}
	}

	[Display(GroupName = "[Investment parameters]/[Treasury]")]
	[StringLength(9, ErrorMessage = "Annotation: Must have length 9")]
	public string? Cusip
	{
		get => investment.Cusip;
		set
		{
			investment.Cusip = value;
			RaisePropertiesChanged();
		}
	}

	[Display(GroupName = "[Investment parameters]")]
	[StringLength(5, ErrorMessage = "Annotation: must be of length 5")]
	public string? Confirmation
	{
		get => investment.Confirmation;
		set
		{
			investment.Confirmation = value;
			RaisePropertyChanged();
		}
	}

	[Display(GroupName = "[Investment parameters]")]
	public int Reinvestments
	{
		get => investment.Reinvestments;
		set
		{
			investment.Reinvestments = value;
			RaisePropertyChanged();
			ClearErrors();
			if (value > 5) AddError("Max re-investments is 5");
			OnErrorsChanged();
		}
	}

	[Display(GroupName = "[Investment parameters]")]
	[DataType(DataType.Currency)]
	public int Par
	{
		get => investment.Par;
		set
		{
			investment.Par = value;
			RaisePropertyChanged();
		}
	}


	[Display(GroupName = "[Investment parameters]/[Treasury]")]
	public DateOnly? IssueDate
	{
		get => investment.IssueDate;
		set
		{
			investment.IssueDate = value;
			RaisePropertiesChanged();
		}
	}

	[Display(GroupName = "[Investment parameters]/[Treasury]")]
	[UsedImplicitly]
	public DateOnly? MaturityDate
	{
		get => investment.MaturityDate;
		set
		{
			investment.MaturityDate = value;
			RaisePropertiesChanged();
		}
	}

	[Display(GroupName = "[Investment parameters]/[Treasury]")]
	public string? Term
	{
		get => investment.SecurityTerm;
		set
		{
			investment.SecurityTerm = value;
			RaisePropertiesChanged();
		}
	}


	[Display(GroupName = "[Investment parameters]/[Treasury]")]
	public TreasuryType Type
	{
		get => investment.Type;
		set
		{
			investment.Type = value;
			RaisePropertiesChanged();
		}
	}

	public int TermSpan => MaturityDate is null || IssueDate is null
		? 0
		: MaturityDate.Value.DayNumber - IssueDate.Value.DayNumber;


	public Investment AsInvestment()
	{
		return investment;
	}
}