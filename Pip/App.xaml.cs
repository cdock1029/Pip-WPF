using System.Windows;
using System.Windows.Threading;
using DevExpress.Xpf.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pip.UI.Components.Auctions;
using Pip.UI.Components.Details;
using Pip.UI.Components.Historical;
using Pip.UI.Components.Home;
using Pip.UI.Components.Investments;
using Pip.UI.Components.Main;
using Pip.UI.Components.Search;
using Pip.UI.Data;
using Pip.UI.Data.Services;

namespace Pip.UI;

public partial class App
{
    private IServiceProvider _serviceProvider = null!;
    private ILogger<App> _log = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        CompatibilitySettings.UseLightweightThemes = true;
        Theme.CachePaletteThemes = true;
        Theme.RegisterPredefinedPaletteThemes();
        ApplicationThemeHelper.ApplicationThemeName = LightweightTheme.Win11SystemColors.Name;
        _serviceProvider = ConfigureServices();
        _log = _serviceProvider.GetRequiredService<ILogger<App>>();

        DispatcherUnhandledException += App_DispatcherUnhandledException;
        TaskScheduler.UnobservedTaskException += (_, args) =>
        {
            _log.LogError(args.Exception, "Unobserved task exception");
            args.SetObserved();
        };

        _ = StartupAsync();
    }

    private async Task StartupAsync()
    {
        try
        {
            Task dbTask = Task.Run(() =>
            {
                PipDbContext dbContext = _serviceProvider.GetRequiredService<PipDbContext>();
                dbContext.Database.Migrate();
            });

            Task uiPreloadTask = ApplicationThemeHelper.PreloadAsync(PreloadCategories.Grid, PreloadCategories.Docking);

            await dbTask;

            MainWindow mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            await uiPreloadTask;
        }

        catch (Exception ex)
        {
            _log.LogCritical(ex, "OnStartup exception");
            ShowError(ex);
            Environment.Exit(1);
        }
    }

    private static IServiceProvider ConfigureServices()
    {
        ServiceCollection serviceCollection = [];

        serviceCollection.AddLogging(builder =>
        {
#if DEBUG
            builder.AddDebug();
#endif
            builder.AddEventLog();
        });

        serviceCollection
            .AddMemoryCache()
            .AddSingleton<MainViewModel>()
            .AddSingleton<HomeViewModel>()
            .AddSingleton<SearchViewModel>()
            .AddSingleton<InvestmentsViewModel>()
            .AddSingleton<AuctionsViewModel>()
            .AddSingleton<HistoricalViewModel>()
            .AddSingleton<DetailsViewModel>()
            .AddSingleton<MainWindow>()
            .AddDbContext<PipDbContext>(ServiceLifetime.Singleton)
            .AddHttpClient<ITreasuryDataProvider, TreasuryDataProvider>();
        return serviceCollection.BuildServiceProvider();
    }

    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        e.Handled = true;
        _log.LogError(e.Exception, "Dispatcher Unhandled Exception");
        ShowError(e.Exception);
    }

    private static void ShowError(Exception ex)
    {
        ThemedMessageBox.Show("Application Error",
            $"{ex.GetType()}: {ex.Message}\n{ex.StackTrace}",
            MessageBoxButton.OK, MessageBoxImage.Error);
    }
}