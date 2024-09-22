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

namespace Pip.UI;

public partial class App : System.Windows.Application
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
			.AddSingleton<Services.INavigationService, NavigationService>()
			.AddSingleton<IMessageBoxService, DXMessageBoxService>()
			.AddTransient<ITreasuryDataProvider, TreasuryDataProvider>()
			.AddSingleton<MainViewModel>()
			.AddSingleton<SearchViewModel>()
			.AddSingleton<SavedTreasuriesViewModel>()
			.AddSingleton<InvestmentsViewModel>()
			.AddSingleton<AuctionsViewModel>()
			.AddSingleton(p => Dispatcher.CurrentDispatcher)
			.AddSingleton(p => new MainWindow { DataContext = p.GetRequiredService<MainViewModel>() })
			.AddSingleton<Func<Type, ViewModel.ViewModelBase>>(p =>
				viewModelType => (ViewModel.ViewModelBase)p.GetRequiredService(viewModelType))
			.AddDbContextFactory<PipDbContext>(optionsBuilder =>
			{
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
