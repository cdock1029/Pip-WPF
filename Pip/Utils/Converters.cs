using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Binding = System.Windows.Data.Binding;

namespace Pip.UI.Utils;

public class BoolToWidthConverter : MarkupExtension, IValueConverter
{
    public double StartupWidth { get; set; }
    private bool _isExpanded = true;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        _isExpanded = value is not null && (bool)value;
        return _isExpanded ? new GridLength(StartupWidth) : GridLength.Auto;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (_isExpanded && value is not null) StartupWidth = ((GridLength)value).Value;
        return Binding.DoNothing;
    }
}