using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using DevExpress.Xpf.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pip.DataAccess;
using Pip.DataAccess.Services;
using Pip.UI.Components.Auctions;
using Pip.UI.Components.Details;
using Pip.UI.Components.Historical;
using Pip.UI.Components.Home;
using Pip.UI.Components.Investments;
using Pip.UI.Components.Main;
using Pip.UI.Components.Search;

namespace Pip.UI;

public partial class App
{
    private IServiceProvider _serviceProvider = null!;

    protected override async void OnStartup(StartupEventArgs e)
    {
        try
        {
            base.OnStartup(e);

            CompatibilitySettings.UseLightweightThemes = true;
            ApplicationThemeHelper.ApplicationThemeName = LightweightTheme.Win11SystemColors.Name;

            Theme.CachePaletteThemes = true;
            Theme.RegisterPredefinedPaletteThemes();

            await ApplicationThemeHelper.PreloadAsync(PreloadCategories.Grid, PreloadCategories.Docking,
                PreloadCategories.Accordion, PreloadCategories.Controls, PreloadCategories.Core);

            ConfigureServices(out ServiceCollection serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();

            PipDbContext dbContext = _serviceProvider.GetRequiredService<PipDbContext>();
            await Task.Run(dbContext.Database.Migrate);


            MainWindow mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"OnStartup exception: {ex.Message}");
            Environment.Exit(1);
        }
    }

    private static void ConfigureServices(out ServiceCollection serviceCollection)
    {
        serviceCollection = [];
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
            .AddDbContext<PipDbContext>(ServiceLifetime.Transient)
            .AddHttpClient<ITreasuryDataProvider, TreasuryDataProvider>();
    }

    private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        e.Handled = true;
        Debug.WriteLine(e.Exception);
        ShowError(e.Exception);
    }

    public static void ShowError(Exception ex)
    {
        ThemedMessageBox.Show("Application Error",
            $"{ex.GetType()}: {ex.Message}",
            MessageBoxButton.OK, MessageBoxImage.Error);
    }
}