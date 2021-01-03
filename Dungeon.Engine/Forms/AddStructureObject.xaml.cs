using Dungeon.Engine.Editable;
using Dungeon.Engine.Events;
using System.Windows;

namespace Dungeon.Engine.Forms
{
    public partial class AddSctructureObject : Window
    {
        public DungeonEngineStructureObject ParentStruct { get; set; }

        public AddSctructureObject(DungeonEngineStructureObject parent)
        {
            ParentStruct = parent;
            InitializeComponent();
            DataContext = this;
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddResProcess(object sender, RoutedEventArgs e)
        {
            if (SelectedObjectTypeView.SelectedItem is DungeonEngineStructureObjectType selectedType)
            {
                var newStruct = new DungeonEngineStructureObject
                {
                    Name = SceneObjectNameView.Text,
                    StructureType = selectedType
                };


                if (ParentStruct != default)
                {
                    if (!ParentStruct.StructureType.CanContains(selectedType))
                    {
                        Message.Show($"{ParentStruct.StructureType} can't contains {selectedType}!");
                        return;
                    }

                    newStruct.Parent = ParentStruct;
                }
                else if (!selectedType.CanInRoot())
                {
                    Message.Show($"{selectedType} can't be root structure! Put it in layer");
                }

                DungeonGlobal.Events.Raise(new AddStructObjectEvent(newStruct));

                this.Close();
            }
        }

        private void Cancel(object sender, RoutedEventArgs e) => OnCloseButtonClick(sender, e);
    }
}