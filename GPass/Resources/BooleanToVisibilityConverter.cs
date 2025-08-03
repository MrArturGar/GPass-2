using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using System;

namespace GPass.Resources;

public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var result = (value is bool b && b);

        if (parameter != null && parameter.ToString() == "Inverted")
        {
            result = !result;
        }

        return result ? Visibility.Visible : Visibility.Collapsed; ;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return (value is Visibility v && v == Visibility.Visible);
    }
}