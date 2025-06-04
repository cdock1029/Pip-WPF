using System.Collections.ObjectModel;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Core;
using Pip.UI.Components.Shared;
using Pip.UI.Data.Services;
using Pip.UI.Models;

namespace Pip.UI.Components.Historical;

[GenerateViewModel]
public partial class HistoricalViewModel(ITreasuryDataProvider treasuryDataProvider) : PipViewModel, IPipRoute
{
    [GenerateProperty] private Year? _selectedYear;

    [GenerateProperty] private ObservableCollection<Treasury>? _treasuries = [];

    [GenerateProperty] private ObservableCollection<Year> _years = [];
    public string View => nameof(HistoricalView);
    public string Title => "Historical Data";
    public Uri? Image { get; } = DXImageHelper.GetImageUri("SvgImages/Business Objects/BO_Audit_ChangeHistory.svg");

    public override async Task LoadAsync()
    {
        if (Years.Any()) return;

        await Dispatcher.InvokeAsync(() =>
        {
            foreach (int i in GenerateYears()) Years.Add(new Year(i));
        });
    }

    [GenerateCommand]
    private async Task HandleYearChanged()
    {
        if (SelectedYear == null) return;

        Treasuries = null;

        IEnumerable<Treasury>? treasuries =
            await treasuryDataProvider.AnnouncementsResultsSearch(SelectedYear.Range.start, SelectedYear.Range.end);

        Treasuries = [];
        if (treasuries is null) return;
            foreach (Treasury treasury in treasuries)
                Treasuries.Add(treasury);
    }

    [GenerateCommand]
    private void HandleCustomColumnSort(RowSortArgs args)
    {
        if (args.FieldName != nameof(Treasury.SecurityTerm)) return;

        Treasury t1 = (Treasury)args.FirstItem;
        Treasury t2 = (Treasury)args.SecondItem;

        if (t1.MaturityDate is not { } m1 ||
            t2.MaturityDate is not { } m2 || t1.IssueDate is not { } i1 || t2.IssueDate is not { } i2)
        {
            args.Result = 0;
            return;
        }

        int span1 = m1.DayNumber - i1.DayNumber;
        int span2 = m2.DayNumber - i2.DayNumber;

        args.Result = span1 - span2;
    }

    private static IEnumerable<int> GenerateYears()
    {
        int currYear = DateTime.Now.Year;
        for (int i = currYear; i >= 1997; i--) yield return i;
    }

    public class Year(int value)
    {
        public int Value => value;
        public (DateOnly start, DateOnly end) Range { get; } = (new DateOnly(value, 1, 1), new DateOnly(value, 12, 31));
        public string Text { get; } = value.ToString();
    }
}