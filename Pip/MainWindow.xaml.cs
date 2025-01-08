using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using JetBrains.Annotations;
using Pip.UI.Properties;
using Pip.UI.View.Types;
using Pip.UI.ViewModel;

namespace Pip.UI;

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

		PopupBaseEdit.IsKeyboardFocusWithinChanged += PopupBaseEdit_IsKeyboardFocusedWithinChanged;
	}

	private void PopupBaseEdit_IsKeyboardFocusedWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		var newVal = (bool)e.NewValue;
		var hasResults = ((MainViewModel)DataContext).SearchViewModel.HasSearchResults;
		if (newVal && hasResults) PopupBaseEdit.ShowPopup();
	}

	private void MainWindow_SourceInitialized(object? sender, EventArgs e)
	{
		try
		{
			// Load window placement details for previous application session from application settings
			// Note - if window was closed on a monitor that is now disconnected from the computer,
			//        SetWindowPlacement will place the window onto a visible monitor.
			var wp = _pipSettings.WindowPlacement;
			wp.length = Marshal.SizeOf(typeof(WindowPlacement));
			wp.flags = 0;
			wp.showCmd = wp.showCmd == ShowMinimized ? ShowNormal : wp.showCmd;
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
			var hwnd = new WindowInteropHelper(this).Handle;
			GetWindowPlacement(hwnd, out var wp);

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

	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	[UsedImplicitly]
	public static partial bool SetWindowPlacement(IntPtr hWnd, ref WindowPlacement windowPlacement);

	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	[UsedImplicitly]
	public static partial bool GetWindowPlacement(IntPtr hWnd, out WindowPlacement windowPlacement);
}