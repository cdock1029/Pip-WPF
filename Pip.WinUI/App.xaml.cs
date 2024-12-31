using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Pip.DataAccess;
using Pip.WinUI.Activation;
using Pip.WinUI.Contracts.Services;
using Pip.WinUI.Core.Contracts.Services;
using Pip.WinUI.Core.Services;
using Pip.WinUI.Models;
using Pip.WinUI.Services;
using Pip.WinUI.ViewModels;
using Pip.WinUI.Views;
using Syncfusion.Licensing;
using UnhandledExceptionEventArgs = Microsoft.UI.Xaml.UnhandledExceptionEventArgs;

namespace Pip.WinUI;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
	public App()
	{
		SyncfusionLicenseProvider.RegisterLicense(
			"Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWH1dcHVdRGdZUEFxXEE=");

		InitializeComponent();

		Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder().UseContentRoot(AppContext.BaseDirectory)
			.ConfigureServices((context, services) =>
			{
				// Default Activation Handler
				services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

				// Other Activation Handlers

				// db
				services.AddDbContext<PipDbContext>();

				// Services
				services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
				services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
				services.AddSingleton<IActivationService, ActivationService>();
				services.AddSingleton<IPageService, PageService>();
				services.AddSingleton<INavigationService, NavigationService>();

				// Core Services
				services.AddSingleton<IFileService, FileService>();

				// Views and ViewModels
				services.AddTransient<SettingsViewModel>();
				services.AddTransient<SettingsPage>();
				services.AddSingleton<MainViewModel>();
				services.AddTransient<MainPage>();
				services.AddTransient<ShellPage>();
				services.AddTransient<ShellViewModel>();

				// Configuration
				services.Configure<LocalSettingsOptions>(
					context.Configuration.GetSection(nameof(LocalSettingsOptions)));
			}).Build();

		UnhandledException += App_UnhandledException;
	}

	// The .NET Generic Host provides dependency injection, configuration, logging, and other services.
	// https://docs.microsoft.com/dotnet/core/extensions/generic-host
	// https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
	// https://docs.microsoft.com/dotnet/core/extensions/configuration
	// https://docs.microsoft.com/dotnet/core/extensions/logging
	public IHost Host { get; }

	public static WindowEx MainWindow { get; } = new MainWindow();

	public static UIElement? AppTitlebar { get; set; }

	public static T GetService<T>()
		where T : class
	{
		if ((Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
			throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");

		return service;
	}

	private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		// TODO: Log and handle exceptions as appropriate.
		// https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
	}

	protected override async void OnLaunched(LaunchActivatedEventArgs args)
	{
		base.OnLaunched(args);

		var dbContext = GetService<PipDbContext>();
		dbContext.Database.Migrate();

		await GetService<IActivationService>().ActivateAsync(args);
	}
}