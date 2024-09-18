namespace Pip.Model;

public class Investment
{
	public int Id { get; init; }

	public Treasury Treasury { get; init; } = null!;
	public required string TreasuryCusip { get; init; }
	public required DateOnly TreasuryIssueDate { get; init; }

	public required int Par { get; set; }

	public required string Confirmation { get; init; }

	public int Reinvestments { get; init; }
}
