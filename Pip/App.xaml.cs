using System.Windows;
using System.Windows.Threading;
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
using Application = System.Windows.Application;
using INavigationService = Pip.UI.Services.INavigationService;
using ViewModelBase = Pip.UI.ViewModel.ViewModelBase;

namespace Pip.UI;

public partial class App : Application
{
	private readonly ServiceProvider _serviceProvider;

	public App()
	{
		CompatibilitySettings.UseLightweightThemes = true;
		ApplicationThemeHelper.ApplicationThemeName = LightweightTheme.Win10System.Name;
		ServiceCollection serviceCollection = [];
		ConfigureServices(serviceCollection);
		_serviceProvider = serviceCollection.BuildServiceProvider();
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);
		var dbContext = _serviceProvider.GetRequiredService<PipDbContext>();
		dbContext.Database.Migrate();
		_serviceProvider.GetRequiredService<SavedTreasuriesViewModel>().IsActive = true;
		_serviceProvider.GetRequiredService<InvestmentsViewModel>().IsActive = true;
		var mainWindow = _serviceProvider.GetService<MainWindow>();
		mainWindow?.Show();
	}

	private static void ConfigureServices(ServiceCollection serviceCollection)
	{
		serviceCollection
			.AddSingleton<INavigationService, NavigationService>()
			.AddSingleton<IMessageBoxService, DXMessageBoxService>()
			.AddSingleton<ITreasuryDataProvider, TreasuryDataProvider>()
			.AddSingleton<MainViewModel>()
			.AddSingleton<SearchViewModel>()
			.AddActivatedSingleton<SavedTreasuriesViewModel>()
			.AddSingleton<InvestmentsViewModel>()
			.AddSingleton<AuctionsViewModel>()
			.AddSingleton(p => new MainWindow { DataContext = p.GetRequiredService<MainViewModel>() })
			.AddSingleton<Func<Type, ViewModelBase>>(p =>
				viewModelType => (ViewModelBase)p.GetRequiredService(viewModelType))
			.AddDbContextFactory<PipDbContext>(optionsBuilder =>
			{
				//var connString = ConfigurationManager.ConnectionStrings["PipDbLocal"].ConnectionString;
				//optionsBuilder.UseSqlServer(connString);
				optionsBuilder.UseSqlite("Data Source=pip.db");
			})
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
