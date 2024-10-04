using CommunityToolkit.Mvvm.ComponentModel;
using Pip.UI.ViewModel;

namespace Pip.UI.Services;

public partial class NavigationService(Func<Type, PipViewModel> viewModelFactory)
	: ObservableObject, INavigationService
{
	[ObservableProperty] private PipViewModel? _currentView;

	public async Task NavigateToAsync<TViewModelBase>() where TViewModelBase : PipViewModel
	{
		CurrentView = viewModelFactory.Invoke(typeof(TViewModelBase));
		await CurrentView.LoadAsync();
	}
}

public interface INavigationService
{
	PipViewModel CurrentView { get; }

	Task NavigateToAsync<T>() where T : PipViewModel;
}
