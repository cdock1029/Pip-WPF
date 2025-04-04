using DevExpress.Xpf.WindowsUI.Navigation;

namespace Pip.UI.Components.Home;

public partial class HomeView : INavigationAware
{
	public HomeView()
	{
		InitializeComponent();
	}

	public void NavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is HomeViewModel vm) DataContext ??= vm;
    }

	public void NavigatingFrom(NavigatingEventArgs e)
	{
	}

	public void NavigatedFrom(NavigationEventArgs e)
	{
	}
}