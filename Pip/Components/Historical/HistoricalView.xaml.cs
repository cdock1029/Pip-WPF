using DevExpress.Xpf.WindowsUI.Navigation;
using UserControl = System.Windows.Controls.UserControl;

namespace Pip.UI.Components.Historical;

public partial class HistoricalView : UserControl, INavigationAware
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