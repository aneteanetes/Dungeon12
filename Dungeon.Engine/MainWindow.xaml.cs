using Dungeon.Engine.Editable;
using Dungeon.Engine.Editable.PropertyTable;
using Dungeon.Engine.Engine;
using Dungeon.Engine.Events;
using Dungeon.Engine.Forms;
using Dungeon.Engine.Host;
using Dungeon.Engine.Menus;
using Dungeon.Engine.Projects;
using Dungeon.Resources;
using Dungeon.Scenes.Manager;
using Dungeon.Types;
using Dungeon.Utils;
using Dungeon.Utils.ReflectionExtensions;
using Dungeon.View.Interfaces;
using LiteDB;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

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

            DungeonGlobal.AudioPlayer = new AudioPlayerImpl();

            DungeonGlobal.Events.Subscribe<ProjectInitializeEvent>(InitializeProject, false);
            DungeonGlobal.Events.Subscribe<PropGridFillEvent>(FillPropGrid, false);
            DungeonGlobal.Events.Subscribe<FreezeAllEvent>(FreezeEventHandler, false);
            DungeonGlobal.Events.Subscribe<UnfreezeAllEvent>(UnreezeEventHandler, false);
            DungeonGlobal.Events.Subscribe<StatusChangeEvent>(ChangeStatusHandler, false);
            DungeonGlobal.Events.Subscribe<ResourceAddEvent>(AddedNewResourceEvent, false);
            DungeonGlobal.Events.Subscribe<AddSceneObjectEvent>(AddedNewSceneObject, false);
            DungeonGlobal.Events.Subscribe<SceneObjectInObjectTreeSelectedEvent>(SelectedSceneObjectEvent, false);
            DungeonGlobal.Events.Subscribe<SceneResolutionChangedEvent>(ChangedResolutionEvent, false);
            DungeonGlobal.Events.Subscribe<RemoveSceneObjectFromSceneEvent>(RemovingSceneObjectFromSceneEvent,false);

            this.KeyDown += D3D11Host.OnKeyDown;
            this.KeyUp += D3D11Host.OnKeyUp;
            XnaHost.MouseDown += XnaHost_MouseDown;
            this.MouseUp += XnaHost_MouseUp;
            this.MouseMove += XnaHost_MouseMove;
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
                        new DungeonEngineResourcesGraph() //root
                    };
                    var meta = new DungeonEngineResourcesGraph() { Name = "DungeonEngine.meta", Type = DungeonEngineResourceType.Embedded };
                    var root = Project.Resources.FirstOrDefault();
                    meta.Parent = root;
                    root.Nodes.Add(meta);
                }

                ResourcesView.ItemsSource = Project.Resources;

                Project.Load();
                this.XnaHost.ChangeCell(Project.CompileSettings.CellSize);

                ResourceLoader.Settings = new ResourceLoaderSettings()
                {
                    ThrowIfNotFound = false,
                    NotFoundAction = r => ChangeStatus("Не найден ресурс: "+r)
                };
                ResourceLoader.ResourceDatabaseResolvers.Add(new DungeonEngineResourceDatabaseResolver(Project.DbFilePath));
                ResourceLoader.ResourceResolvers.Add(new EmbeddedResourceResolver());

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
                    Project.Close();
                }

                SelectedScene = default;
                ObjectsView.ItemsSource = default;
                AddObjectBtn.IsEnabled = RemoveObjectBtn.IsEnabled = false;

                Project = default;
                ClearPropGrid();
                WindowTitle.Text = $"Dungeon Engine";
                if (SceneManager != default)
                {
                    SceneManager.Change<EasyScene>();
                    ChangeStatus();
                }
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
                    Message.Show("Невозможно удалить Стартовую сцену");
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
                RemovingSceneObjectFromSceneEvent(new RemoveSceneObjectFromSceneEvent(obj));
            }
        }

        private void RemovingSceneObjectFromSceneEvent(RemoveSceneObjectFromSceneEvent @event)
        {
            var obj = @event.RootedObject;
            if (obj.Parent == default)
            {
                SelectedScene.SceneObjects.Remove(obj);
                if (obj.Instance != default)
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
            bool notYet = SelectedSceneObject.Published;
            PushSceneObjectToScene(SelectedSceneObject, !SelectedSceneObject.Published);
            if (!notYet && SelectedSceneObject.Published)
            {
                FillPropGrid(SelectedSceneObject, SelectedSceneObject.GetType().Name);
            }
        }

        private void PushSceneObjectToScene(DungeonEngineSceneObject obj, bool initial=false, DungeonEngineSceneObject parent=default)
        {
            try
            {
                ISceneObject instance = default;

                var properties = obj.Properties.ToList();

                var ctors = properties.Where(x => x.Name.Contains("Constructor")).ToList();
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

                    //всё что относится к конструктору мы не должны пытаться установить
                    //var firstCtorProp = properties.FirstOrDefault(x => x.Name.ToLowerInvariant().Contains("Constructor"));
                    //ctors.AddRange(properties.Skip(properties.IndexOf(firstCtorProp) + 1));
                    //ниже идёт Except(ctors)

                    var activeCtorIndex = int.Parse(activeCtor.Name.Replace("Constructor ", ""));
                    var activeCtorInstance = obj.ClassType.GetConstructors().ElementAtOrDefault(activeCtorIndex);

                    if (activeCtorInstance != default)
                    {
                        var @params = activeCtorInstance.GetParameters()
                            .Select(param => properties.FirstOrDefault(p => p.Name == param.Name).Value)
                            .ToArray();

                        instance = obj.ClassType.New<object>(activeCtorInstance, @params).As<ISceneObject>();
                    }
                }

                var props = properties.Except(ctors);
                foreach (var p in props)
                {
                    try
                    {
                        var nowValue = instance.GetPropertyExprRaw(p.Name);
                        if (initial && nowValue != default)
                        {
                            if (string.IsNullOrWhiteSpace(p.Value?.ToString()))
                            {
                                obj.Set(p.Name, nowValue, nowValue.GetType());
                            }
                            continue;
                        }
                        instance.SetPropertyExprType(p.Name, p.Value, p.Type);
                    }
                    catch
                    {
                        //ну кароч, свойства из конструктора пытаются установиться. 
                    }
                }

                obj.Instance = instance;

                if (obj.Nodes?.Count > 0)
                {
                    foreach (var n in obj.Nodes)
                    {
                        PushSceneObjectToScene(n, initial, obj);
                    }
                }

                if (parent != default)
                {
                    parent.Instance.AddChild(instance);
                    return;
                }

                SceneManager.Current.AddObject(instance);
                obj.Published=true;
                if (PublishSceneObject.Visibility == Visibility.Visible)
                {
                    PublishSceneObject.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                ChangeStatus($"Ошибка публикации: {ex}");
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
        private List<(string Key, Func<string> Value, int index)> PropGridBinding;

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
            PropGridBinding = new List<(string Key, Func<string> Value, int index)>();
            PropGridObject = Obj;

            int i = 0;
            CreatePropGridCategory(Obj.GetType().Name, true, i++);

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

        public void FillPropGrid(IPropertyTable propertyTable, string commonTitle="Основные")
        {
            PropGridBinding = new List<(string Key, Func<string> Value, int index)>();

            ClearPropGrid();
            PropGridTable = propertyTable;

            int i = 0;

            CreatePropGridCategory(commonTitle, true, i++);

            foreach (var prop in propertyTable.Properties)
            {
                if (prop.Name.Contains("Constructor"))
                {
                    CreatePropGridCategory("Конструктор", true, i++);
                    CreatePropGridCell("Использовать", prop.Value, typeof(bool), i, prop.Name);
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
                PropGridBinding.Add((nameBinding ?? name, () => comboBoxEditor.Text, rowNum-1));

                if (description != default)
                {
                    comboBoxEditor.ToolTip = description;
                }
            }
            else
            {
                var textEditor = new TextBox() { Text = value?.ToString() ?? "" };
                editor.Child = textEditor;
                PropGridBinding.Add((nameBinding ?? name, () => textEditor.Text, rowNum - 1));

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

                    PropGridTable.Set(propBind.Key, data, type, PropGridBinding.IndexOf(propBind));
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
            this.XnaHost.ChangeCell(Project.CompileSettings.CellSize);
            ScenesView.Items.Refresh();
        }

        private void ChangeStatusHandler(StatusChangeEvent @event)
            => ChangeStatus(@event.Status);

        public void ChangeStatus(string status = default, bool error=false)
        {
            if (status != default)
            {
                if(error)
                {
                    StatusBar.Background = Brushes.Red;
                }
                else
                {
                    StatusBar.Background = new SolidColorBrush(Color.FromRgb(0, 122, 204));
                }

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
            var newRes = new DungeonEngineResourcesGraph
            {
                Type = DungeonEngineResourceType.File,
                Parent = @event.ParentResource
            };
            @event.ParentResource.Nodes.Add(newRes);
            newRes.Name = Path.GetFileName(@event.ResourceFilePath);

            new LiteDatabase(Project.DbFilePath).GetCollection<Resource>()
                .Insert(new Resource()
                {
                    Path = newRes.GetFullPath(),
                    Data = File.ReadAllBytes(@event.ResourceFilePath)
                });

            Save();
        }

        private void AddedNewSceneObject(AddSceneObjectEvent @event)
        {
            if (@event.Root)
            {
                this.SelectedScene.SceneObjects.Add(@event.SceneObject);
            }

            PushSceneObjectToScene(@event.SceneObject,true);
        }

        private DungeonEngineSceneObject SelectedSceneObject;

        private void SelectedSceneObjectEvent(SceneObjectInObjectTreeSelectedEvent @event)
        {
            if (@event.SceneObject != default)
            {
                if (!@event.SceneObject.Published)
                {
                    PublishSceneObject.Visibility = Visibility.Visible;
                }

                FillPropGrid(@event.SceneObject, @event.SceneObject.ClassName);
            }
            SelectedSceneObject = @event.SceneObject;
        }

        private void AddResourceCtxClick(object sender, RoutedEventArgs e)
        {
            if (ResourcesView.SelectedItem is DungeonEngineResourcesGraph resGraph)
            {
                if (resGraph.Type == DungeonEngineResourceType.Folder || resGraph.Display == "Resources")
                {
                    new AddResourceForm(resGraph).Show();
                }
                else
                {
                    ChangeStatus("Ресурс можно добавить только в папку или корень проекта!");
                }
            }
        }

        private void RemoveResourceCtxClick(object sender, RoutedEventArgs e)
        {
            if (ResourcesView.SelectedItem is DungeonEngineResourcesGraph resGraph)
            {
                if (resGraph.Type == DungeonEngineResourceType.Embedded || resGraph.Display == "Resources")
                    return;

                if (RemoveResImpl(resGraph))
                {
                    resGraph.Parent.Nodes.Remove(resGraph);
                }
                else
                {
                    ChangeStatus($"Не удалось удалить ресурс: {resGraph.GetFullPath()}");
                }
            }
        }

        private bool RemoveResImpl(DungeonEngineResourcesGraph resGraph)
        {
            if (resGraph.Type != DungeonEngineResourceType.Folder)
            {
                var db = new LiteDatabase(Project.DbFilePath);
                var store = db.GetCollection<Resource>();
                var stored = store.FindOne(r => r.Path == resGraph.GetFullPath());
                if (stored != default)
                {
                    return store.Delete(stored.Id);
                }
                return false;
            }
            else
            {
                List<DungeonEngineResourcesGraph> remove = new List<DungeonEngineResourcesGraph>();

                foreach (var resInside in resGraph.Nodes)
                {
                    RemoveResImpl(resInside);
                    remove.Add(resInside);
                }

                remove.ForEach(r => resGraph.Nodes.Remove(r));
                return true;
            }
        }

        private void RenameResourceCtxClick(object sender, RoutedEventArgs e)
        {
            if (ResourcesView.SelectedItem is DungeonEngineResourcesGraph resGraph)
            {
                var nmform = new AddNamedForm("Переименовать ресурс");
                nmform.ShowDialog();

                using var db = new LiteDatabase(Project.DbFilePath);
                var store = db.GetCollection<Resource>();

                var fullPath = resGraph.GetFullPath();
                var stored = store.FindOne(r => r.Path == fullPath);

                if (stored != default)
                {
                    resGraph.Name = nmform.Text;
                    stored.Path = resGraph.GetFullPath();
                    store.Update(stored);
                }
                else
                {
                    ChangeStatus($"Неудалось переименовать ресурс: {resGraph.Display}!", error: true);
                }
            }
        }

        private void AddResourceFolderCtxClick(object sender, RoutedEventArgs e)
        {
            if (ResourcesView.SelectedItem is DungeonEngineResourcesGraph resGraph)
            {
                if (resGraph.Type == DungeonEngineResourceType.Folder || resGraph.Display == "Resources")
                {
                    var nmform = new AddNamedForm("Добавить папку");
                    nmform.ShowDialog();
                    resGraph.Nodes.Add(new DungeonEngineResourcesGraph()
                    {
                        Name = nmform.Text,
                        Type = DungeonEngineResourceType.Folder,
                        Parent = resGraph
                    });
                }
            }
        }

        private void ClearPropGridManually(object sender, RoutedEventArgs e)
        {
            ClearPropGrid();
        }

        private void SceneDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedScene == default)
                return;
            SelectScene(ScenesView.SelectedItem.As<DungeonEngineScene>());
        }

        private void ChangedResolutionEvent(SceneResolutionChangedEvent @event)
        {
            var resolution = @event.Size;
            XnaHost.Width = resolution.X;
            XnaHost.Height = resolution.Y;
        }

        private void ExpandStatusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(StatusText.Text))
            {
                Message.Show(StatusText.Text);
            }
        }

        private void XnaHost_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!moveTool)
                return;

            if (e.ChangedButton == MouseButton.Left)
            {
                moveMode = false;
            }
        }

        private void XnaHost_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedSceneObject == default || !moveTool)
                return;

            if (e.ChangedButton == MouseButton.Left)
            {
                var posWPF = Mouse.GetPosition(XnaHost);

                var mouse = new Types.Rectangle(posWPF.X, posWPF.Y, 1, 1);
                var obj = new Rectangle(
                    SelectedSceneObject.Instance.ComputedPosition.X,
                    SelectedSceneObject.Instance.ComputedPosition.Y,
                    SelectedSceneObject.Instance.Width,
                    SelectedSceneObject.Instance.Height);

                if (obj.Width == 0 && obj.Height == 0 && !string.IsNullOrWhiteSpace(SelectedSceneObject.Instance.Image))
                {
                    var size = DungeonGlobal.DrawClient.MeasureImage(SelectedSceneObject.Instance.Image);
                    obj.Width = size.X;
                    obj.Height = size.Y;
                }

                if (obj.IntersectsWithOrContains(mouse))
                {
                    moveMode = true;
                    prev = new Types.Point(mouse.X, mouse.Y);
                }
            }
        }
        bool moveTool = false;
        bool moveMode = false;

        Types.Point prev;

        private void XnaHost_MouseMove(object sender, MouseEventArgs e)
        {            
            if (!moveMode || !moveTool || SelectedSceneObject==default)
                return;

            void set(bool? plus=null, double x =0, double y =0, bool plusX=false, bool plusY=true)
            {
                var X = SelectedSceneObject.Get("Left").Value.As<double>();
                var Y = SelectedSceneObject.Get("Top").Value.As<double>();

                if (x != default)
                {
                    if (plus != default)
                    {
                        X += x * (plus.Value ? 1 : -1);
                    }
                    else
                    {
                        X += x * (plusX ? 1 : -1);
                    }

                    SelectedSceneObject.Set("Left", X, typeof(double));
                    SelectedSceneObject.Instance.Left = X;
                }
                if (y != default)
                {
                    if (plus != default)
                    {
                        Y += y * (plus.Value ? 1 : -1);
                    }
                    else
                    {
                        Y += y * (plusY ? 1 : -1);
                    }
                    SelectedSceneObject.Set("Top", Y, typeof(double));
                    SelectedSceneObject.Instance.Top = Y;
                }
            }

            var posWPF = Mouse.GetPosition(XnaHost);
            var pos = new Types.Point(posWPF.X, posWPF.Y);

            var valX = Math.Abs(pos.X - prev.X);
            var valY = Math.Abs(pos.Y - prev.Y);

            //valX += SelectedSceneObject.Instance.Left - pos.X;
            //valY += SelectedSceneObject.Instance.Top - pos.Y;

            switch (prev.DetectDirection(pos, 0))
            {
                case Direction.Up: set(false, y: valY); break;
                case Direction.Down: set(true, y: valY); break;
                case Direction.Left: set(false, x: valX); break;
                case Direction.Right: set(true, x: valX); break;
                case Direction.UpLeft: set(x: valX, y: valY, plus: false); break;
                case Direction.UpRight: set(x: valX, y: valY, plusY: false, plusX: true); break;
                case Direction.DownLeft: set(x: valX, y: valY, plusY: true, plusX: false); break;
                case Direction.DownRight: set(x: valX, y: valY, plus: true); break;
                default: break;
            }

            FillPropGrid(SelectedSceneObject, SelectedSceneObject.ClassName);

            prev = new Types.Point(pos.X, pos.Y);
        }


        private void ToolsMovingBtn(object sender, RoutedEventArgs e) => ToolButton(sender.As<Button>(), Cursors.SizeAll,
            () => moveTool = true,
            () => moveTool = false);

        private void ToolsUsualBtn(object sender, RoutedEventArgs e) => ToolButton(sender.As<Button>(), Cursors.Arrow);

        private Dictionary<Button, bool> ToolboxButtons = new Dictionary<Button, bool>();

        private void ToolButton(Button btn, Cursor cursor, Action enable=default, Action disable=default)
        {
            this.toolMove.Background = Brushes.Transparent;
            this.toolDefault.Background = Brushes.Transparent;

            if (!ToolboxButtons.ContainsKey(btn))
            {
                ToolboxButtons.Add(btn, false);
            }

            if (ToolboxButtons[btn]) // disabled
            {
                XnaHost.Cursor = Cursors.Arrow;
                disable?.Invoke();
                ToolboxButtons[btn] = false;
            }
            else // enabled
            {
                btn.Background = Brushes.Gray;
                XnaHost.Cursor = cursor;
                enable?.Invoke();
                ToolboxButtons[btn] = true;
            }
        }

        private void refreshSceneBtn_Click(object sender, RoutedEventArgs e)
        {
            SceneManager.Change<EasyScene>();
            if (SelectedScene == default)
                return;

            foreach (var obj in SelectedScene.SceneObjects)
            {
                PushSceneObjectToScene(obj);
            }
        }

        private void resetScaleBtn(object sender, RoutedEventArgs e)
        {
            XnaHost.Camera.CameraOffsetZ = 0;
        }

        private void tileEditorBtn(object sender, RoutedEventArgs e)
        {
            if (this.Project == default)
            {
                Message.Show("Нет проекта!");
                return;
            }
            else
            {
                new TileEditorForm(this.Project).Show();
            }
        }
    }
}