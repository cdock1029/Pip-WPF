using CommunityToolkit.Mvvm.ComponentModel;
using Pip.UI.ViewModel;

namespace Pip.UI.Services;

public partial class NavigationService(Func<Type, ViewModelBase> viewModelFactory)
	: ObservableObject, INavigationService
{
	[ObservableProperty] private ViewModelBase? _currentView;

	public async Task NavigateToAsync<TViewModelBase>() where TViewModelBase : ViewModelBase
	{
		CurrentView = viewModelFactory.Invoke(typeof(TViewModelBase));
		await CurrentView.LoadAsync();
	}
}

public interface INavigationService
{
	ViewModelBase CurrentView { get; }

	Task NavigateToAsync<T>() where T : ViewModelBase;
}
