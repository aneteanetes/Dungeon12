using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Dungeon.Engine.Controls
{
    public abstract class EngineCollectionEditorSettings
    {
        public IEnumerable Collection { get; protected set; }

        public string Name { get; set; }

        public abstract object AddCall();

        public abstract void RemoveCall(object removed);

        public abstract void SelectCall(object select);
    }

    public class EngineCollectionEditorSettings<T> : EngineCollectionEditorSettings
        where T : class
    {
        public EngineCollectionEditorSettings(ObservableCollection<T> collection,
            Action<T> select = default, Action<T> remove = default, Func<T> add = default)
        {
            Collection = collection;
            Selected = select;
            Remove = remove;
            Add = add;
        }

        public Action<T> Selected { get; }

        public Action<T> Remove { get; }

        public Func<T> Add { get; }

        public override object AddCall() =>  Add?.Invoke();

        public override void RemoveCall(object removed) => Remove.Invoke(removed.As<T>());

        public override void SelectCall(object select) => Selected.Invoke(select.As<T>());
    }

    public partial class EngineCollectionEditor : UserControl
    {
        public EngineCollectionEditor()
        {
            InitializeComponent();
        }

        private EngineCollectionEditorSettings settings;

        public void Init<T>(EngineCollectionEditorSettings settings)
        {
            this.settings = settings;
            this.CollectionName.Content = settings.Name;
            this.CollectionView.ItemsSource = settings.Collection;
        }

        private void Selected(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                settings.SelectCall(item);
            }
        }

        private void Add(object sender, System.Windows.RoutedEventArgs e)
        {
            settings.AddCall();
        }

        private void Remove(object sender, System.Windows.RoutedEventArgs e)
        {
            settings.RemoveCall(CollectionView.SelectedItem);
        }

        private void SelectDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            settings.SelectCall(CollectionView.SelectedItem);
        }
    }
}