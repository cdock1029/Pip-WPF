using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Pip.Model;

[Index(nameof(Cusip), nameof(IssueDate))]
public class Investment
{
	public int Id { get; set; }

	public required string Cusip { get; set; }

	public required DateOnly IssueDate { get; set; }

	public required TreasuryType Type { get; set; }

	public int Par { get; set; }

	public DateOnly? MaturityDate { get; set; }

	public DateOnly? AuctionDate { get; set; }

	public string? Confirmation { get; set; }

	public int Reinvestments { get; set; }

	[MaxLength(20)] public string? SecurityTerm { get; set; }
}