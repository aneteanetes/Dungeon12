using Dungeon.Control;
using Dungeon.Engine.Events;
using Dungeon.Engine.Menus;
using Dungeon.Engine.Projects;
using Dungeon.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace Dungeon.Engine
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<MenuItem> MenuItems { get; set; } = new ObservableCollection<MenuItem>();

        public DungeonEngineProject Project { get; set; }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            InitializeMenu();

            DungeonGlobal.Events.Subscribe<PropGridFillEvent>(FillPropGrid, false);
            DungeonGlobal.Events.Subscribe<ProjectInitializeEvent>(InitializeProject, false);
            DungeonGlobal.Events.Subscribe<FreezeAllEvent>(FreezeEventHandler, false);
            DungeonGlobal.Events.Subscribe<UnfreezeAllEvent>(UnreezeEventHandler, false);
            DungeonGlobal.Events.Subscribe<StatusChangeEvent>(ChangeStatusHandler, false);
        }
        private void UnreezeEventHandler(UnfreezeAllEvent @event)
        {
            LoaderFreeze.Visibility = Visibility.Hidden;
        }

        private void FreezeEventHandler(FreezeAllEvent @event)
        {
            LoaderFreeze.Visibility = Visibility.Visible;

            if (@event.Seconds != default)
            {
                var dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Tick += (s, e) =>
                {
                    LoaderFreeze.Visibility = Visibility.Hidden;
                };
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)(@event.Seconds * 1000));
                dispatcherTimer.Start();
            }
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void InitializeProject(ProjectInitializeEvent @event)
        {
            var proj = @event.Project;


            App.Container.Reset();
            if (proj != default)
            {
                App.Container.Register<DungeonEngineProject, DungeonEngineProject>(Utils.ContainerLifeStyle.Singleton, @event.Project);
                ClearPropGrid();
                this.Project = proj;
                ChangeStatus($"Загружен проект '{Project.Name}'");
                WindowTitle.Text = $"Dungeon Engine - {Project.Name}";

                ScenesView.ItemsSource = Project.Scenes;
            }
            else
            {
                WindowTitle.Text = $"Dungeon Engine";
                ChangeStatus();
            }
        }

        private bool Available => Project != default;

        private void InitializeMenu()
        {
            var menus = App.Container.ResolveAll<IEngineMenuItem>();
            var groups = menus.GroupBy(x => x.Tag);
            foreach (var menuRoot in groups.FirstOrDefault(g => g.Key == default).OrderBy(x => x.Weight))
            {
                var menuRootItem = new MenuItem()
                {
                    Header = menuRoot.Text,
                };

                var taggedGroup = groups.FirstOrDefault(g => g.Key == menuRoot.GetType().Name);
                foreach (var innerItem in taggedGroup.OrderBy(x => x.Weight))
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

        private void AddScene(object sender, RoutedEventArgs e)
        {
            if (!Available)
                return;

            Project.Scenes.Add(new DungeonEngineScene());

            ScenesView.SelectedIndex = ScenesView.Items.Count - 1;
        }

        private void RemoveScene(object sender, RoutedEventArgs e)
        {
            Project.Scenes.Remove(ScenesView.SelectedItem as DungeonEngineScene);
            ScenesView.SelectedIndex = ScenesView.Items.Count - 1;
        }

        private void AddObject(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveObject(object sender, RoutedEventArgs e)
        {

        }

        private void ScenesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                FillPropGrid(new PropGridFillEvent(item));
            }
        }

        private void ObjectsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private object PropGridObject;
        private Dictionary<string, Func<string>> PropGridBinding;

        private void ClearPropGrid()
        {
            PropGridSaveBtn.IsEnabled = false;
            PropGrid.Children.Clear();
            PropGrid.RowDefinitions.Clear();
        }

        public void FillPropGrid(PropGridFillEvent @event)
        {
            var Obj = @event.Target;
            PropGridBinding = new Dictionary<string, Func<string>>();
            PropGridObject = Obj;

            ClearPropGrid();

            int i = 0;

            var grayBrush = new SolidColorBrush(Color.FromRgb(128, 128, 128));

            foreach (var prop in Obj.GetType().GetProperties())
            {
                PropGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(28) });

                var displ = (DisplayAttribute)Attribute
                    .GetCustomAttributes(prop)
                    .FirstOrDefault(x => x.GetType() == typeof(DisplayAttribute));

                var label = new Label()
                {
                    Content = displ?.Name ?? prop.Name,
                    Foreground = new SolidColorBrush(Color.FromRgb(245, 245, 245)),
                    BorderBrush = grayBrush,
                    BorderThickness = new Thickness(0, 0, 1, 1)
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

                if (prop.PropertyType == typeof(bool))
                {
                    var comboBoxEditor = new ComboBox();
                    comboBoxEditor.Items.Add("True");
                    comboBoxEditor.Items.Add("False");
                    comboBoxEditor.SelectedIndex = Obj.GetPropertyExpr<bool>(prop.Name) ? 0 : 1;
                    editor.Child = comboBoxEditor;
                    PropGridBinding.Add(prop.Name, () => comboBoxEditor.Text);

                    if(displ!=default)
                    comboBoxEditor.ToolTip = displ.Description;
                }
                else
                {
                    var textEditor = new TextBox() { Text = prop.GetValue(Obj)?.ToString() };
                    editor.Child = textEditor;
                    PropGridBinding.Add(prop.Name, () => textEditor.Text);

                    if (displ != default)
                        textEditor.ToolTip = displ.Description;
                }

                PropGrid.Children.Add(label);
                PropGrid.Children.Add(editor);

                if (displ != default)
                {
                    label.ToolTip = displ.Description;
                    editor.ToolTip = displ.Description;
                }


                i++;
            }

            PropGridSaveBtn.IsEnabled = true;
        }

        private void SavePropGrid(object sender, RoutedEventArgs e)
        {
            List<string> failedProps = new List<string>();

            foreach (var propBind in PropGridBinding)
            {
                if (!PropGridObject.SetPropertyExprConverted(propBind.Key, propBind.Value()))
                {
                    failedProps.Add(propBind.Key);
                }
            }

            var status = "Cохранено.";

            if (failedProps.Count != 0)
            {
                status += " Не удалось сохранить свойства: " + string.Join(" | ", failedProps);
            }

            ChangeStatus(status);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.S && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                Save();

            base.OnKeyDown(e);
        }

        private void Save()
        {
            SavePropGrid(default, default);
            Project.Save();
            ScenesView.Items.Refresh();
        }

        private void ChangeStatusHandler(StatusChangeEvent @event)
            => ChangeStatus(@event.Status);

        private void ChangeStatus(string status = default)
        {
            if (status != default)
            {
                StatusText.Text = $"{DateTime.Now:HH:mm} | {status}";
                return;
            }

            StatusText.Text = "";
        }
    }
}