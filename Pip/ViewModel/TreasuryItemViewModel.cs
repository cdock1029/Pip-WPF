using Pip.Model;

namespace Pip.UI.ViewModel;

public class TreasuryItemViewModel(Treasury treasury) : ViewModelBase
{
    public string? Cusip
    {
        get => treasury.Cusip;
        set { SetProperty(treasury.Cusip, value, treasury, (t, c) => t.Cusip = c); }
    }

    public DateOnly IssueDate
    {
        get => treasury.IssueDate;
        set { SetProperty(treasury.IssueDate, value, treasury, (t, i) => t.IssueDate = i); }
    }

    public DateOnly? MaturityDate
    {
        get => treasury.MaturityDate;
        set { SetProperty(treasury.MaturityDate, value, treasury, (t, m) => t.MaturityDate = m); }
    }

    public string SecurityType => treasury.SecurityType;
    public string SecurityTerm => treasury.SecurityTerm;

    public string Type => treasury.Type;
}
