using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Pip.Model;

[Index(nameof(Cusip), nameof(IssueDate))]
public class Investment
{
	public int Id { get; set; }

	[Required]
	[MaxLength(9)]
	[MinLength(9)]
	public string? Cusip { get; set; }

	[Required] public DateOnly? IssueDate { get; set; }

	public int Par { get; set; }

	public string? Confirmation { get; set; }

	public int Reinvestments { get; set; }
	public DateOnly? MaturityDate { get; set; }
	public string? SecurityTerm { get; set; }
	public TreasuryType Type { get; set; }
}