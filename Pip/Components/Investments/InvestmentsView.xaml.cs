using DevExpress.Xpf.WindowsUI.Navigation;

namespace Pip.UI.Components.Investments;

public partial class InvestmentsView : INavigationAware
{
	public InvestmentsView()
    {
        InitializeComponent();
    }

    public void NavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is InvestmentsViewModel vm) DataContext ??= vm;
    }

	public void NavigatingFrom(NavigatingEventArgs e)
	{
	}

	public void NavigatedFrom(NavigationEventArgs e)
	{
	}
}