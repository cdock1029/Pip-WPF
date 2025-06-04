using Treasury = Pip.UI.Models.Treasury;

namespace Pip.UI.Utils;

public static class Extensions
{
    public static int GetTermSpan(this Treasury t)
    {
        return t.MaturityDate is null || t.IssueDate is null
            ? 0
            : t.MaturityDate.Value.DayNumber - t.IssueDate.Value.DayNumber;
    }
}
