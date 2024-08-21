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

    private readonly PipSettings _pipSettings;

    public MainWindow()
    {
        InitializeComponent();
        _pipSettings = new PipSettings();

        SourceInitialized += MainWindow_SourceInitialized;
        Closing += MainWindow_Closing;
    }

    private void MainWindow_SourceInitialized(object? sender, EventArgs e)
    {
        try
        {
            // Load window placement details for previous application session from application settings
            // Note - if window was closed on a monitor that is now disconnected from the computer,
            //        SetWindowPlacement will place the window onto a visible monitor.
            //var wp = Settings.Default.WindowPlacement;
            //_pipSettings.Reload();
            var wp = _pipSettings.WindowPlacement;
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
            // Persist window placement details to application settings
            WindowPlacement wp;
            var hwnd = new WindowInteropHelper(this).Handle;
            GetWindowPlacement(hwnd, out wp);

            /*
            Settings.Default.WindowPlacement = wp;
            Settings.Default.Save();
            */
            _pipSettings.WindowPlacement = wp;
            _pipSettings.Save();
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
