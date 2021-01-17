using Dungeon.Engine.Editable;
using Dungeon.Engine.Editable.ObjectTreeList;
using Dungeon.Engine.Editable.PropertyTable;
using Dungeon.Engine.Editable.Structures;
using Dungeon.Engine.Engine;
using Dungeon.Engine.Events;
using Dungeon.Engine.Forms;
using Dungeon.Engine.Host;
using Dungeon.Engine.Menus;
using Dungeon.Engine.Projects;
using Dungeon.Engine.Utils;
using Dungeon.Resources;
using Dungeon.Scenes.Manager;
using Dungeon.Types;
using Dungeon.Utils.ReflectionExtensions;
using Dungeon.View.Interfaces;
using LiteDB;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public EngineProject Project { get; set; }

        public Scene SelectedScene { get; set; }

        public SceneManager SceneManager { get; set; }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            InitializeMenu();

            DungeonGlobal.AudioPlayer = new AudioPlayerImpl();

            DungeonGlobal.Events.Subscribe<ProjectInitializeEvent>(InitializeProject, false);
            DungeonGlobal.Events.Subscribe<PropGridFillEvent>(PropGrid.Fill, false);
            DungeonGlobal.Events.Subscribe<FreezeAllEvent>(FreezeEventHandler, false);
            DungeonGlobal.Events.Subscribe<UnfreezeAllEvent>(UnreezeEventHandler, false);
            DungeonGlobal.Events.Subscribe<StatusChangeEvent>(ChangeStatusHandler, false);
            DungeonGlobal.Events.Subscribe<ResourceAddEvent>(AddedNewResourceEvent, false);
            DungeonGlobal.Events.Subscribe<PublishSceneObjectEvent>(PublishSceneObject, false);
            DungeonGlobal.Events.Subscribe<SceneObjectInObjectTreeSelectedEvent>(SelectedSceneObjectEvent, false);
            DungeonGlobal.Events.Subscribe<SceneResolutionChangedEvent>(ChangedResolutionEvent, false);
            DungeonGlobal.Events.Subscribe<RemoveSceneObjectFromSceneEvent>(RemovingSceneObjectFromSceneEvent,false);
            DungeonGlobal.Events.Subscribe<AddStructObjectEvent>(AddStructObjectEvent, false);
            
            SceneManager = new SceneManager()
            {
                DrawClient = XnaHost
            };
            DungeonGlobal.SceneManager = SceneManager;
            XnaHost.BindSceneManager(SceneManager);

            this.KeyDown += XnaHost.OnKeyDown;
            this.KeyUp += XnaHost.OnKeyUp;

            XnaHost.MouseDown += XnaHost_MouseDown;
            this.MouseUp += XnaHost_MouseUp;
            this.MouseMove += XnaHost_MouseMove;

            StructsView.AddObjectBinding += (e, r) => AddStruct(e, r);
            StructsView.TreeView.SelectedItemChanged += StructSelect;
        }

        private void StructSelect(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var @struct = e.NewValue.As<StructureObject>();
            if (@struct == default)
                return;
            if (!@struct.IsInitialized)
                @struct.InitTable();

            StructProps.Fill(@struct);

            if (@struct is StructureSceneObject structSceneObject)
            {
                var sceneObjMeta = structSceneObject.SceneObject;
                if (sceneObjMeta != default)
                {
                    if (!sceneObjMeta.IsInitialized)
                        sceneObjMeta.InitTable();

                    PropGrid.Fill(sceneObjMeta, structSceneObject.SceneObjectTypeSelected?.Name);
                }
            }
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
                App.Container.Register<EngineProject, EngineProject>(Utils.ContainerLifeStyle.Singleton, @event.Project);
                PropGrid.Clear();
                this.Project = proj;
                ChangeStatus($"Загружен проект '{Project.Name}'");
                WindowTitle.Text = $"Dungeon Engine - {Project.Name}";

                ScenesView.ItemsSource = Project.Scenes;

                if (Project.Resources == default)
                {
                    Project.Resources = new ObservableCollection<ResourcesGraph>
                    {
                        new ResourcesGraph() //root
                    };
                    var meta = new ResourcesGraph() { Name = "DungeonEngine.meta", Type = ResourceType.Embedded };
                    var root = Project.Resources.FirstOrDefault();
                    meta.Parent = root;
                    root.Nodes.Add(meta);
                }


                Project.Load();
                this.XnaHost.ChangeCell(Project.CompileSettings.CellSize);

                ResourceLoader.NotDisposingResources = true;
                ResourceLoader.Settings = new ResourceLoaderSettings()
                {
                    ThrowIfNotFound = false,
                    NotFoundAction = r => ChangeStatus("Не найден ресурс: "+r)
                };
                ResourceLoader.ResourceDatabaseResolvers.Add(new EngineResourceDatabaseResolver(Project.DbFilePath));
                ResourceLoader.ResourceResolvers.Add(new EmbeddedResourceResolver());
                ResourceLoader.ResourceResolvers.Add(new PhysicalFileResourceResolver());

                SceneManager = new SceneManager()
                {
                    DrawClient = XnaHost
                };
                DungeonGlobal.SceneManager = SceneManager;
                XnaHost.BindSceneManager(SceneManager);

                SceneManager.Start();
                SceneManager.Change<EasyScene>();
                PublishCurrentScene();

                DungeonGlobal.Sizes.Width = Project.CompileSettings.WidthPixel;
                DungeonGlobal.Sizes.Height = Project.CompileSettings.HeightPixel;
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
                StructsView.ItemsSource = default;

                Project = default;
                PropGrid.Clear();
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
            System.Windows.Application.Current.Shutdown();
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

            Project.Scenes.Add(new Scene() { Name = "Scene", Width = DungeonGlobal.Resolution.Width, Height = DungeonGlobal.Resolution.Width });

            ScenesView.SelectedIndex = ScenesView.Items.Count - 1;
        }

        private void RemoveScene(object sender, RoutedEventArgs e)
        {
            if (ScenesView.SelectedItem is Scene des)
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

        private void AddStruct(object sender, RoutedEventArgs e)
        {
            if (SelectedScene != default)
            {
                ObjectTreeListItem parent = default;

                var frameworkElement = e.Source.As<FrameworkElement>();
                if(frameworkElement.DataContext is ObjectTreeListItem @struct)
                {
                    parent = @struct;
                }

                new AddSctructureObject(parent)
                    .ShowDialog();
            }
        }

        private void AddStructObjectEvent(AddStructObjectEvent @event)
        {
            SelectedScene.StructObjects.Add(@event.StructureObject);            
            StructsView.UpdateLayout();
        }

        private void RemovingSceneObjectFromSceneEvent(RemoveSceneObjectFromSceneEvent @event)
        {
            var obj = @event.RootedObject;
            if (obj.Parent == default)
            {
                obj.RemoveFromHost();
                if (obj.Instance != default)
                {
                    var instance = obj.Instance.As<ISceneObject>();
                    instance.Layer.RemoveObject(instance);
                    
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
                SelectedScene = (Scene)item;
                SelectScene((Scene)item);

                SceneManager.Change<EasyScene>();
                PublishCurrentScene();

                return;
            }
#warning diabling btns
            //AddObjectBtn.IsEnabled = RemoveObjectBtn.IsEnabled = false;

        }

        private void PublishCurrentScene()
        {
            if (SelectedScene == default)
                return;

            SceneManager.Change<Scenes.Sys_Clear_Screen>();
            SceneManager.Change<EasyScene>();
            foreach (var @struct in SelectedScene.StructObjects)
            {
                if (@struct is StructureLayer structureLayer) //always, but easy cast
                {
                    structureLayer.SceneLayer = SceneManager.Current.AddLayer(structureLayer.Name);

                    foreach (var obj in structureLayer.Nodes)
                    {
                        if (obj is StructureSceneObject structSceneObject && structSceneObject.SceneObject != default)
                        {
                            PushSceneObjectToScene(structSceneObject.SceneObject, layer: structureLayer.SceneLayer);
                        }
                    }
                }
            }
        }

        private void PushSceneObjectToScene(SceneObject obj, SceneObject parent = default, Scenes.SceneLayer layer=default)
        {
            var instance = SceneObjectActivator.Activate(obj, out string error);
            if (error.IsNotEmpty())
            {
                this.ChangeStatus(error, true);
                return;
            }

            if (obj.Nodes?.Count > 0)
            {
                foreach (var node in obj.Nodes)
                {
                    PushSceneObjectToScene(node.As<SceneObject>(), obj,layer);
                }
            }

            if (parent != default)
            {
                var instance1 = parent.Instance.As<ISceneObject>();
                instance1.AddChild(instance.As<ISceneObject>());
                return;
            }

            layer.AddObject(instance.As<ISceneObject>());
            obj.Published = true;
        }

        private void SelectScene(Scene item)
        {
            StructsView.ItemsSource = SelectedScene.StructObjects;
            PropGrid.Fill(new PropGridFillEvent(item));
            XnaHost.Width = SelectedScene.Width;
            XnaHost.Height = SelectedScene.Height;
            SceneResolution.Content = $"{XnaHost.Width}x{XnaHost.Height}";
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.S && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                Save();

            base.OnKeyDown(e);
        }

        private void Save()
        {
            StructProps.Save();
            PropGrid.Save();
            Project.Save();
            this.XnaHost.ChangeCell(Project.CompileSettings.CellSize);
            ScenesView.Items.Refresh();
            StructsView.TreeView.Items.Refresh();
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
            
        }

        private void PublishSceneObject(PublishSceneObjectEvent @event) => PushSceneObjectToScene(@event.SceneObject, @event.Parent, @event.Layer.SceneLayer);

        private SceneObject SelectedSceneObject;

        private void SelectedSceneObjectEvent(SceneObjectInObjectTreeSelectedEvent @event)
        {
            if (@event.SceneObject != default)
            {
                PropGrid.Fill(@event.SceneObject, @event.SceneObject.ClassName);
            }
            SelectedSceneObject = @event.SceneObject;
        }

        private void AddResourceCtxClick(object sender, RoutedEventArgs e)
        {
            //if (ResourcesView.SelectedItem is DungeonEngineResourcesGraph resGraph)
            //{
            //    if (resGraph.Type == DungeonEngineResourceType.Folder || resGraph.Display == "Resources")
            //    {
            //        new AddResourceForm(resGraph).Show();
            //    }
            //    else
            //    {
            //        ChangeStatus("Ресурс можно добавить только в папку или корень проекта!");
            //    }
            //}
        }

        private void RemoveResourceCtxClick(object sender, RoutedEventArgs e)
        {
            //if (ResourcesView.SelectedItem is DungeonEngineResourcesGraph resGraph)
            //{
            //    if (resGraph.Type == DungeonEngineResourceType.Embedded || resGraph.Display == "Resources")
            //        return;

            //    if (RemoveResImpl(resGraph))
            //    {
            //        resGraph.Parent.Nodes.Remove(resGraph);
            //    }
            //    else
            //    {
            //        ChangeStatus($"Не удалось удалить ресурс: {resGraph.GetFullPath()}");
            //    }
            //}
        }

        private bool RemoveResImpl(ResourcesGraph resGraph)
        {
            if (resGraph.Type != ResourceType.Folder)
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
                List<ResourcesGraph> remove = new List<ResourcesGraph>();

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
            //if (ResourcesView.SelectedItem is DungeonEngineResourcesGraph resGraph)
            //{
            //    var nmform = new AddNamedForm("Переименовать ресурс");
            //    nmform.ShowDialog();

            //    using var db = new LiteDatabase(Project.DbFilePath);
            //    var store = db.GetCollection<Resource>();

            //    var fullPath = resGraph.GetFullPath();
            //    var stored = store.FindOne(r => r.Path == fullPath);

            //    if (stored != default)
            //    {
            //        resGraph.Name = nmform.Text;
            //        stored.Path = resGraph.GetFullPath();
            //        store.Update(stored);
            //    }
            //    else
            //    {
            //        ChangeStatus($"Неудалось переименовать ресурс: {resGraph.Display}!", error: true);
            //    }
            //}
        }

        private void AddResourceFolderCtxClick(object sender, RoutedEventArgs e)
        {
            //if (ResourcesView.SelectedItem is DungeonEngineResourcesGraph resGraph)
            //{
            //    if (resGraph.Type == DungeonEngineResourceType.Folder || resGraph.Display == "Resources")
            //    {
            //        var nmform = new AddNamedForm("Добавить папку");
            //        nmform.ShowDialog();
            //        resGraph.Nodes.Add(new DungeonEngineResourcesGraph()
            //        {
            //            Name = nmform.Text,
            //            Type = DungeonEngineResourceType.Folder,
            //            Parent = resGraph
            //        });
            //    }
            //}
        }

        private void SceneDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedScene == default)
                return;
            SelectScene(ScenesView.SelectedItem.As<Scene>());
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
                    SelectedSceneObject.Instance.As<ISceneObject>().ComputedPosition.X,
                    SelectedSceneObject.Instance.As<ISceneObject>().ComputedPosition.Y,
                    SelectedSceneObject.Instance.As<ISceneObject>().Width,
                    SelectedSceneObject.Instance.As<ISceneObject>().Height);

                if (obj.Width == 0 && obj.Height == 0 && !string.IsNullOrWhiteSpace(SelectedSceneObject.Instance.As<ISceneObject>().Image))
                {
                    var size = DungeonGlobal.DrawClient.MeasureImage(SelectedSceneObject.Instance.As<ISceneObject>().Image);
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
                    SelectedSceneObject.Instance.As<ISceneObject>().Left = X;
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
                    SelectedSceneObject.Instance.As<ISceneObject>().Top = Y;
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

            PropGrid.Fill(SelectedSceneObject, SelectedSceneObject.ClassName);

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

            PublishCurrentScene();
        }

        private void resetScaleBtn(object sender, RoutedEventArgs e)
        {
            XnaHost.Camera.CameraOffsetZ = 0;
        }
    }
}