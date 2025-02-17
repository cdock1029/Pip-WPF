using System.Windows;
using System.Windows.Threading;
using DevExpress.Blazor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor;
using Syncfusion.Licensing;
using MessageBox = System.Windows.MessageBox;

namespace Td;

public partial class App
{
	public App()
	{
		SyncfusionLicenseProvider.RegisterLicense(
			"Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWX5ednVWQmReWUR+XkE=");
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		var serviceCollection = new ServiceCollection();
		ConfigureServices(serviceCollection);

		var serviceProvider = serviceCollection.BuildServiceProvider();
		Resources.Add("services", serviceProvider);

		var dbContext = serviceProvider.GetRequiredService<PipDbContext>();
		dbContext.Database.Migrate();

		var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
		mainWindow.Show();
	}

	private void ConfigureServices(IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<MainWindow>();
		serviceCollection.AddDbContext<PipDbContext>(ServiceLifetime.Singleton);
		serviceCollection.AddWpfBlazorWebView();

		serviceCollection.AddDevExpressBlazor(configure =>
		{
			configure.BootstrapVersion = BootstrapVersion.v5;
			configure.SizeMode = SizeMode.Medium;
		});

		serviceCollection.AddSyncfusionBlazor();

		serviceCollection.AddMemoryCache();
		serviceCollection.AddHttpClient<ITreasuryDataProvider, TreasuryDataProvider>();
		serviceCollection.AddSingleton<SearchComponentState>();
		serviceCollection.AddSingleton<InvestmentPageState>();
		serviceCollection.AddSingleton<ReloadNotifierService>();
		serviceCollection.AddSingleton<Settings>();
		serviceCollection.AddSingleton<AppState>();
		serviceCollection.AddFluentUIComponents();
#if DEBUG
		serviceCollection.AddBlazorWebViewDeveloperTools();
		serviceCollection.AddLogging(logging =>
		{
			logging.AddFilter("Microsoft.AspNetCore.Components.WebView", LogLevel.Trace);
			logging.AddDebug();
		});
#endif
	}

	private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
#if DEBUG
		MessageBox.Show(e.Exception.ToString(), "Error");
#else
		MessageBox.Show("An error has occurred.", "Error");
#endif
		Debug.WriteLine($"Unhandled Exception: {e.Exception}");
	}
}