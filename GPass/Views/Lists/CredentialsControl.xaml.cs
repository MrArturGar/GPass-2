using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Collections;
using GPass.Models;
using System.Windows.Input;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GPass.Views.Elements
{
    public sealed partial class CredentialsControl : UserControl
    {
        public CredentialsControl()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable<ICredential>),
                typeof(CredentialsControl),
                new PropertyMetadata(null));


        public static readonly DependencyProperty AddCommandProperty =
            DependencyProperty.Register(
                nameof(AddCommand),
                typeof(ICommand),
                typeof(CredentialsControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ToggleEditCommandProperty =
            DependencyProperty.Register(
                nameof(ToggleEditCommand),
                typeof(ICommand),
                typeof(CredentialsControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty IsEditingProperty =
            DependencyProperty.Register(
                nameof(IsEditing),
                typeof(bool),
                typeof(CredentialsControl),
                new PropertyMetadata(false));

        public ICommand AddCommand
        {
            get => (ICommand)GetValue(AddCommandProperty);
            set => SetValue(AddCommandProperty, value);
        }

        public IEnumerable<ICredential>? ItemsSource
        {
            get => (IEnumerable<ICredential>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public ICommand ToggleEditCommand
        {
            get => (ICommand)GetValue(ToggleEditCommandProperty);
            set => SetValue(ToggleEditCommandProperty, value);
        }

        public bool IsEditing
        {
            get => (bool)GetValue(IsEditingProperty);
            set => SetValue(IsEditingProperty, value);
        }

        private void ButtonAdd_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (ItemsSource != null)
            {
                ButtonAdd.Visibility = Visibility.Visible;
                ButtonEdit.Visibility = Visibility.Visible;
            }
        }

        private void ButtonAdd_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ButtonAdd.Visibility = Visibility.Collapsed;
            ButtonEdit.Visibility = Visibility.Collapsed;
        }
    }
}
