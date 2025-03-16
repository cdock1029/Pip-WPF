using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Web.WebView2.Core;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Color = System.Drawing.Color;

namespace Td;

public partial class MainWindow
{
	private const int ShowNormal = 1;
	private const int ShowMinimized = 2;
	private readonly ReloadNotifierService _reloadService;
	private readonly Settings _settings;

	public MainWindow(ReloadNotifierService reloadService, Settings settings)
	{
		_settings = settings;
		_reloadService = reloadService;

		ApplicationThemeManager.ApplySystemTheme();

		const WindowBackdropType wb = WindowBackdropType.Mica;
		WindowBackdropType = wb;

		SystemThemeWatcher.Watch(this);

		InitializeComponent();

		BlazorWebView.BlazorWebViewInitializing += BlazorWebViewInitializing;

		BlazorWebView.BlazorWebViewInitialized += BlazorWebViewInitialized;

		//BackButton.Click += BackButton_Click;
	}


	public bool CanGoBack { get; set; }

	private void BlazorWebViewInitializing(object? sender, BlazorWebViewInitializingEventArgs args)
	{
		args.EnvironmentOptions = new CoreWebView2EnvironmentOptions
		{
			ScrollBarStyle = CoreWebView2ScrollbarStyle.FluentOverlay
		};
		BlazorWebView.WebView.DefaultBackgroundColor = Color.Transparent;
	}

	private void BlazorWebViewInitialized(object? sender, BlazorWebViewInitializedEventArgs args)
	{
		CoreWebView2? webView2 = args.WebView.CoreWebView2;
		webView2.Settings.AreDefaultContextMenusEnabled = true;
		webView2.NavigationStarting += WebView2_NavigationStarting;
		webView2.NavigationCompleted += WebView2_NavigationCompleted;
		webView2.HistoryChanged += CoreWebView2_HistoryChanged;
    }

	private void CoreWebView2_HistoryChanged(object? sender, object e)
	{
		CanGoBack = BlazorWebView.WebView.CoreWebView2.CanGoBack;
		//BindingOperations.GetBindingExpression(BackButton, IsEnabledProperty)?.UpdateTarget();
	}

	private void WebView2_NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
	{
		Dispatcher.InvokeAsync(() => _reloadService.Update(true));
	}


	private void WebView2_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
	{
		Dispatcher.InvokeAsync(() => _reloadService.Update(false));
	}

	protected override void OnSourceInitialized(EventArgs e)
	{
		base.OnSourceInitialized(e);

		try
		{
			WindowPlacement wp = _settings.WindowPlacement;
			wp.length = Marshal.SizeOf(typeof(WindowPlacement));
			wp.flags = 0;
			wp.showCmd = wp.showCmd == ShowMinimized ? ShowNormal : wp.showCmd;
			IntPtr hwnd = new WindowInteropHelper(this).Handle;
			bool _ = SetWindowPlacement(hwnd, ref wp);
		}
		catch
		{
			// ignored
		}
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		base.OnClosing(e);

		IntPtr hwnd = new WindowInteropHelper(this).Handle;
		bool _ = GetWindowPlacement(hwnd, out WindowPlacement wp);
		_settings.WindowPlacement = wp;
		_settings.Save();
	}

	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static partial bool SetWindowPlacement(IntPtr hWnd, ref WindowPlacement windowPlacement);

	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static partial bool GetWindowPlacement(IntPtr hWnd, out WindowPlacement windowPlacement);

	private void Quit_OnClick(object sender, RoutedEventArgs e)
	{
		Close();
	}
}