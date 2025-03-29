using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pip.DataAccess;
using Pip.DataAccess.Services;
using Pip.UI.Components.Auctions;
using Pip.UI.Components.Details;
using Pip.UI.Components.Home;
using Pip.UI.Components.Investments;
using Pip.UI.Components.Main;
using Pip.UI.Components.Search;

namespace Pip.UI;

public partial class App
{
    private IServiceProvider _serviceProvider = null!;

    public App()
    {
        CompatibilitySettings.UseLightweightThemes = true;

        ThemedWindow.UseNativeWindow = false;

        //ApplicationThemeHelper.ApplicationThemeName = LightweightTheme.Win10System.Name;
        ApplicationThemeHelper.ApplicationThemeName = LightweightTheme.Win11System.Name;

        Theme.CachePaletteThemes = true;
        Theme.RegisterPredefinedPaletteThemes();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        try
        {
            base.OnStartup(e);

            await ApplicationThemeHelper.PreloadAsync(PreloadCategories.Grid, PreloadCategories.Docking);

            ConfigureServices(out ServiceCollection serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();

            PipDbContext dbContext = _serviceProvider.GetRequiredService<PipDbContext>();
            await Task.Run(() => dbContext.Database.Migrate());

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
            .AddSingleton<IMessageBoxService, DXMessageBoxService>()
            .AddSingleton<MainViewModel>()
            .AddSingleton<HomeViewModel>()
            .AddSingleton<SearchViewModel>()
            .AddSingleton<InvestmentsViewModel>()
            .AddSingleton<AuctionsViewModel>()
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
        /*
        IMessageBoxService messageBoxService = _serviceProvider.GetRequiredService<IMessageBoxService>();
        messageBoxService.Show($"Unhandled Exception. Contact administrator: [{e.Exception.Message}]", "Error",
            MessageBoxButton.OK);
        */
        ThemedMessageBox.Show("Application Error",
            $"{ex.GetType()}: {ex.Message}",
            MessageBoxButton.OK, MessageBoxImage.Error);
    }
}