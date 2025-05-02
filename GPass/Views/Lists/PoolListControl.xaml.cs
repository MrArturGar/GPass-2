using GPass.Models;
using GPass.ViewModels;
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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GPass.Views.Elements
{
    public sealed partial class PoolListControl : UserControl
    {
        public PoolListControl()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(PoolListControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                nameof(SelectedItem),
                typeof(object),
                typeof(PoolListControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty AddCommandProperty =
            DependencyProperty.Register(
                nameof(AddCommand),
                typeof(ICommand),
                typeof(PoolListControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ToggleEditCommandProperty =
            DependencyProperty.Register(
                nameof(ToggleEditCommand),
                typeof(ICommand),
                typeof(PoolListControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty IsEditingProperty =
            DependencyProperty.Register(
                nameof(IsEditing),
                typeof(bool),
                typeof(PoolListControl),
                new PropertyMetadata(false));

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

        private void NameList_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            if (ItemsSource is IList<PoolList> list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Order = i;
                }
            }
        }
    }
}
