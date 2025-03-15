using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using JetBrains.Annotations;
using Pip.UI.Properties;

namespace Pip.UI.Components.Main;

public partial class MainWindow
{
	private const int ShowNormal = 1;
	private const int ShowMinimized = 2;

	private readonly PipSettings _pipSettings;

	public MainWindow(MainViewModel mainViewModel, PipSettings pipSettings)
	{
		InitializeComponent();

		DataContext = mainViewModel;
		_pipSettings = pipSettings;

		SourceInitialized += MainWindow_SourceInitialized;
		Closing += MainWindow_Closing;
	}

	private void MainWindow_SourceInitialized(object? sender, EventArgs e)
	{
		try
		{
			WindowPlacement wp = _pipSettings.WindowPlacement;
			wp.Length = Marshal.SizeOf<WindowPlacement>();
			wp.Flags = 0;
			if (wp.ShowCmd == ShowMinimized) wp.ShowCmd = ShowNormal;
			IntPtr hwnd = new WindowInteropHelper(this).Handle;
			WindowPlacementHelper.SetWindowPlacement(hwnd, ref wp);
		}

		catch (ExternalException ex)
		{
			Debug.WriteLine($"Error accessing WindowPosition: {ex}");
		}
	}

	private void MainWindow_Closing(object? sender, EventArgs e)
	{
		try
		{
			IntPtr hwnd = new WindowInteropHelper(this).Handle;
			WindowPlacementHelper.GetWindowPlacement(hwnd, out WindowPlacement wp);

			Debug.WriteLine($"closing wp: MinPos={wp.MinPosition}, MaxPos={wp.MaxPosition}");
			_pipSettings.WindowPlacement = wp;
			_pipSettings.Save();
		}
		catch (ExternalException ex)
		{
			Debug.WriteLine($"Exception saving settings: {ex}");
		}
	}
}

public static partial class WindowPlacementHelper
{
	[LibraryImport("user32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool SetWindowPlacement(IntPtr hWnd, ref WindowPlacement windowPlacement);

	[LibraryImport("user32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool GetWindowPlacement(IntPtr hWnd, out WindowPlacement windowPlacement);
}

[Serializable]
[StructLayout(LayoutKind.Sequential)]
[PublicAPI]
public record struct WindowPlacement(
	int Length = 0,
	int Flags = 0,
	int ShowCmd = 0,
	Point MinPosition = default,
	Point MaxPosition = default,
	Rect NormalPosition = default
);

[Serializable]
[StructLayout(LayoutKind.Sequential)]
public record struct Rect(int Left = 20, int Top = 20, int Right = 200, int Bottom = 200);

[Serializable]
[StructLayout(LayoutKind.Sequential)]
public record struct Point(int X, int Y);