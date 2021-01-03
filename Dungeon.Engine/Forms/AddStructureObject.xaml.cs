using Dungeon.Engine.Editable;
using Dungeon.Engine.Editable.ObjectTreeList;
using Dungeon.Engine.Editable.Structures;
using Dungeon.Engine.Events;
using System.Windows;

namespace Dungeon.Engine.Forms
{
    public partial class AddSctructureObject : Window
    {
        public ObjectTreeListItem ParentStruct { get; set; }

        public AddSctructureObject(ObjectTreeListItem parent)
        {
            ParentStruct = parent;
            InitializeComponent();
            DataContext = this;
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddStruct(object sender, RoutedEventArgs e)
        {
            if (SelectedObjectTypeView.SelectedItem is StructureObjectType selectedType)
            {
                StructureObject newStruct = default;

                switch (selectedType)
                {
                    case StructureObjectType.Layer:
                        newStruct = new StructureLayer();
                        break;
                    case StructureObjectType.Object:
                        newStruct = new StructureSceneObject();
                        break;
                    case StructureObjectType.TileMap:
                        newStruct = new StructureTilemap();
                        break;
                    default:
                        break;
                }

                newStruct.SetPropertyExpr(nameof(StructureObject.Name), SceneObjectNameView.Text);

                if (ParentStruct != default)
                {
                    var parent = ParentStruct.As<StructureObject>();
                    if (!parent.StructureType.CanContains(selectedType))
                    {
                        Message.Show($"{parent.StructureType} can't contains {selectedType}!");
                        return;
                    }

                    newStruct.Parent = ParentStruct;
                    ParentStruct.Nodes.Add(newStruct);
                    this.Close();
                    return;
                }
                else if (!selectedType.CanInRoot())
                {
                    Message.Show($"{selectedType} can't be root structure! Put it in layer");
                    return;
                }

                DungeonGlobal.Events.Raise(new AddStructObjectEvent(newStruct));

                this.Close();
            }
        }

        private void Cancel(object sender, RoutedEventArgs e) => OnCloseButtonClick(sender, e);
    }
}