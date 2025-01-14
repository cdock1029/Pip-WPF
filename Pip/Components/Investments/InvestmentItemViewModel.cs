using System.ComponentModel.DataAnnotations;
using DevExpress.Mvvm.CodeGenerators;
using JetBrains.Annotations;
using Pip.Model;
using Pip.UI.ViewModel;

namespace Pip.UI.Components.Investments;

[GenerateViewModel]
public partial class InvestmentItemViewModel : PipViewModel
{
	[Display(GroupName = "[Investment parameters]")] [GenerateProperty]
	private DateOnly? _auctionDate;

	[Display(GroupName = "[Investment parameters]")]
	[StringLength(5, ErrorMessage = "Annotation: must be of length 5")]
	[GenerateProperty]
	private string? _confirmation;

	[Display(GroupName = "[Investment parameters]/[Treasury]")]
	[StringLength(9, ErrorMessage = "Annotation: Must have length 9")]
	[Required]
	[GenerateProperty]
	private string? _cusip;

	[GenerateProperty] private int _id;

	[Display(GroupName = "[Investment parameters]/[Treasury]")] [GenerateProperty] [Required]
	private DateOnly? _issueDate;

	[Display(GroupName = "[Investment parameters]/[Treasury]")] [UsedImplicitly] [GenerateProperty]
	private DateOnly? _maturityDate;

	[Display(GroupName = "[Investment parameters]")]
	[DataType(DataType.Currency)]
	[GenerateProperty]
	[Range(100, int.MaxValue)]
	private int _par;

	[Display(GroupName = "[Investment parameters]")]
	[Range(0, 5, ErrorMessage = "Maximum re-investments is 5")]
	[GenerateProperty]
	private int _reinvestments;

	[Display(GroupName = "[Investment parameters]/[Treasury]")] [GenerateProperty]
	private string? _term;


	[Display(GroupName = "[Investment parameters]/[Treasury]")] [GenerateProperty] [Required]
	private TreasuryType? _type;

	public int TermSpan => MaturityDate is null || IssueDate is null
		? 0
		: MaturityDate.Value.DayNumber - IssueDate.Value.DayNumber;


	public Investment AsInvestment()
	{
		return new Investment
		{
			Id = Id,
			Cusip = Cusip!,
			IssueDate = IssueDate!.Value,
			Type = Type!.Value,
			Par = Par,
			MaturityDate = MaturityDate,
			AuctionDate = AuctionDate,
			Confirmation = Confirmation,
			Reinvestments = Reinvestments,
			SecurityTerm = Term
		};
	}

	public static InvestmentItemViewModel FromModel(Investment model)
	{
		return new InvestmentItemViewModel
		{
			Id = model.Id,
			Cusip = model.Cusip,
			IssueDate = model.IssueDate,
			Par = model.Par,
			MaturityDate = model.MaturityDate,
			AuctionDate = model.AuctionDate,
			Confirmation = model.Confirmation,
			Reinvestments = model.Reinvestments,
			Term = model.SecurityTerm,
			Type = model.Type
		};
	}
}