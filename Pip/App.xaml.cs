using System.Windows;
using Microsoft.Extensions.DependencyInjection;
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
            .AddTransient<AnnouncedViewModel>()
            .AddTransient<AuctionsViewModel>()
            .AddHttpClient()
            .AddSingleton<TreasuryDataProvider>()
            .AddSingleton<ITreasuryDataProvider>(p => p.GetRequiredService<TreasuryDataProvider>());
    }
}
