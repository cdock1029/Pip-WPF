namespace Pip.Model;

public class Investment
{
	public int Id { get; init; }

	public Treasury Treasury { get; set; } = null!;
	public required string TreasuryCusip { get; init; }
	public required DateOnly TreasuryIssueDate { get; init; }

	public int Par { get; set; }

	public string? Confirmation { get; init; }

	public int Reinvestments { get; init; }
}
