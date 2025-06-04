using System.Windows.Input;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI.Navigation;
using Treasury = Pip.UI.Models.Treasury;

namespace Pip.UI.Components.Auctions;

public partial class AuctionsView : INavigationAware
{
	public AuctionsView()
	{
		InitializeComponent();
	}

	public void NavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is AuctionsViewModel vm) DataContext ??= vm;
    }

    public void NavigatingFrom(NavigatingEventArgs e)
    {
    }

    public void NavigatedFrom(NavigationEventArgs e)
    {
    }

    private void CopyCommandBindingRecent_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        GridControl grid = (GridControl)sender;
        if (grid.SelectedItem is Treasury t) Clipboard.SetText(t.Cusip);
    }

    private void CopyCommandBindingUpcoming_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        GridControl grid = (GridControl)sender;
        if (grid.SelectedItem is Treasury t) Clipboard.SetText(t.Cusip);
    }
}