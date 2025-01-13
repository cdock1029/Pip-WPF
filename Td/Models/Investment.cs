using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Td.Models;

[Index(nameof(Cusip), nameof(IssueDate))]
public class Investment
{
	public int Id { get; set; }

	[Required]
	[MaxLength(9)]
	[MinLength(9)]
	public string? Cusip { get; set; }

	[Required] public DateOnly? IssueDate { get; set; }

	[Required] [Range(100, int.MaxValue)] public int? Par { get; set; }

	public DateOnly? MaturityDate { get; set; }

	public DateOnly? AuctionDate { get; set; }

	[MaxLength(5)] [MinLength(5)] public string? Confirmation { get; set; }

	public int? Reinvestments { get; set; }

	[MaxLength(20)] public string? SecurityTerm { get; set; }

	public TreasuryType? Type { get; set; }
}