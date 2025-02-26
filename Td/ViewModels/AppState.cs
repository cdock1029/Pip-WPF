using Td.Layout;
using Td.Pages;

namespace Td.ViewModels;

public class AppState
{
    public MainLayout? MainLayoutComponent { get; set; }

    public SearchComponent? SearchComponent { get; set; }

    public List<PreviousAuctionsPage.Year>? Years { get; set; }
}