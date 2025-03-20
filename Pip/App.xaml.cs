using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pip.DataAccess;
using Pip.DataAccess.Services;
using Pip.UI.Components.Auctions;
using Pip.UI.Components.Details;
using Pip.UI.Components.Home;
using Pip.UI.Components.Investments;
using Pip.UI.Components.Main;
using Pip.UI.Components.Search;

namespace Pip.UI;

public partial class App
{
	public App()
	{
		CompatibilitySettings.UseLightweightThemes = true;
		ThemedWindow.UseNativeWindow = false;
        //ApplicationThemeHelper.ApplicationThemeName = LightweightTheme.Office2019BlackBrickwork.Name;
        //ApplicationThemeHelper.ApplicationThemeName = LightweightTheme.Win10SystemColors.Name;
        ApplicationThemeHelper.ApplicationThemeName = LightweightTheme.Win11System.Name;
        //ApplicationThemeHelper.ApplicationThemeName = LightweightTheme.VS2019System.Name;
	}

	private IServiceProvider ServiceProvider { get; set; } = null!;

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);
		Task.Run(() => ApplicationThemeHelper.PreloadAsync(PreloadCategories.Grid, PreloadCategories.LayoutControl));

		ServiceCollection serviceCollection = [];
		ConfigureServices(serviceCollection);
		ServiceProvider = serviceCollection.BuildServiceProvider();

		PipDbContext dbContext = ServiceProvider.GetRequiredService<PipDbContext>();
		dbContext.Database.Migrate();

		MainWindow mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
		mainWindow.Show();
	}

	private static void ConfigureServices(ServiceCollection serviceCollection)
    {
        serviceCollection
            .AddMemoryCache()
            .AddSingleton<IMessageBoxService, DXMessageBoxService>()
            .AddSingleton<MainViewModel>()
            .AddSingleton<HomeViewModel>()
            .AddSingleton<SearchViewModel>()
            .AddSingleton<InvestmentsViewModel>()
            .AddSingleton<AuctionsViewModel>()
            .AddSingleton<DetailsViewModel>()
            .AddSingleton<MainWindow>()
            .AddDbContext<PipDbContext>(ServiceLifetime.Transient)
            .AddHttpClient<ITreasuryDataProvider, TreasuryDataProvider>();
    }

	private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
		e.Handled = true;
		Debug.WriteLine(e.Exception);
		IMessageBoxService messageBoxService = ServiceProvider.GetRequiredService<IMessageBoxService>();
        messageBoxService.Show($"Unhandled Exception. Contact administrator: [{e.Exception.Message}]", "Error",
            MessageBoxButton.OK);
	}
}