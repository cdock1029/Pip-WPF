using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pip.Model;

[Description("A US Treasury investment saved in a porfolio")]
[Index(nameof(Cusip), nameof(IssueDate))]
public class Investment
{
	[Description("Database ID")] public int Id { get; set; }

	[Description("Treasury CUSIP identifier")]
	public required string Cusip { get; set; }

	[Description("Date when it was issued to the purchaser by the treasury department")]
	public required DateOnly IssueDate { get; set; }

	[Description("Which type of treasury: Bill, Note, Bond, CMB, TIPS, or FRN")]
	public required TreasuryType Type { get; set; }

	[Description("The principle or face value of the treasury investment")]
	public int Par { get; set; }

	[Description("Date when the treasury matures")]
	public DateOnly? MaturityDate { get; set; }

	[Description("Date of the auction")] public DateOnly? AuctionDate { get; set; }

	[Description("Confirmation code for the purchase order")]
	public string? Confirmation { get; set; }

	[Description("The count of how many automatic reinvestments or rollovers are scheduled")]
	public int Reinvestments { get; set; }

	[Description("The length of time the security earns interest, from issue date to maturity date")]
	[MaxLength(20)]
	public string? SecurityTerm { get; set; }

	[NotMapped] public int TermSpan => MaturityDate is null ? 0 : MaturityDate.Value.DayNumber - IssueDate.DayNumber;
}

public static class InvestmentMapper
{
	public static Investment Clone(Investment source)
	{
		return new Investment
		{
			Id = source.Id,
			AuctionDate = source.AuctionDate,
			Confirmation = source.Confirmation,
			Cusip = source.Cusip,
			IssueDate = source.IssueDate,
			MaturityDate = source.MaturityDate,
			Par = source.Par,
			Reinvestments = source.Reinvestments,
			SecurityTerm = source.SecurityTerm,
			Type = source.Type
		};
	}

	public static void CopyFrom(this Investment target, Investment source)
	{
		target.Id = source.Id;
		target.AuctionDate = source.AuctionDate;
		target.Confirmation = source.Confirmation;
		target.Cusip = source.Cusip;
		target.IssueDate = source.IssueDate;
		target.MaturityDate = source.MaturityDate;
		target.Par = source.Par;
		target.Reinvestments = source.Reinvestments;
		target.SecurityTerm = source.SecurityTerm;
		target.Type = source.Type;
	}
}