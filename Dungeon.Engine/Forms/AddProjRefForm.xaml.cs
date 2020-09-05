using Dungeon.Engine.Editable;
using Dungeon.Engine.Engine;
using Dungeon.Engine.Events;
using Dungeon.Engine.Projects;
using Dungeon.Resources;
using LiteDB;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace Dungeon.Engine.Forms
{
    /// <summary>
    /// Interaction logic for ProjectForm.xaml
    /// </summary>
    public partial class AddProjRefForm : Window
    {
        public DungeonEngineProject Project { get; set; }

        public DungeonEngineReference Reference { get; set; } = new DungeonEngineReference()
        {
            Kind = DungeonEngineReferenceKind.Loadable
        };

        public AddProjRefForm(DungeonEngineProject proj)
        {
            Project = proj;
            InitializeComponent();
            DataContext = this;
        }
        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SelectPathButton(object sender, RoutedEventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "C# library (.dll)|*.dll"
            };
            var result = dialog.ShowDialog();
            PhysicalPath.Content = Reference.Path = dialog.FileName;
        }

        private void SelectDbPathButton(object sender, RoutedEventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "Dungeon resources (.dtr)|*.dtr"
            };
            var result = dialog.ShowDialog();
            DBPath.Content = Reference.DbPath = dialog.FileName;
        }

        private void AddResProcess(object sender, RoutedEventArgs e)
        {
            if(!ResourceLoader.LoadAssemblyUnloadable(Reference.Path))
            {
                Message.Show($"Не удалось загрузить сборку {Reference.Path}!");
                return;
            }
            else if (!ResourceLoader.UnloadAssemblies().GetAwaiter().GetResult())
            {
                Message.Show($"Не удалось выгрузить сборку {Reference.Path}, желательно перезапустить программу!");
                return;
            }

            if (!string.IsNullOrWhiteSpace(Reference.Path))
            {
                Reference.Title = Path.GetFileNameWithoutExtension(Reference.Path);

                Project.References.Add(Reference);
                ResourceLoader.LoadAssemblyUnloadable(Reference.Path);
                ResourceLoader.ResourceDatabaseResolvers.Add(new DungeonEngineResourceDatabaseResolver(Reference.DbPath));

                DungeonGlobal.Events.Raise(new AddProjRefEvent(Reference));
            }
            this.Close();
        }

        private void Cancel(object sender, RoutedEventArgs e) => OnCloseButtonClick(sender, e);
    }


}
