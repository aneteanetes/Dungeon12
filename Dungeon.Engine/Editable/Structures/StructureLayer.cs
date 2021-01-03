using Dungeon.Engine.Editable.ObjectTreeList;
using Dungeon.Engine.Projects;
using System.Collections.ObjectModel;
using System.Linq;

namespace Dungeon.Engine.Editable.Structures
{
    public class StructureLayer : StructureObject
    {
        public StructureLayer()
        {
            this.BindEmbeddedIcon("LayerFillSlider_16x");
        }

        public override StructureObjectType StructureType => StructureObjectType.Layer;

        public override void Remove()
        {
            App.Container.Resolve<EngineProject>().Scenes.ForEach(s =>
            {
                s.StructObjects.Remove(this);
            });
        }

        public override void CopyInParent()
        {
            var scene = App.Container.Resolve<EngineProject>()
                .Scenes
                .FirstOrDefault(s => s?.StructObjects?.Contains(this) ?? false);

            scene?.StructObjects.Add(this.Clone());
        }

        public override ObjectTreeListItem Clone()
        {
            var layer = new StructureLayer();
            FillClone(layer);
            return layer;
        }

        public override ObservableCollection<ObjectTreeListItem> CloneNodes() => new ObservableCollection<ObjectTreeListItem>();
    }
}