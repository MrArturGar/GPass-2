using GPass.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GPass.Views.Elements
{
    public sealed partial class CredentialSetControl : UserControl
    {
        public CredentialSetControl()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(CredentialSetControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                nameof(SelectedItem),
                typeof(object),
                typeof(CredentialSetControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty AddCommandProperty =
            DependencyProperty.Register(
                nameof(AddCommand),
                typeof(ICommand),
                typeof(CredentialSetControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ToggleEditCommandProperty =
            DependencyProperty.Register(
                nameof(ToggleEditCommand),
                typeof(ICommand),
                typeof(CredentialSetControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty IsEditingProperty =
            DependencyProperty.Register(
                nameof(IsEditing),
                typeof(bool),
                typeof(CredentialSetControl),
                new PropertyMetadata(false));

        public static readonly DependencyProperty CancelEditCommandProperty =
            DependencyProperty.Register(
                nameof(CancelEditCommand),
                typeof(ICommand),
                typeof(CredentialSetControl),
                new PropertyMetadata(null));

        public ICommand AddCommand
        {
            get => (ICommand)GetValue(AddCommandProperty);
            set => SetValue(AddCommandProperty, value);
        }

        public IEnumerable? ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
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

        public ICommand CancelEditCommand
        {
            get => (ICommand)GetValue(CancelEditCommandProperty);
            set => SetValue(CancelEditCommandProperty, value);
        }

        private void ButtonAdd_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (ItemsSource != null)
            {
                ButtonPanel.Visibility = Visibility.Visible;
            }
        }

        private void ButtonAdd_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ButtonPanel.Visibility = Visibility.Collapsed;
        }
    }
}

