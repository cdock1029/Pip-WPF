using Td.Components.Layout;
using Td.Components.Pages;

namespace Td.ViewModels;

public class AppState
{
    public MainLayout? MainLayoutComponent { get; set; }

    public SearchComponent? SearchComponent { get; set; }

    public List<PreviousAuctionsPage.Year>? Years { get; set; }

    public HashSet<string> RenderedPages { get; set; } = [];
}