using System.Windows.Controls;

namespace Pip.UI.View;

public partial class AuctionsView : UserControl
{
    public AuctionsView()
    {
        InitializeComponent();
        Dg.Loaded += (sender, args) =>
        {
            foreach (var column in Dg.Columns)
            {
                column.MinWidth = column.ActualWidth;
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            }
        };
    }

    private void DataGrid_OnAutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        //var dg = sender as DataGrid;
        //if (dg == null)
        //    return;
        //if (e.Column.Header.ToString() == nameof(TreasuryItemViewModel.Type)) e.Cancel = true;

        //var dgl = new DataGridLength(1.0, DataGridLengthUnitType.Star);
        //dg.ColumnWidth = dgl;
        //foreach (var oColumn in dg.Columns)
        //    oColumn.Width = dgl;
    }
}
