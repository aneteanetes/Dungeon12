using Dungeon.Engine.Projects;
using System.Windows;

namespace Dungeon.Engine.Forms
{
    public partial class TileEditorForm : Window
    {
        public TileEditorForm(DungeonEngineProject dungeonEngineProject)
        {
            InitializeComponent();

            this.MapCollection.CollectionName.Content = "Карты";
            this.LayerCollection.CollectionName.Content = "Слои";
            this.ImageCollection.CollectionName.Content = "Тайлсеты";
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
    }
}
