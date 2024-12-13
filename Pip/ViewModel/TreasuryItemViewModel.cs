using Pip.Model;

namespace Pip.UI.ViewModel;

public class TreasuryItemViewModel(Treasury treasury)
{
	public string Cusip { get; set; } = treasury.Cusip;

	public DateOnly? IssueDate { get; set; } = treasury.IssueDate;

	public TreasuryType? Type { get; set; } = treasury.Type;

	public string? Term { get; set; } = treasury.SecurityTerm;

	public Treasury Treasury { get; set; } = treasury;

	public override string ToString()
	{
		return $"Issue: {IssueDate:dd MMM yyyy} Type: {Type} Term: {Term}";
	}
}