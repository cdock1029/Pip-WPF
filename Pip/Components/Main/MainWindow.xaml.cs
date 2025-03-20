using System.Windows;
using Pip.UI.Properties;
using SystemColors = System.Windows.SystemColors;

namespace Pip.UI.Components.Main;

public partial class MainWindow
{
    public MainWindow(MainViewModel mainViewModel)
    {
		ActiveGlowColor = SystemColors.AccentColorBrush;
		InitializeComponent();

		DataContext = mainViewModel;

        SourceInitialized += MainWindow_SourceInitialized;
		Closing += MainWindow_Closing;
	}

	private void MainWindow_SourceInitialized(object? sender, EventArgs e)
    {
        if (Settings.Default.WindowWidth > 0 && Settings.Default.WindowHeight > 0)
        {
            Left = Settings.Default.WindowLeft;
            Top = Settings.Default.WindowTop;
            Width = Settings.Default.WindowWidth;
            Height = Settings.Default.WindowHeight;

            WindowState savedState = Settings.Default.WindowState;
            WindowState = savedState != WindowState.Minimized ? savedState : WindowState.Normal;
        }
    }

    private void MainWindow_Closing(object? sender, EventArgs e)
    {
        if (WindowState == WindowState.Normal)
        {
            Settings.Default.WindowLeft = Left;
            Settings.Default.WindowTop = Top;
            Settings.Default.WindowWidth = Width;
            Settings.Default.WindowHeight = Height;
        }
        else
        {
            Rect bounds = RestoreBounds;
            Settings.Default.WindowLeft = bounds.Left;
            Settings.Default.WindowTop = bounds.Top;
            Settings.Default.WindowWidth = bounds.Width;
            Settings.Default.WindowHeight = bounds.Height;
        }

        Settings.Default.WindowState = WindowState;
        Settings.Default.Save();
    }

    private void Show_Flyout(object sender, RoutedEventArgs e)
    {
        FlyoutControl.IsOpen = true;
    }
}

