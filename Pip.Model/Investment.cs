using System.ComponentModel.DataAnnotations;

namespace Pip.Model;

public class Investment
{
	public int Id { get; set; }

	[Required] public required string Cusip { get; set; }

	[Required] public required DateOnly IssueDate { get; set; }

	public int Par { get; set; }

	public string? Confirmation { get; set; }

	public int Reinvestments { get; set; }
	public DateOnly? MaturityDate { get; set; }
	public string? SecurityTerm { get; set; }
	public TreasuryType Type { get; set; }
}