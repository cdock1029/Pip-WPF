using CommunityToolkit.Mvvm.ComponentModel;
using Pip.UI.ViewModel;

namespace Pip.UI.Services;

public partial class NavigationService(Func<Type, ViewModelBase> viewModelFactory)
	: ObservableObject, INavigationService
{
	[ObservableProperty] private ViewModelBase? _currentView;

	public void NavigateTo<TViewModelBase>() where TViewModelBase : ViewModelBase
	{
		CurrentView = viewModelFactory.Invoke(typeof(TViewModelBase));
	}

	public async Task NavigateToAsync<TViewModelBase>() where TViewModelBase : ViewModelBase
	{
		CurrentView = await Task.Run(() => viewModelFactory.Invoke(typeof(TViewModelBase)));
	}
}

public interface INavigationService
{
	ViewModelBase CurrentView { get; }

	void NavigateTo<T>() where T : ViewModelBase;

	Task NavigateToAsync<T>() where T : ViewModelBase;
}
