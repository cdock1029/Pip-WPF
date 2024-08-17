using System.Windows;
using System.Windows.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pip.DataAccess;
using Pip.UI.Data;
using Pip.UI.View.Services;
using Pip.UI.ViewModel;

namespace Pip.UI;

public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;

    public App()
    {
        ServiceCollection serviceCollection = [];
        ConfigureServices(serviceCollection);
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        // create SavedTreasuriesViewModel early so it can receive messages
        _serviceProvider.GetRequiredService<SavedTreasuriesViewModel>();
        var mainWindow = _serviceProvider.GetService<MainWindow>();
        mainWindow?.Show();
    }

    private static void ConfigureServices(ServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<INavigationService, NavigationService>()
            .AddSingleton<IMessageDialogService, MessageDialogService>()
            .AddSingleton<ITreasuryDataProvider, TreasuryDataProvider>()
            .AddSingleton<MainViewModel>()
            .AddSingleton<SearchViewModel>()
            .AddSingleton<SavedTreasuriesViewModel>()
            .AddSingleton<UpcomingAuctionsViewModel>()
            .AddSingleton<AuctionsViewModel>()
            .AddSingleton(p => new MainWindow { DataContext = p.GetRequiredService<MainViewModel>() })
            .AddSingleton<Func<Type, ViewModelBase>>(p =>
                viewModelType => (ViewModelBase)p.GetRequiredService(viewModelType))
            .AddHttpClient()
            .AddDbContextFactory<PipDbContext>(optionsBuilder =>
            {
                //var connString = ConfigurationManager.ConnectionStrings["PipDbLocal"].ConnectionString;
                //optionsBuilder.UseSqlServer(connString);
                optionsBuilder.UseSqlite("Data Source=pip.db");
            });
    }

    private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        e.Handled = true;
        var messageDialogService = _serviceProvider.GetService<IMessageDialogService>();
        messageDialogService?.ShowOkCancelDialog($"Unhandled Exception. Contact administrator: [{e.Exception}]",
            "Error");
    }
}
