using Dungeon.Engine.Events;
using Dungeon.Engine.Projects;
using Dungeon.Resources;
using Dungeon.Utils;
using Dungeon.View.Interfaces;
using LiteDB;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace Dungeon.Engine.Forms
{
    public partial class AddSceneObjectForm : Window
    {
        public ObservableCollection<DungeonEngineSceneObjectClass> AvailableSceneObjects = new ObservableCollection<DungeonEngineSceneObjectClass>();

        public DungeonEngineSceneObject SceneObject { get; set; } = new DungeonEngineSceneObject();

        public DungeonEngineSceneObject ParentSceneObject { get; set; }

        public AddSceneObjectForm(DungeonEngineSceneObject parent)
        {
            ParentSceneObject = parent;
            InitializeComponent();
            DataContext = this;
            Init();
        }

        private void Init()
        {
            AvailableSceneObjects = new ObservableCollection<DungeonEngineSceneObjectClass>(
                ResourceLoader.LoadTypes<ISceneObject>()
                .Where(x => x.IsClass && !x.IsAbstract && Attribute.GetCustomAttribute(x, typeof(HiddenAttribute)) == default)
                .Select(x => new DungeonEngineSceneObjectClass()
                {
                    Name = (Attribute.GetCustomAttribute(x, typeof(DisplayNameAttribute)) as DisplayNameAttribute)?.DisplayName ?? x.Name,
                    ClassName = x.FullName,
                    ClassType = x
                }));
            SelectSceneObjectTypeView.ItemsSource = AvailableSceneObjects.OrderBy(x => x.Name);
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddResProcess(object sender, RoutedEventArgs e)
        {
            if (SelectSceneObjectTypeView.SelectedItem is DungeonEngineSceneObjectClass selectedType)
            {
                SceneObject.ClassName = selectedType.ClassName;
                SceneObject.ClassType = selectedType.ClassType;
                SceneObject.InitTable();

                if (ParentSceneObject == default)
                {
                    DungeonGlobal.Events.Raise(new AddSceneObjectEvent()
                    {
                        SceneObject = this.SceneObject                        
                    });
                }
                else
                {
                    ParentSceneObject.Nodes.Add(this.SceneObject);
                    DungeonGlobal.Events.Raise(new AddSceneObjectEvent(false)
                    {
                        SceneObject = this.SceneObject
                    });
                }

                this.Close();
            }
        }

        private void Cancel(object sender, RoutedEventArgs e) => OnCloseButtonClick(sender, e);
    }
}