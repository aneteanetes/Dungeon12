using System.Collections.Generic;
using System.Windows;

namespace Dungeon.Engine.Forms
{
    /// <summary>
    /// Interaction logic for ProjectForm.xaml
    /// </summary>
    public partial class NestedPropertyGrid : Window
    {
        public NestedPropertyGrid(string title, object Obj, List<string> path)
        {
            InitializeComponent();
            this.Title.Text = title;
            PathVisual.Text = string.Join(" / ", path);
            DataContext = this;
            PropGrid.Fill(Obj);

        }
        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Cancel(object sender, RoutedEventArgs e) => OnCloseButtonClick(sender, e);
    }
}
