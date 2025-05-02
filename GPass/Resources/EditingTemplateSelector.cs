using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPass.Resources;

public class EditingTemplateSelector : DataTemplateSelector
{
    public DataTemplate EditableTemplate { get; set; }
    public DataTemplate ReadonlyTemplate { get; set; }

    public bool IsEditing { get; set; }

    protected override DataTemplate SelectTemplateCore(object item)
    {
        return IsEditing ? EditableTemplate : ReadonlyTemplate;
    }
}