using Dungeon.Engine.Projects;
using System.Windows;
using System.Windows.Forms;

namespace Dungeon.Engine.Forms
{
    /// <summary>
    /// Interaction logic for ProjectForm.xaml
    /// </summary>
    public partial class ProjectForm : Window
    {
        public DungeonEngineProject Project { get; set; } = new DungeonEngineProject();

        public ProjectForm()
        {
            InitializeComponent();
            DataContext = this;
        }
        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SelectPathButton(object sender, RoutedEventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
            PhysicalPath.Text = Project.Path = dialog.SelectedPath;            
        }

        private void CreateNewProject(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Cancel(object sender, RoutedEventArgs e) => OnCloseButtonClick(sender, e);
    }


}
