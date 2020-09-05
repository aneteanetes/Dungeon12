using Dungeon.Engine.Editable;
using Dungeon.Engine.Projects;
using System.Windows;

namespace Dungeon.Engine.Forms
{
    /// <summary>
    /// Interaction logic for DllManagerForm.xaml
    /// </summary>
    public partial class DllManagerForm : Window
    {
        public DungeonEngineProject Project { get; set; }

        public DllManagerForm(DungeonEngineProject project)
        {
            this.Project = project;
            InitializeComponent();
            RefListView.ItemsSource = project.References;
        }

        private void RemoveDllReference(object sender, RoutedEventArgs e)
        {
            Project.References.Remove(RefListView.SelectedItem.As<DungeonEngineReference>());
            RefListView.ItemsSource = Project.References;
        }


        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddNewRef(object sender, RoutedEventArgs e)
        {
            new AddProjRefForm(Project).ShowDialog();
            RefListView.ItemsSource = Project.References;
        }
    }
}
