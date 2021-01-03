using Dungeon.Engine.Projects;
using Dungeon.Resources;
using LiteDB;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace Dungeon.Engine.Forms
{
    public partial class AddResourceForm : Window
    {
        string resPath;

        private bool available;

        private ObservableCollection<ResourcesGraph> resources = new ObservableCollection<ResourcesGraph>();
        private EngineProject project;

        public AddResourceForm()
        {
            project = App.Container.Resolve<EngineProject>();
            available = project != default;
            InitializeComponent();
            if (available)
            {
                ResourcesView.ItemsSource = resources = project.Resources;
            }
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
            nametxt.Text = "";
        }

        private void AddResProcess(object sender, RoutedEventArgs e)
        {
            var resType = SelectedObjectTypeView.SelectedItem.As<ResourceType>();
            if (resType != ResourceType.Folder && string.IsNullOrWhiteSpace(resPath))
            {
                Message.Show("Path is empty.");
                return;
            }

            ResourcesGraph parent;
            if (ResourcesView.SelectedItem == default)
            {
                parent = resources.FirstOrDefault();
            }
            else
            {
                parent = ResourcesView.SelectedItem.As<ResourcesGraph>();
            }

            switch (resType)
            {
                case ResourceType.Folder:
                    AddFolder(parent);
                    break;
                case ResourceType.Image:
                    AddWithoutProcess(parent, resType);
                    break;
                case ResourceType.File:
                case ResourceType.Embedded:
                case ResourceType.Font:
                case ResourceType.Model3D:
                case ResourceType.Music:
                case ResourceType.Audio:
                case ResourceType.Particle:
                case ResourceType.Shader:
                default:
                    Message.Show($"Processor for resource type {resType} not found!");
                    break;
            }

            nametxt.Text = "";
        }

        private void AddFolder(ResourcesGraph parent)
        {
            var nmform = new AddNamedForm("Название папки");
            nmform.ShowDialog();

            var text = nmform.Text;

            if (!string.IsNullOrWhiteSpace(text))
            {
                parent.Nodes.Add(new ResourcesGraph()
                {
                    Name = text,
                    Type = ResourceType.Folder,
                    Parent = parent
                });
            }
            else Message.Show("Folder must have name!");
        }

        private void AddWithoutProcess(ResourcesGraph parent, ResourceType resType)
        {
            var newRes = new ResourcesGraph
            {
                Type = resType,
                Parent = parent
            };
            parent.Nodes.Add(newRes);
            newRes.Name = string.IsNullOrWhiteSpace(nametxt.Text)
                ? Path.GetFileName(resPath)
                : nametxt.Text;

            newRes.ResourceId = new LiteDatabase(project.DbFilePath).GetCollection<Resource>()
                .Insert(new Resource()
                {
                    Path = newRes.GetFullPath(),
                    Data = File.ReadAllBytes(resPath),
                    CustomInfo = resType.ToString()
                });

            project.Save();
        }

        private void Cancel(object sender, RoutedEventArgs e) => OnCloseButtonClick(sender, e);

        private void ResourcesView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            nametxt.Text = ResourcesView?.SelectedItem?.As<ResourcesGraph>()?.Name ?? "";
        }

        private void RenameBtn(object sender, RoutedEventArgs e)
        {
            var selected = ResourcesView.SelectedItem;
            if (selected != default)
            {
                if (selected is ResourcesGraph resGraph)
                {
                    if (resGraph.Nodes.Count > 0)
                    {
                        Message.Show("Can't rename folder which already contains files!");
                        return;
                    }

                    resGraph.Name = nametxt.Text;
                    nametxt.Text = "";

                    var db = new LiteDatabase(project.DbFilePath).GetCollection<Resource>();

                    var res = db.FindById(resGraph.ResourceId);
                    res.Path = resGraph.GetFullPath();
                    db.Update(resGraph.ResourceId, res);
                }
            }
        }

        private void CopyResPath(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText(ResourcesView.SelectedItem.As<ResourcesGraph>().GetFullPath());
        }

        private void DelResProcess(object sender, RoutedEventArgs e)
        {
            var item = ResourcesView.SelectedItem.As<ResourcesGraph>();
            item.Parent.Nodes.Remove(item);

            if (item.ResourceId != default)
            {
                new LiteDatabase(project.DbFilePath)
                    .GetCollection<Resource>()
                    .Delete(item.ResourceId);
            }
            else
            {
                var db = new LiteDatabase(project.DbFilePath).GetCollection<Resource>();
                db.Delete(x => x.Path == item.GetFullPath());
            }
        }
    }
}