using Geranium.Reflection;
using LiteDB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace Dungeon.Engine.Forms
{
    /// <summary>
    /// Interaction logic for ProjectForm.xaml
    /// </summary>
    public partial class LiteDbEditorForm : Window
    {
        private LiteDatabase LiteDatabase;

        public LiteDbEditorForm()
        {
            InitializeComponent();
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)=> this.CloseInternal();

        private void Close(object sender, RoutedEventArgs e)=> this.CloseInternal();

        private void Cancel(object sender, RoutedEventArgs e) => this.CloseInternal();

        private void CloseInternal() => this.Close();

        private void SelectPathButton(object sender, RoutedEventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "Dungeon resources (.dtr)|*.dtr;*.deproj"
            };
            var result = dialog.ShowDialog();

            try
            {
                LiteDatabase = new LiteDatabase(dialog.FileName);
            }
            catch
            {
                Message.Show("Невозможно прочитать данные!");
                return;
            }

            Collections.ItemsSource = new ObservableCollection<string>(LiteDatabase.GetCollectionNames());
        }

        List<object> data = new List<object>();

        private void Collections_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                if (item is string itm)
                {
                    throw new Exception("lite db viewer");
                    var type = typeof(object);

                    var @enum = LiteDatabase.CallGeneric("GetCollection", new Type[] { type })
                       .Call("FindAll")
                       .As<IEnumerable>()
                       .GetEnumerator();

                    while(@enum.MoveNext())
                    {
                        data.Add(@enum.Current);
                    }

                    CollectionDataView.ItemsSource = data;
                    Columns.ItemsSource = new ObservableCollection<string>(CollectionDataView.Columns.Select(x => x.Header.ToString()));
                    Columns.SelectedIndex = 0;
                }

                return;
            }
        }

        private void ApplyFilter(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                CollectionDataView.ItemsSource = data;
                return;
            }

            CollectionDataView.ItemsSource = data.Where(x => x.GetPropertyExprRaw(Columns.SelectedItem.ToString()).ToString().ToLowerInvariant().Contains(SearchBox.Text.ToLowerInvariant())).ToArray();
        }
    }
}
