using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;
using Pip.WinUI.Contracts.Services;

namespace Pip.WinUI.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
	public ShellViewModel(INavigationService navigationService)
	{
		NavigationService = navigationService;
		NavigationService.Navigated += OnNavigated;
	}

	[ObservableProperty] public partial bool IsBackEnabled { get; set; }

	public INavigationService NavigationService { get; }

	private void OnNavigated(object sender, NavigationEventArgs e)
	{
		IsBackEnabled = NavigationService.CanGoBack;
	}

	[RelayCommand]
	private void MenuFileExit()
	{
		Application.Current.Exit();
	}

	[RelayCommand]
	private void MenuSettings()
	{
		NavigationService.NavigateTo(typeof(SettingsViewModel).FullName!);
	}

	[RelayCommand]
	private void MenuViewsMain()
	{
		NavigationService.NavigateTo(typeof(MainViewModel).FullName!);
	}
}