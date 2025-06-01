using Dungeon.Engine.Editable;
using Dungeon.Engine.Editable.ObjectTreeList;
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

        private ObservableCollection<ObjectTreeListItem> items = new ObservableCollection<ObjectTreeListItem>();

        [Bindable(true)]
        public ObservableCollection<ObjectTreeListItem> ItemsSource
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
        typeof(ObservableCollection<ObjectTreeListItem>),
        typeof(SceneObjectsTreeList),
        new PropertyMetadata(default(ObservableCollection<ObjectTreeListItem>), PropertyChangedCallback));

        public static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public TreeView TreeView => this.SceneObjectsView;

        public SceneObjectsTreeList()
        {
            InitializeComponent();
            this.SceneObjectsView.ItemsSource = this.ItemsSource;
        }

        public Action<object, RoutedEventArgs> AddObjectBinding;

        private void AddObject(object sender, RoutedEventArgs e) => AddObjectBinding?.Invoke(sender, e);

        private void CopyObject(object sender, System.Windows.RoutedEventArgs e)
        {
            Selected.CopyInParent();
        }

        private void RemoveSceneObject(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Selected.Parent != default)
            {
                Selected.Parent.Nodes.Remove(Selected);
            }
            else
            {
#warning raise RemoveSceneObjectFromSceneEvent
                //DungeonGlobal.Events.Raise(new RemoveSceneObjectFromSceneEvent(Selected));
            }
        }

        public ObjectTreeListItem Selected => TreeView.SelectedItem.As<ObjectTreeListItem>();

        private void SceneObjectsView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
#warning selecteditem sceneview BLAD BLAD BLAD
            //_lastMouseDown = Mouse.GetPosition(SceneObjectsView);
            //DungeonGlobal.Events.Raise(new SceneObjectInObjectTreeSelectedEvent()
            //{
            //    ObjectTreeListItem = e.NewValue.As<ObjectTreeListItem>()
            //});
        }

        Point _lastMouseDown;
        ObjectTreeListItem draggedItem, _target;

        private void SceneObjectsView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPosition = e.GetPosition(SceneObjectsView);

                if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
                    (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
                {
                    draggedItem = SceneObjectsView.SelectedItem.As<ObjectTreeListItem>();
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
                            else if (draggedItem.Parent != default)
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
                                else if (draggedItem.Parent != default)
                                {
                                    var initIndex = draggedItem.Parent.Nodes.IndexOf(draggedItem);
                                    var newIndex = initIndex + diff;
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

        private ObjectTreeListItem GetNearestContainer(UIElement element)
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
                return container.DataContext.As<ObjectTreeListItem>();
            }
            else
            {
                return default;
            }
        }

        private void CallItemRemove(object sender, RoutedEventArgs e)
        {
            e.Source.As<FrameworkElement>()
                ?.DataContext.As<ObjectTreeListItem>()
                ?.Remove();
        }


        private void MoveItem(ObjectTreeListItem _sourceItem, ObjectTreeListItem _targetItem)
        {
            if (_sourceItem.Parent == default)
                return;

            //adding dragged TreeViewItem in target TreeViewItem
            _targetItem.Nodes.Add(_sourceItem);

            //finding Parent TreeViewItem of dragged TreeViewItem 
            _sourceItem.Parent.Nodes.Remove(_sourceItem);
            _sourceItem.Parent = _targetItem;
        }
    }
}