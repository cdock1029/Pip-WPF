namespace Pip.Web.Services;

public class AppState
{
    //public MainLayout? MainLayoutComponent { get; set; }

    //public SearchComponent? SearchComponent { get; set; }

    private static List<int>? _years;

    public static List<int> GetYears()
    {
        _years ??= GenerateYears();

        return _years;

        static List<int> GenerateYears()
        {
            int currYear = DateTime.Now.Year;
            List<int> yrs = [];
            for (int i = currYear; i >= 1997; i--) yrs.Add(i);

            return yrs;
        }
    }
}