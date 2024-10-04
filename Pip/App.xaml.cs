using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pip.DataAccess;
using Pip.UI.Components.Auctions;
using Pip.UI.Components.Investments;
using Pip.UI.Components.SavedTreasuries;
using Pip.UI.Components.Search;
using Pip.UI.Services;
using Pip.UI.ViewModel;
using System.Windows;
using System.Windows.Threading;
using INavigationService = Pip.UI.Services.INavigationService;

namespace Pip.UI;

public partial class App
{
	private readonly ServiceProvider _serviceProvider;

	public App()
	{
		CompatibilitySettings.UseLightweightThemes = true;
		ApplicationThemeHelper.ApplicationThemeName = LightweightTheme.Win11System.Name;
		ThemedWindow.UseNativeWindow = true;
		ServiceCollection serviceCollection = [];
		ConfigureServices(serviceCollection);
		_serviceProvider = serviceCollection.BuildServiceProvider();
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);
		var dbContext = _serviceProvider.GetRequiredService<PipDbContext>();
		dbContext.Database.Migrate();
		var mainWindow = _serviceProvider.GetService<MainWindow>();
		mainWindow?.Show();
	}

	private static void ConfigureServices(ServiceCollection serviceCollection)
	{
		serviceCollection
			.AddSingleton<INavigationService, NavigationService>()
			.AddSingleton<IMessageBoxService, DXMessageBoxService>()
			.AddTransient<ITreasuryDataProvider, TreasuryDataProvider>()
			.AddSingleton<MainViewModel>()
			.AddSingleton<SearchViewModel>()
			.AddSingleton<SavedTreasuriesViewModel>()
			.AddSingleton<InvestmentsViewModel>()
			.AddSingleton<AuctionsViewModel>()
			.AddSingleton(_ => Dispatcher.CurrentDispatcher)
			.AddSingleton(p => new MainWindow { DataContext = p.GetRequiredService<MainViewModel>() })
			.AddSingleton<Func<Type, PipViewModel>>(p =>
				viewModelType => (PipViewModel)p.GetRequiredService(viewModelType))
			.AddDbContextFactory<PipDbContext>(optionsBuilder =>
			{
				//var connectionString = ConfigurationManager.ConnectionStrings["PipDbLocal"].ConnectionString;
				//optionsBuilder.UseSqlServer(connectionString, ob => ob.MigrationsAssembly("Pip.DataAccess"));
				optionsBuilder.UseSqlite("Data Source=pip.db");
			}, ServiceLifetime.Transient)
			.AddHttpClient<ITreasuryDataProvider, TreasuryDataProvider>(c =>
			{
				c.BaseAddress = new Uri("https://www.treasurydirect.gov/TA_WS/");
			});
	}

	private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
		e.Handled = true;
		var messageBoxService = _serviceProvider.GetRequiredService<IMessageBoxService>();
		messageBoxService.Show($"Unhandled Exception. Contact administrator: [{e.Exception}]", "Error",
			MessageBoxButton.OK);
	}
}
