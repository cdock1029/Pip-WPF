using System.Windows;
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
        var mainWindow = _serviceProvider.GetService<MainWindow>();
        mainWindow?.Show();
    }

    private static void ConfigureServices(ServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<MainWindow>()
            .AddSingleton<MainViewModel>()
            .AddSingleton<SearchViewModel>()
            .AddSingleton<TreasuriesViewModel>()
            .AddSingleton<UpcomingAuctionsViewModel>()
            .AddSingleton<AuctionsViewModel>()
            .AddHttpClient()
            .AddSingleton<IMessageDialogService, MessageDialogService>()
            .AddSingleton<TreasuryDataProvider>()
            .AddSingleton<ITreasuryDataProvider>(p => p.GetRequiredService<TreasuryDataProvider>())
            .AddDbContextFactory<PipDbContext>(optionsBuilder =>
            {
                //var connString = ConfigurationManager.ConnectionStrings["PipDbLocal"].ConnectionString;
                //optionsBuilder.UseSqlServer(connString);
                optionsBuilder.UseSqlite("Data Source=pip.db");
                optionsBuilder.UseLazyLoadingProxies();
            });
    }
}
