using Pip.Model;

namespace Pip.ViewModel;

public class TreasuryItemViewModel(Treasury treasury) : ViewModelBase
{
    private readonly Treasury _treasury = treasury;

    public string? Cusip
    {
        get => _treasury.Cusip;
        set
        {
            _treasury.Cusip = value;
            OnPropertyChanged();
        }
    }

    public DateOnly IssueDate
    {
        get => _treasury.IssueDate;
        set
        {
            _treasury.IssueDate = value;
            OnPropertyChanged();
        }
    }

    public DateOnly MaturityDate
    {
        get => _treasury.MaturityDate;
        set
        {
            _treasury.MaturityDate = value;
            OnPropertyChanged();
        }
    }
}