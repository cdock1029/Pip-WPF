using Microsoft.UI.Xaml.Controls;

using Pip.WinUI.ViewModels;

namespace Pip.WinUI.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
