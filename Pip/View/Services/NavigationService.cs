using CommunityToolkit.Mvvm.ComponentModel;
using Pip.UI.ViewModel;

namespace Pip.UI.View.Services;

public partial class NavigationService(Func<Type, ViewModelBase> viewModelFactory)
    : ObservableObject, INavigationService
{
    [ObservableProperty] private ViewModelBase? _currentView;

    public void NavigateTo<TViewModelBase>() where TViewModelBase : ViewModelBase
    {
        var viewModel = viewModelFactory.Invoke(typeof(TViewModelBase));
        CurrentView = viewModel;
    }
}

public interface INavigationService
{
    ViewModelBase CurrentView { get; }

    void NavigateTo<T>() where T : ViewModelBase;
}
