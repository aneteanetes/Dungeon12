using Dungeon.Engine.Events;
using Dungeon.Engine.Forms;
using Dungeon.Engine.Projects;
using Force.DeepCloner;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Dungeon.Engine.Controls
{
    /// <summary>
    /// Interaction logic for SceneObjectsTreeList.xaml
    /// </summary>
    public partial class SceneObjectsTreeList : UserControl
    {

        private ObservableCollection<DungeonEngineSceneObject> items = new ObservableCollection<DungeonEngineSceneObject>();

        [Bindable(true)]
        public ObservableCollection<DungeonEngineSceneObject> ItemsSource
        {
            get => items;
            set
            {
                items = value;
                this.SceneObjectsView.ItemsSource = value;
            }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        "ItemsSource",
        typeof(ObservableCollection<DungeonEngineSceneObject>),
        typeof(SceneObjectsTreeList),
        new PropertyMetadata(default(ObservableCollection<DungeonEngineSceneObject>), PropertyChangedCallback));

        public static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public TreeView TreeView => this.SceneObjectsView;

        public SceneObjectsTreeList()
        {
            InitializeComponent();
            this.SceneObjectsView.ItemsSource = this.ItemsSource;
        }

        private void AddSceneObject(object sender, System.Windows.RoutedEventArgs e)
        {
            new AddSceneObjectForm(Selected).Show();
        }

        private void CopySceneObject(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Selected.Parent != default)
            {
                var cloned = Selected.Clone();
                Selected.Parent.Nodes.Add(cloned);
                DungeonGlobal.Events.Raise(new AddSceneObjectEvent(false) { SceneObject = cloned });
            }
            else
            {
                DungeonGlobal.Events.Raise(new AddSceneObjectEvent()
                {
                    SceneObject = Selected.Clone()
                });
            }
        }

        private void RemoveSceneObject(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Selected.Parent != default)
            {
                Selected.Parent.Nodes.Remove(Selected);
            }
            else
            {
                DungeonGlobal.Events.Raise(new RemoveSceneObjectFromSceneEvent(Selected));
            }
        }

        private DungeonEngineSceneObject Selected => TreeView.SelectedItem.As<DungeonEngineSceneObject>();

        private void SceneObjectsView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _lastMouseDown = Mouse.GetPosition(SceneObjectsView);
            DungeonGlobal.Events.Raise(new SceneObjectInObjectTreeSelectedEvent()
            {
                SceneObject = e.NewValue.As<DungeonEngineSceneObject>()
            });
        }

        Point _lastMouseDown;
        DungeonEngineSceneObject draggedItem, _target;

        private void SceneObjectsView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPosition = e.GetPosition(SceneObjectsView);

                if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
                    (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
                {
                    draggedItem = SceneObjectsView.SelectedItem.As<DungeonEngineSceneObject>();
                    if (draggedItem != null)
                    {
                        DragDropEffects finalDropEffect = DragDrop.DoDragDrop(SceneObjectsView, SceneObjectsView.SelectedValue,
                            DragDropEffects.Move);
                        //Checking target is not null and item is dragging(moving)
                        if ((finalDropEffect == DragDropEffects.Move))
                        {
                            if (_target != default)
                            {
                                // A Move drop was accepted
                                if (draggedItem != _target && _target!=draggedItem && _target.Parent != draggedItem)
                                {
                                    MoveItem(draggedItem, _target);
                                }
                            }
                            else
                            {
                                var diff = (int)(Math.Abs(currentPosition.Y - _lastMouseDown.Y) % 10);

                                //переместить там же где есть
                                //но выше
                                if (currentPosition.Y < _lastMouseDown.Y)
                                {
                                    var initIndex = draggedItem.Parent.Nodes.IndexOf(draggedItem);
                                    var newIndex = initIndex - diff;
                                    draggedItem.Parent.Nodes.Remove(draggedItem);
                                    if (newIndex > draggedItem.Parent.Nodes.Count - 1)
                                    {
                                        newIndex = draggedItem.Parent.Nodes.Count - 1;
                                    }
                                    if (newIndex < 0)
                                    {
                                        newIndex = 0;
                                    }
                                    draggedItem.Parent.Nodes.Insert(newIndex, draggedItem);

                                }
                                //но ниже
                                else
                                {
                                    var initIndex = draggedItem.Parent.Nodes.IndexOf(draggedItem);
                                    var newIndex = initIndex + diff;
                                    draggedItem.Parent.Nodes.Remove(draggedItem);
                                    if (newIndex > draggedItem.Parent.Nodes.Count - 1)
                                    {
                                        newIndex = draggedItem.Parent.Nodes.Count - 1;
                                    }
                                    if(newIndex<0)
                                    {
                                        newIndex = 0;
                                    }
                                    draggedItem.Parent.Nodes.Insert(newIndex, draggedItem);
                                }
                            }

                            _target = null;
                            draggedItem = null;
                        }
                    }
                }
            }
        }

        private void SceneObjectsView_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _lastMouseDown = e.GetPosition(SceneObjectsView);
            }
        }

        private void SceneObjectsView_Drop(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;

            // Verify that this is a valid drop and then store the drop target
            var TargetItem = GetNearestContainer(e.OriginalSource.As<UIElement>());
            if (draggedItem != null)
            {
                _target = TargetItem;
                e.Effects = DragDropEffects.Move;
            }
        }

        private DungeonEngineSceneObject GetNearestContainer(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item.
            TreeViewItem container = element as TreeViewItem;
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
            }
            if (container != default)
            {
                return container.DataContext.As<DungeonEngineSceneObject>();
            }
            else
            {
                return default;
            }
        }

        private void MoveItem(DungeonEngineSceneObject _sourceItem, DungeonEngineSceneObject _targetItem)
        {
            //adding dragged TreeViewItem in target TreeViewItem
            _targetItem.Nodes.Add(_sourceItem);

            //finding Parent TreeViewItem of dragged TreeViewItem 
            _sourceItem.Parent.Nodes.Remove(_sourceItem);
            _sourceItem.Parent = _targetItem;
        }
    }
}