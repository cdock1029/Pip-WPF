using DevExpress.Xpf.WindowsUI.Navigation;

namespace Pip.UI.Components.Auctions;

public partial class AuctionsView : INavigationAware
{
	public AuctionsView()
	{
		InitializeComponent();
	}

	public void NavigatedTo(NavigationEventArgs e)
	{
		if (DataContext is not null) return;
		if (e.Parameter is AuctionsViewModel vm) DataContext = vm;
	}

	public void NavigatingFrom(NavigatingEventArgs e)
	{
	}

	public void NavigatedFrom(NavigationEventArgs e)
	{
	}
}