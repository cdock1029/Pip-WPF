using System.ComponentModel.DataAnnotations;

namespace Pip.Model;

public class Investment
{
	public int Id { get; init; }

	[Required] public required string Cusip { get; init; }

	[Required] public required DateOnly IssueDate { get; init; }

	public int Par { get; set; }

	public string? Confirmation { get; init; }

	public int Reinvestments { get; init; }
	public DateOnly? MaturityDate { get; set; }
	public string? SecurityTerm { get; set; }
	public TreasuryType Type { get; set; }
}
