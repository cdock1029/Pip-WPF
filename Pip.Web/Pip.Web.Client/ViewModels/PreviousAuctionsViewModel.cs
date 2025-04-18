namespace Pip.Web.Client.ViewModels;

public class PreviousAuctionsViewModel
{
    public List<int>? Years;

    public List<int> GetYears()
    {
        Years = GenerateYears();

        return Years;

        static List<int> GenerateYears()
        {
            int currYear = DateTime.Now.Year;
            List<int> yrs = [];
            for (int i = currYear; i >= 1997; i--) yrs.Add(i);

            return yrs;
        }
    }
}
