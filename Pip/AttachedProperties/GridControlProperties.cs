using System.Windows;
using System.Windows.Input;

namespace Pip.UI.AttachedProperties
{
    public static class GridControlProperties
    {
        public static readonly DependencyProperty RowCellMenuCommandProperty =
            DependencyProperty.RegisterAttached(
                "RowCellMenuCommand",
                typeof(ICommand),
                typeof(GridControlProperties),
                new PropertyMetadata(null));

        public static ICommand GetRowCellMenuCommand(UIElement target)
        {
            return (ICommand)target.GetValue(RowCellMenuCommandProperty);
        }

        public static void SetRowCellMenuCommand(UIElement target, ICommand value)
        {
            target.SetValue(RowCellMenuCommandProperty, value);
        }
    }
}
