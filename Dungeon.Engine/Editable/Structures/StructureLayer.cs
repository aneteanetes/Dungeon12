using Dungeon.Engine.Editable.ObjectTreeList;
using Dungeon.Engine.Engine;
using Dungeon.Engine.Projects;
using Dungeon.Utils;
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
            HostScene?.StructObjects.Add(this.Clone());
        }

        public override ObjectTreeListItem Clone()
        {
            var layer = new StructureLayer();
            FillClone(layer);
            return layer;
        }

        public override ObservableCollection<ObjectTreeListItem> CloneNodes() => new ObservableCollection<ObjectTreeListItem>();

        private Scene _hostScene;
        private Scene HostScene
        {
            get
            {
                if(_hostScene==default)
                {
                    _hostScene = App.Container.Resolve<EngineProject>()
                        .Scenes
                        .FirstOrDefault(s => s?.StructObjects?.Contains(this) ?? false);
                }

                return _hostScene;
            }
        }

        [Title("Переместить выше")]
        [Visible]
        public void Up()
        {
            var thisIndx = HostScene.StructObjects.IndexOf(this);
            if (thisIndx == 0)
                return;

            HostScene.StructObjects.Remove(this);
            HostScene.StructObjects.Insert(thisIndx - 1, this);
        }

        [Title("Переместить ниже")]
        [Visible]
        public void Down()
        {
            var thisIndx = HostScene.StructObjects.IndexOf(this);
            if (thisIndx == HostScene.StructObjects.Count-1)
                return;

            HostScene.StructObjects.Remove(this);
            HostScene.StructObjects.Insert(thisIndx + 1, this);
        }
    }
}