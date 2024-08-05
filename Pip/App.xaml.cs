using System.Configuration;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pip.DataAccess;
using Pip.UI.Data;
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
            .AddTransient<MainWindow>()
            .AddTransient<MainViewModel>()
            .AddTransient<SearchViewModel>()
            .AddTransient<TreasuriesViewModel>()
            .AddTransient<UpcomingAuctionsViewModel>()
            .AddTransient<AuctionsViewModel>()
            .AddHttpClient()
            .AddSingleton<TreasuryDataProvider>()
            .AddTransient<ITreasuryDataProvider>(p => p.GetRequiredService<TreasuryDataProvider>())
            .AddDbContextFactory<PipDbContext>(options =>
            {
                var connString = ConfigurationManager.ConnectionStrings["PipDbLocal"].ConnectionString;
                options.UseSqlServer(connString);
            }, ServiceLifetime.Transient);
    }
}
