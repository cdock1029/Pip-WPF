using System.Windows;
using System.Windows.Threading;
using DevExpress.Blazor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MessageBox = System.Windows.MessageBox;

namespace Td;

public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        ServiceCollection serviceCollection = new();
        ConfigureServices(serviceCollection);

        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        Resources.Add("services", serviceProvider);

        PipDbContext dbContext = serviceProvider.GetRequiredService<PipDbContext>();
        dbContext.Database.Migrate();

        MainWindow mainWindow = serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private static void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<MainWindow>();

        serviceCollection.AddDbContext<PipDbContext>(ServiceLifetime.Singleton);
        serviceCollection.AddWpfBlazorWebView();

        serviceCollection.AddDevExpressBlazor(configure =>
        {
            configure.SizeMode = SizeMode.Medium;
            //configure.BootstrapVersion = BootstrapVersion.v5;
        });


        serviceCollection.AddMemoryCache();
        serviceCollection.AddHttpClient<ITreasuryDataProvider, TreasuryDataProvider>();
        serviceCollection.AddSingleton<ReloadNotifierService>();
        serviceCollection.AddSingleton<Settings>();
        serviceCollection.AddSingleton<AppState>();
        serviceCollection.AddFluentUIComponents();
#if DEBUG
        serviceCollection.AddBlazorWebViewDeveloperTools();
        serviceCollection.AddLogging(logging =>
        {
            logging.AddFilter("Microsoft.AspNetCore.Components.WebView", LogLevel.Trace);
            logging.AddDebug();
        });
#endif
    }

    private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
#if DEBUG
        MessageBox.Show(e.Exception.ToString(), "Error");
#else
		MessageBox.Show("An error has occurred.", "Error");
#endif
        Debug.WriteLine($"Unhandled Exception: {e.Exception}");
    }
}