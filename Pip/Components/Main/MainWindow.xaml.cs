using System.ComponentModel;
using System.Windows;
using Pip.UI.Properties;

namespace Pip.UI.Components.Main;

public partial class MainWindow
{
    public MainWindow(MainViewModel mainViewModel)
    {
        InitializeComponent();

        DataContext = mainViewModel;

        Loaded += MainWindow_LoadedFocus;
    }

    private void MainWindow_LoadedFocus(object? sender, EventArgs e)
    {
        NavAccordion.Focus();
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        if (!(Settings.Default.WindowWidth > 0) || !(Settings.Default.WindowHeight > 0)) return;

        Left = Settings.Default.WindowLeft;
        Top = Settings.Default.WindowTop;
        Width = Settings.Default.WindowWidth;
        Height = Settings.Default.WindowHeight;

        WindowState savedState = Settings.Default.WindowState;
        WindowState = savedState != WindowState.Minimized ? savedState : WindowState.Normal;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
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

    private void ButtonEdit_OnIsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        var isFocusedWithin = (bool)e.NewValue;

        bool searchExecuted = ((MainViewModel)DataContext).SearchViewModel.SearchResults is not null;

        if (isFocusedWithin && searchExecuted)
            FlyoutControl.IsOpen = true;
    }

    private void FlyoutControl_OnClosing(object? sender, CancelEventArgs e)
    {
        if (((MainViewModel)DataContext).SearchViewModel.SearchResults is not null) e.Cancel = true;
    }
}