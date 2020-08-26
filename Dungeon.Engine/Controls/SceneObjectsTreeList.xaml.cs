using Dungeon.Engine.Events;
using Dungeon.Engine.Projects;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

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
            set {
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

        }

        private void RemoveSceneObject(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void SceneObjectsView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DungeonGlobal.Events.Raise(new SceneObjectInObjectTreeSelectedEvent()
            {
                SceneObject= e.NewValue.As<DungeonEngineSceneObject>()
            });
        }
    }
}
