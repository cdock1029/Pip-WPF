using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Pip.UI.Properties;
using Pip.UI.View.Types;

namespace Pip.UI;

public partial class MainWindow : Window
{
    private const int SwShownormal = 1;
    private const int SwShowminimized = 2;

    public MainWindow()
    {
        InitializeComponent();

        SourceInitialized += MainWindow_SourceInitialized;
        Closing += MainWindow_Closing;
    }

    private void MainWindow_SourceInitialized(object? sender, EventArgs e)
    {
        //try
        //{
        //    var bounds = Settings.Default.WindowPosition;
        //    if (bounds.IsEmpty) return;
        //    if (WindowStartupLocation == WindowStartupLocation.Manual)
        //    {
        //        Top = bounds.Top;
        //        Left = bounds.Left;
        //    }

        //    if (SizeToContent != SizeToContent.Manual) return;
        //    Width = bounds.Width;
        //    Height = bounds.Height;
        //}
        try
        {
            // Load window placement details for previous application session from application settings
            // Note - if window was closed on a monitor that is now disconnected from the computer,
            //        SetWindowPlacement will place the window onto a visible monitor.
            var wp = Settings.Default.WindowPlacement;
            wp.length = Marshal.SizeOf(typeof(WindowPlacement));
            wp.flags = 0;
            wp.showCmd = wp.showCmd == SwShowminimized ? SwShownormal : wp.showCmd;
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowPlacement(hwnd, ref wp);
        }

        catch (Exception ex)
        {
            Debug.WriteLine($"Error accessing WindowPosition: {ex}");
        }
    }

    private void MainWindow_Closing(object? sender, EventArgs e)
    {
        try
        {
            //Debug.WriteLine($"Saving MainWindow RestoreBounds {RestoreBounds}");
            //Settings.Default.WindowPosition = RestoreBounds;
            //Settings.Default.Save();

            // Persist window placement details to application settings
            WindowPlacement wp;
            var hwnd = new WindowInteropHelper(this).Handle;
            GetWindowPlacement(hwnd, out wp);
            Settings.Default.WindowPlacement = wp;
            Settings.Default.Save();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception saving settings: {ex}");
        }
    }

    [DllImport("user32.dll")]
    private static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WindowPlacement lpwndpl);

    [DllImport("user32.dll")]
    private static extern bool GetWindowPlacement(IntPtr hWnd, out WindowPlacement lpwndpl);
}
