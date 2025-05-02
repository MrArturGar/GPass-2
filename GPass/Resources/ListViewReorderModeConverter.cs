using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPass.Resources;

public class ListViewReorderModeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return (value is bool b && b) ? ListViewReorderMode.Enabled : ListViewReorderMode.Disabled;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return (value is ListViewReorderMode v && v == ListViewReorderMode.Enabled);
    }
}
