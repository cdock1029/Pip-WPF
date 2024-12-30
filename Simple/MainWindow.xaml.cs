using System.Windows;

namespace Simple;

public partial class MainWindow
{
	public MainWindow()
	{
		InitializeComponent();
	}

	private void MenuItem_OnClick(object sender, RoutedEventArgs e)
	{
		Close();
	}
}