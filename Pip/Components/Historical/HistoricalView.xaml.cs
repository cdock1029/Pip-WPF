using DevExpress.Xpf.WindowsUI.Navigation;

namespace Pip.UI.Components.Historical;

public partial class HistoricalView : INavigationAware
{
    public HistoricalView()
    {
        InitializeComponent();
    }

    public void NavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is HistoricalViewModel vm) DataContext ??= vm;
    }

    public void NavigatingFrom(NavigatingEventArgs e)
    {
    }

    public void NavigatedFrom(NavigationEventArgs e)
    {
    }
}