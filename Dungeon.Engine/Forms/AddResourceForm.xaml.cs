using Dungeon.Engine.Events;
using Dungeon.Engine.Projects;
using LiteDB;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace Dungeon.Engine.Forms
{
    /// <summary>
    /// Interaction logic for ProjectForm.xaml
    /// </summary>
    public partial class AddResourceForm : Window
    {
        string resPath;

        private DungeonEngineResourcesGraph ParentRes { get; set; } 

        public AddResourceForm(DungeonEngineResourcesGraph parent)
        {
            ParentRes = parent;
            InitializeComponent();
            DataContext = this;
        }
        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SelectPathButton(object sender, RoutedEventArgs e)
        {
            using var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            PhysicalPath.Content = resPath = dialog.FileName;
        }

        private void AddResProcess(object sender, RoutedEventArgs e)
        {
            DungeonGlobal.Events.Raise(new ResourceAddEvent(resPath,ParentRes));
            this.Close();
        }

        private void Cancel(object sender, RoutedEventArgs e) => OnCloseButtonClick(sender, e);
    }


}
