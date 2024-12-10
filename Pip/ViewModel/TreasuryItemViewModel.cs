using Pip.Model;

namespace Pip.UI.ViewModel;

public class TreasuryItemViewModel
{
	public required string Cusip { get; set; }

	public DateOnly? IssueDate { get; set; }

	public string Id => $"{Cusip}_{IssueDate:yy-MM-dd}";

	public TreasuryType? Type { get; set; }

	public string? Term { get; set; }

	public override string ToString()
	{
		return $"Issue: {IssueDate:dd MMM yyyy} Type: {Type} Term: {Term}";
	}
}