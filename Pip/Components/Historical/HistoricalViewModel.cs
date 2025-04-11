using System.Collections.ObjectModel;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Xpf.Core;
using Pip.DataAccess.Services;
using Pip.Model;
using Pip.UI.Components.Shared;

namespace Pip.UI.Components.Historical;

[GenerateViewModel]
// ReSharper disable once PartialTypeWithSinglePart
public partial class HistoricalViewModel(ITreasuryDataProvider treasuryDataProvider) : PipViewModel, IPipRoute
{
    public string View => nameof(HistoricalView);
    public string Title => "Historical Data";
    public Uri? Image { get; } = DXImageHelper.GetImageUri("SvgImages/Business Objects/BO_Audit_ChangeHistory.svg");

    [GenerateProperty] private Year? _selectedYear;

    [GenerateProperty] private ObservableCollection<Year> _years = [];

    [GenerateProperty] private ObservableCollection<Treasury>? _treasuries;

    [GenerateProperty] private bool _isLoading;

    public override void Load()
    {
        if (Years.Any()) return;

        foreach (int i in GenerateYears()) Years.Add(new Year(i));
    }

    [GenerateCommand]
    public async Task HandleYearChanged()
    {
        if (SelectedYear is not null)
        {
            IsLoading = true;
            Treasuries = [];

            IEnumerable<Treasury>? treasuries =
                await treasuryDataProvider.AnnouncementsResultsSearch(SelectedYear.Range.start, SelectedYear.Range.end);

            await Dispatcher.InvokeAsync(() =>
            {
                IsLoading = false;
                if (treasuries is null) return;
                foreach (Treasury treasury in treasuries)
                    Treasuries.Add(treasury);
            });
        }
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