using Dungeon.Control;
using Dungeon.Engine.Controls;
using Dungeon.Engine.Editable;
using Dungeon.Engine.Editable.PropertyTable;
using Dungeon.Engine.Engine;
using Dungeon.Engine.Events;
using Dungeon.Engine.Forms;
using Dungeon.Engine.Host;
using Dungeon.Engine.Menus;
using Dungeon.Engine.Projects;
using Dungeon.Engine.Utils;
using Dungeon.Entities;
using Dungeon.Resources;
using Dungeon.Scenes.Manager;
using Dungeon.Utils;
using Dungeon.Utils.ReflectionExtensions;
using Dungeon.View.Interfaces;
using LiteDB;
using MathNet.Numerics.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace Dungeon.Engine
{
    class TreeViewLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TreeViewItem item = (TreeViewItem)value;
            ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(item);
            return ic.ItemContainerGenerator.IndexFromContainer(item) == ic.Items.Count - 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }

    public partial class MainWindow : Window
    {
        public ObservableCollection<MenuItem> MenuItems { get; set; } = new ObservableCollection<MenuItem>();

        public DungeonEngineProject Project { get; set; }

        public DungeonEngineScene SelectedScene { get; set; }

        public SceneManager SceneManager { get; set; }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            InitializeMenu();

            DungeonGlobal.Events.Subscribe<ProjectInitializeEvent>(InitializeProject, false);
            DungeonGlobal.Events.Subscribe<PropGridFillEvent>(FillPropGrid, false);
            DungeonGlobal.Events.Subscribe<FreezeAllEvent>(FreezeEventHandler, false);
            DungeonGlobal.Events.Subscribe<UnfreezeAllEvent>(UnreezeEventHandler, false);
            DungeonGlobal.Events.Subscribe<StatusChangeEvent>(ChangeStatusHandler, false);
            DungeonGlobal.Events.Subscribe<ResourceAddEvent>(AddedNewResourceEvent, false);
            DungeonGlobal.Events.Subscribe<AddSceneObjectEvent>(AddedNewSceneObject, false);
            DungeonGlobal.Events.Subscribe<SceneObjectInObjectTreeSelectedEvent>(SelectedSceneObjectEvent, false);
            DungeonGlobal.Events.Subscribe<SceneResolutionChangedEvent>(ChangedResolutionEvent, false);
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

        /// <summary>
        /// Главный метод
        /// </summary>
        /// <param name="event"></param>
        private void InitializeProject(ProjectInitializeEvent @event)
        {
            var proj = @event.Project;

            AddSceneBtn.IsEnabled = RemoveSceneBtn.IsEnabled = proj != default;

            App.Container.Reset();
            if (proj != default)
            {
                App.Container.Register<DungeonEngineProject, DungeonEngineProject>(Utils.ContainerLifeStyle.Singleton, @event.Project);
                ClearPropGrid();
                this.Project = proj;
                ChangeStatus($"Загружен проект '{Project.Name}'");
                WindowTitle.Text = $"Dungeon Engine - {Project.Name}";

                ScenesView.ItemsSource = Project.Scenes;

                if (Project.Resources == default)
                {
                    Project.Resources = new ObservableCollection<DungeonEngineResourcesGraph>
                    {
                        new DungeonEngineResourcesGraph()
                        {
                            Nodes = new ObservableCollection<DungeonEngineResourcesGraph>()
                            {
                                new DungeonEngineResourcesGraph(){ Path="./DungeonEngine.meta"}
                            }
                        }
                    };
                }

                ResourcesView.ItemsSource = Project.Resources;

                Project.Load();

                ResourceLoader.ResourceDatabaseResolver = new DungeonEngineResourceDatabaseResolver(Project.DbFilePath);

                SceneManager = new SceneManager()
                {
                    DrawClient = XnaHost
                };
                DungeonGlobal.SceneManager = SceneManager;

                SceneManager.Start();
                SceneManager.Change<EasyScene>();
            }
            else
            {
                if (Project != default)
                {
                    Project.Scenes.Clear();
                    Project.Resources.Clear();
                }

                SelectedScene = default;
                ObjectsView.ItemsSource = default;
                AddObjectBtn.IsEnabled = RemoveObjectBtn.IsEnabled = false;

                Project = default;
                ClearPropGrid();
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

            Project.Scenes.Add(new DungeonEngineScene() { Name = "Scene", Width = Project.CompileSettings.WidthPixel, Height = Project.CompileSettings.HeightPixel });

            ScenesView.SelectedIndex = ScenesView.Items.Count - 1;
        }

        private void RemoveScene(object sender, RoutedEventArgs e)
        {
            if (ScenesView.SelectedItem is DungeonEngineScene des)
            {
                if (des.StartScene)
                {
                    MessageBox.Show("Невозможно удалить Стартовую сцену");
                    return;
                }

                Project.Scenes.Remove(des);
                ScenesView.SelectedIndex = ScenesView.Items.Count - 1;
            }
        }

        private void AddObject(object sender, RoutedEventArgs e)
        {
            new AddSceneObjectForm(default).Show();
        }

        private void RemoveObject(object sender, RoutedEventArgs e)
        {
            var cmp = ObjectsView.TreeView;
            if (cmp.SelectedItem is DungeonEngineSceneObject obj)
            {
                SelectedScene.SceneObjects.Remove(obj);
                if(obj.Instance!=default)
                {
                    SceneManager.Current.RemoveObject(obj.Instance.As<ISceneObject>());
                }
            }
        }

        /// <summary>
        /// Выбор сцены
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScenesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                SelectedScene = (DungeonEngineScene)item;
                SelectScene((DungeonEngineScene)item);


                SceneManager.Change<EasyScene>();
                foreach (var obj in SelectedScene.SceneObjects)
                {
                    PushSceneObjectToScene(obj);
                }

                return;
            }
            AddObjectBtn.IsEnabled = RemoveObjectBtn.IsEnabled = false;

        }

        private void PublishSceneObject_Click(object sender, RoutedEventArgs e)
        {
            PushSceneObjectToScene(SelectedSceneObject);
        }

        private void PushSceneObjectToScene(DungeonEngineSceneObject obj)
        {
            ISceneObject instance = default;

            var ctors = obj.Properties.Where(x => x.Name.Contains("Constructor")).ToList();
            if (ctors.Count == 0)
            {
                instance = obj.ClassType.NewAs<ISceneObject>();
            }
            else
            {
                var activeCtor = ctors.FirstOrDefault(row => row.Value.As<bool>() == true);
                if (activeCtor == default)
                {
                    PublishSceneObject.Visibility = Visibility.Visible;
                    ChangeStatus("Для текущего объекта не выбран используемый конструктор!");
                    return;
                }

                var activeCtorIndex = int.Parse(activeCtor.Name.Replace("Constructor ", ""));
                var activeCtorInstance = obj.ClassType.GetConstructors().ElementAtOrDefault(activeCtorIndex);

                if (activeCtorInstance != default)
                {
                    var @params = activeCtorInstance.GetParameters()
                        .Select(param => obj.Properties.FirstOrDefault(p => p.Name == param.Name).Value)
                        .ToArray();

                    instance = obj.ClassType.New<object>(activeCtorInstance, @params).As<ISceneObject>();
                }
            }

            var props = obj.Properties.Except(ctors);
            foreach (var p in props)
            {
                instance.SetPropertyExprType(p.Name, p.Value, p.Type);
            }
            obj.Instance = instance;
            SceneManager.Current.AddObject(instance);
            if(PublishSceneObject.Visibility== Visibility.Visible)
            {
                PublishSceneObject.Visibility = Visibility.Hidden;
            }
        }

        private void SelectScene(DungeonEngineScene item)
        {
            ObjectsView.ItemsSource = SelectedScene.SceneObjects;
            FillPropGrid(new PropGridFillEvent(item));
            XnaHost.Width = SelectedScene.Width;
            XnaHost.Height = SelectedScene.Height;
            SceneResolution.Content = $"{XnaHost.Width}x{XnaHost.Height}";
            AddObjectBtn.IsEnabled = RemoveObjectBtn.IsEnabled = true;
        }

        private object PropGridObject;
        private IPropertyTable PropGridTable;
        private Dictionary<string, Func<string>> PropGridBinding;

        private void ClearPropGrid()
        {
            PropGridSaveBtn.IsEnabled = false;
            PropGrid.Children.Clear();
            PropGrid.RowDefinitions.Clear();
            PropGridObject = default;
            PropGridTable = default;
        }


        private static SolidColorBrush grayBrush = new SolidColorBrush(Color.FromRgb(128, 128, 128));
        private static SolidColorBrush darkGrayBrush = new SolidColorBrush(Color.FromRgb(51, 51, 55));

        public void FillPropGrid(PropGridFillEvent @event)
        {
            var Obj = @event.Target;

            ClearPropGrid();
            PropGridBinding = new Dictionary<string, Func<string>>();
            PropGridObject = Obj;

            int i = 0;
            CreatePropGridCategory("Основные", true, i++);

            var grayBrush = new SolidColorBrush(Color.FromRgb(128, 128, 128));

            foreach (var prop in Obj.GetType().GetProperties())
            {
                var hidden = Attribute.GetCustomAttributes(prop)
                    .FirstOrDefault(x => x.GetType() == typeof(HiddenAttribute)) != default;
                if (hidden)
                    continue;

                if (!prop.CanWrite)
                    continue;

                var displ = Attribute.GetCustomAttributes(prop)
                    .FirstOrDefault(x => x.GetType() == typeof(DisplayAttribute)).As<DisplayAttribute>();

                CreatePropGridCell(prop.Name, prop.GetValue(Obj), prop.PropertyType, i, default, displ?.Name, displ?.Description);

                i++;
            }

            PropGridSaveBtn.IsEnabled = true;
        }

        public void FillPropGrid(IPropertyTable propertyTable)
        {
            PropGridBinding = new Dictionary<string, Func<string>>();

            ClearPropGrid();
            PropGridTable = propertyTable;

            int i = 0;

            CreatePropGridCategory("Основные", true, i++);

            foreach (var prop in propertyTable.Properties)
            {
                if (prop.Name.Contains("Constructor"))
                {
                    CreatePropGridCategory("Конструктор", true, i++);
                    CreatePropGridCell("Использовать", false, typeof(bool), i, prop.Name);
                }
                else
                {
                    CreatePropGridCell(prop.Name, prop.Value, prop.Type, i);
                }
                i++;
            }

            PropGridSaveBtn.IsEnabled = true;
        }

        private void CreatePropGridCategory(string name, bool first, int rowNum)
        {
            PropGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(28) });

            var label = new Label()
            {
                Foreground = new SolidColorBrush(Color.FromRgb(245, 245, 245)),
                BorderBrush = grayBrush,
                BorderThickness = new Thickness(0, (first ? 0 : 1), 1, 1),
                FontWeight = FontWeights.Bold
            };

            var textBlock = new TextBlock()
            {
                Text = name,
                Margin = new Thickness(10, 0, 0, 0)
            };

            label.Content = textBlock;

            Grid.SetRow(label, rowNum);
            Grid.SetColumn(label, 0);
            Grid.SetColumnSpan(label, 2);

            PropGrid.Children.Add(label);
        }

        private void CreatePropGridCell(string name, object value, Type type, int rowNum, string nameBinding = default, string display = default, string description = default)
        {
            PropGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(28) });

            var label = new Label()
            {
                Content = display ?? name,
                Foreground = new SolidColorBrush(Color.FromRgb(245, 245, 245)),
                BorderBrush = grayBrush,
                Background = darkGrayBrush,
                BorderThickness = new Thickness(1, 0, 1, 1),
                Margin = new Thickness(10, 0, 0, 0)
            };
            Grid.SetRow(label, rowNum);
            Grid.SetColumn(label, 0);

            if (description != default)
            {
                label.ToolTip = description;
            }

            var editor = new Border()
            {
                BorderBrush = grayBrush,
                BorderThickness = new Thickness(0, 0, 0, 1)
            };
            Grid.SetRow(editor, rowNum);
            Grid.SetColumn(editor, 1);

            if (type == typeof(bool))
            {
                var comboBoxEditor = new ComboBox();
                comboBoxEditor.Items.Add("True");
                comboBoxEditor.Items.Add("False");
                comboBoxEditor.SelectedIndex = value.As<bool>() ? 0 : 1;
                editor.Child = comboBoxEditor;
                PropGridBinding.Add(nameBinding ?? name, () => comboBoxEditor.Text);

                if (description != default)
                {
                    comboBoxEditor.ToolTip = description;
                }
            }
            else
            {
                var textEditor = new TextBox() { Text = value?.ToString() ?? "" };
                editor.Child = textEditor;
                PropGridBinding.Add(nameBinding ?? name, () => textEditor.Text);

                if (description != default)
                {
                    textEditor.ToolTip = description;
                }
            }

            PropGrid.Children.Add(label);
            PropGrid.Children.Add(editor);
        }

        private void SavePropGrid(object sender, RoutedEventArgs e)
        {
            if (PropGridObject == default && PropGridTable == default)
                return;

            List<string> failedProps = new List<string>();

            foreach (var propBind in PropGridBinding)
            {
                if (PropGridObject != default)
                {
                    if (!PropGridObject.SetPropertyExprConverted(propBind.Key, propBind.Value()))
                    {
                        failedProps.Add(propBind.Key);
                    }
                }
                else
                {
                    var type = PropGridTable.Get(propBind.Key).Type;
                    if (type == default)
                        continue;
                    object data = default;
                    try
                    {
                        data = Convert.ChangeType(propBind.Value(), type);
                    }
                    catch (Exception)
                    {
                        data = type.GetDefault();
                        failedProps.Add(propBind.Key);
                    }

                    PropGridTable.Set(propBind.Key, data, type);
                }
            }

            var status = "Cохранено.";

            if (failedProps.Count != 0)
            {
                status += " Не удалось сохранить свойства: " + string.Join(" | ", failedProps);
            }

            if (PropGridObject is IEditable editable)
            {
                editable.Commit();
            }
            if (PropGridTable != default)
            {
                PropGridTable.Commit();
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

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void AddedNewResourceEvent(ResourceAddEvent @event)
        {
            @event.ParentResource.Nodes.Add(@event.Resource);
            new LiteDatabase(Project.DbFilePath).GetCollection<Resource>()
                .Insert(new Resource()
                {
                    Path = Path.GetFileName(@event.Resource.Path),
                    Data = File.ReadAllBytes(@event.Resource.Path)
                });
            Save();
        }

        private void AddedNewSceneObject(AddSceneObjectEvent @event)
        {
            if (@event.Root)
            {
                this.SelectedScene.SceneObjects.Add(@event.SceneObject);
            }

            PushSceneObjectToScene(@event.SceneObject);
        }

        private DungeonEngineSceneObject SelectedSceneObject;

        private void SelectedSceneObjectEvent(SceneObjectInObjectTreeSelectedEvent @event)
        {
            if (@event.SceneObject != default)
            {
                FillPropGrid(@event.SceneObject);
            }
            SelectedSceneObject = @event.SceneObject;
        }

        private void AddResourceCtxClick(object sender, RoutedEventArgs e)
        {
            if (ResourcesView.SelectedItem is DungeonEngineResourcesGraph resGraph)
            {
                new AddResourceForm(resGraph).Show();
            }
        }

        private void ClearPropGridManually(object sender, RoutedEventArgs e)
        {
            ClearPropGrid();
        }

        private void SceneDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectScene(ScenesView.SelectedItem.As<DungeonEngineScene>());
        }

        private void ChangedResolutionEvent(SceneResolutionChangedEvent @event)
        {
            var resolution = @event.Size;
            XnaHost.Width = resolution.X;
            XnaHost.Height = resolution.Y;
        }
    }
}