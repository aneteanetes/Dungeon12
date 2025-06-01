using Dungeon.Engine.Projects;
using System.Collections.Generic;
using System.Windows;

namespace Dungeon.Engine.Forms
{
    public partial class CollectionView : Window
    {
        private SceneObjectClass @class;
        private List<SceneObject> collection;
        private List<string> path;

        public CollectionView(string title, List<string> path, SceneObjectClass collectionClass,  List<SceneObject> collection)
        {
            @class = collectionClass;
            this.path = path;
            this.collection = collection;
            InitializeComponent();
            Title.Text = title;
            PathVisual.Text = string.Join(" / ", path);
            DataContext = this;
            ItemsView.ItemsSource = collection;

        }
        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Cancel(object sender, RoutedEventArgs e) => OnCloseButtonClick(sender, e);

        private void AddClick(object sender, RoutedEventArgs e)
        {
            var newItem = new SceneObject(@class)
            {
                Name = $"new {@class.Name}"
            };
            collection.Add(newItem);
            newItem.InitTable();
            ItemsView.Items.Refresh();
            ItemsView.UpdateLayout();
        }

        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            var selected = ItemsView.SelectedItem.As<SceneObject>();
            if (selected != default)
            {
                collection.Remove(selected);
            }
            ItemsView.Items.Refresh();
            ItemsView.UpdateLayout();
        }

        private void EditClick(object sender, RoutedEventArgs e)
        {
            var item = e.Source.As<FrameworkElement>()
               ?.DataContext.As<SceneObject>();

            if (item != default)
            {
                new NestedPropertyGrid(@class.Name, item, new List<string>(path)
                {
                    "Item"
                }).ShowDialog();
            }
        }
    }
}