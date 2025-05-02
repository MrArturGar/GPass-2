using GPass.Models;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GPass.Resources
{
    public class CredentialTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CredTitleTemplate { get; set; }
        public DataTemplate CredSecretFieldTemplate { get; set; }
        public DataTemplate CredFieldTemplate { get; set; }
        public DataTemplate CredLineTemplate { get; set; }
        public DataTemplate DefaultTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return item switch
            {
                CredTitle => CredTitleTemplate,
                CredSecretField => CredSecretFieldTemplate,
                CredField => CredFieldTemplate,
                CredLine => CredLineTemplate,
                _ => DefaultTemplate
            };
        }
    }
}
