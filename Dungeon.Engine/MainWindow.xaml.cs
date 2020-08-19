using Dungeon.Control;
using Dungeon.Engine.Menus;
using Dungeon.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace Dungeon.Engine
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<MenuItem> MenuItems { get; set; } = new ObservableCollection<MenuItem>();

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            InitializeMenu();
            BindProperties(new Drawable());
        }

        private void BindProperties(object Obj)
        {
            PropGrid.Children.Clear();
            PropGrid.RowDefinitions.Clear();

            int i = 0;

            var grayBrush = new SolidColorBrush(Color.FromRgb(128, 128, 128));

            foreach (var prop in Obj.GetType().GetProperties())
            {
                PropGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(28) });

                var label = new Label() {
                    Content = prop.Name,
                    Foreground = new SolidColorBrush(Color.FromRgb(245, 245, 245)),
                    BorderBrush= grayBrush,
                    BorderThickness=new Thickness(0, 0, 1, 1)
                };
                Grid.SetRow(label, i);
                Grid.SetColumn(label, 0);

                var editor = new Border()
                {
                    BorderBrush = grayBrush,
                    BorderThickness = new Thickness(0, 0, 0, 1)
                };
                Grid.SetRow(editor, i);
                Grid.SetColumn(editor, 1);

                var textEditor = new TextBox() { Text = prop.GetValue(Obj)?.ToString() };
                editor.Child = textEditor;

                PropGrid.Children.Add(label);
                PropGrid.Children.Add(editor);

                i++;
            }
        }

        private void InitializeMenu()
        {
            var menus = App.Container.ResolveAll<IEngineMenuItem>();
            var groups = menus.GroupBy(x => x.Tag);
            foreach (var menuRoot in groups.FirstOrDefault(g => g.Key == default))
            {
                var menuRootItem = new MenuItem()
                {
                    Header = menuRoot.Text,
                };

                var taggedGroup = groups.FirstOrDefault(g => g.Key == menuRoot.GetType().Name);
                foreach (var innerItem in taggedGroup)
                {
                    var innerMenuItem = new MenuItem() { Header = innerItem.Text };
                    innerMenuItem.Click += (s, e) => innerItem.Click?.Invoke();
                    menuRootItem.Items.Add(innerMenuItem);
                }

                MenuItems.Add(menuRootItem);
            }
        }

        private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void OnMaximizeRestoreButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            this.RefreshMaximizeRestoreButton();
        }

        private void RefreshMaximizeRestoreButton()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.maximizeButton.Visibility = Visibility.Collapsed;
                this.restoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                this.maximizeButton.Visibility = Visibility.Visible;
                this.restoreButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}
