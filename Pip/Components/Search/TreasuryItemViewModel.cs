using JetBrains.Annotations;
using Pip.Model;

namespace Pip.UI.Components.Search;

[PublicAPI]
public class TreasuryItemViewModel(Treasury treasury)
{
    public string Cusip { get; set; } = treasury.Cusip;

    public DateOnly? IssueDate { get; set; } = treasury.IssueDate;

    public TreasuryType? Type { get; set; } = treasury.Type;

    public string? SecurityTerm { get; set; } = treasury.SecurityTerm;

    public Treasury Treasury { get; set; } = treasury;

    public DateOnly? MaturityDate { get; set; } = treasury.MaturityDate;

    public int TermSpan => MaturityDate is null || IssueDate is null
        ? 0
        : MaturityDate.Value.DayNumber - IssueDate.Value.DayNumber;

    public override string ToString()
    {
        return $"{Type}|{SecurityTerm}|Issue: {IssueDate:dd MMM yyyy}";
    }
}