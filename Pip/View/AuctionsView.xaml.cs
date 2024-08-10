using System.Windows.Controls;
using Pip.UI.ViewModel;

namespace Pip.UI.View;

public partial class AuctionsView : UserControl
{
    public AuctionsView()
    {
        InitializeComponent();
    }

    private void DataGrid_OnAutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        if (e.Column.Header.ToString() == nameof(TreasuryItemViewModel.Type)) e.Cancel = true;
    }
}
