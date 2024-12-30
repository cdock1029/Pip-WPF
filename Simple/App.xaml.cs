using System.Windows;
using DevExpress.Xpf.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pip.DataAccess;

namespace Simple;

public partial class App
{
	//private readonly ServiceProvider _serviceProvider;


	public App()
	{
		CompatibilitySettings.UseLightweightThemes = true;
		//ThemedWindow.UseNativeWindow = true;

		//ApplicationThemeHelper.ApplicationThemeName = Theme.Win10SystemColorsName;
		ApplicationThemeHelper.ApplicationThemeName = LightweightTheme.Win10SystemColors.Name;
		//ApplicationThemeHelper.ApplicationThemeName = Theme.Win11System.Name;
		ServiceCollection serviceCollection = [];
		//ConfigureServices(serviceCollection);
		//_serviceProvider = serviceCollection.BuildServiceProvider();

		//Current.ThemeMode = ThemeMode.System;
	}


	protected override void OnStartup(StartupEventArgs e)
	{
		using var context = new PipDbContext(new DbContextOptions<PipDbContext>());
		context.Database.Migrate();

		//var mainWindow = new MainWindow { ThemeMode = ThemeMode.System };
		var mainWindow = new MainWindow();
		mainWindow.Show();
	}

	private static void ConfigureServices(ServiceCollection serviceCollection)
	{
		serviceCollection.AddDbContext<PipDbContext>();
	}
}