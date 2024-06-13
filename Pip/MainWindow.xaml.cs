using System.Windows;
using Pip.Data;
using Pip.ViewModel;

namespace Pip;

public partial class MainWindow : Window
{
    private readonly TreasuriesViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new TreasuriesViewModel(new TreasuryDataProvider());
        DataContext = _viewModel;
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        await _viewModel.LoadAsync();
    }
}