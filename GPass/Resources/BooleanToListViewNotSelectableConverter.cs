using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;

namespace GPass.Resources;

public class BooleanToListViewNotSelectableConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        var result = (value is bool b && b);

        if (parameter != null && parameter.ToString() == "Inverted")
        {
            result = !result;
        }

        return result ? ListViewSelectionMode.None : ListViewSelectionMode.Single;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return (value is ListViewSelectionMode v && v == ListViewSelectionMode.Single);
    }
}
