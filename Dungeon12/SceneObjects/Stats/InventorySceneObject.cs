using Dungeon.SceneObjects;
using Dungeon12.Entities;
using Dungeon12.Entities.Enums;

namespace Dungeon12.SceneObjects.Stats
{
    internal class InventorySceneObject : SceneControl<Inventory>
    {
        public InventorySceneObject(Inventory component, bool bindView = true) : base(component, bindView)
        {
            this.Height=358;
            this.Width=285;
        }

        public override void Init()
        {
            this.AddChild(new WearSceneObject(Component.Neck, ItemType.Neck));
            this.AddChild(new WearSceneObject(Component.Helm, ItemType.Helm) { Left=77 });
            this.AddChild(new WearSceneObject(Component.Mask, ItemType.Mask) { Left=153 });
            this.AddChild(new WearSceneObject(Component.Trinket, ItemType.Trinket) { Left=229 });

            this.AddChild(new WearSceneObject(Component.Shoulder, ItemType.Shoulder) { Top=76 });
            this.AddChild(new WearSceneObject(Component.Cloak, ItemType.Cloak) { Top=76, Left=229 });
            this.AddChild(new WearSceneObject(Component.Chest, ItemType.Chest) { Top=151 });
            this.AddChild(new WearSceneObject(Component.Legs, ItemType.Legs) { Top=151, Left=229 });
            this.AddChild(new WearSceneObject(Component.Gloves, ItemType.Gloves) { Top=226 });
            this.AddChild(new WearSceneObject(Component.Boots, ItemType.Boots) { Top=226, Left=229 });

            this.AddChild(new WearSceneObject(Component.Ranged, ItemType.Ranged) { Top=302 });
            this.AddChild(new WearSceneObject(Component.LeftHand, ItemType.LeftHand) { Top=302, Left=77 });
            this.AddChild(new WearSceneObject(Component.RightHand, ItemType.RightHand) { Top=302, Left=153 });
            this.AddChild(new WearSceneObject(Component.Ammunition, ItemType.Ammunition) { Top=302, Left=229 });
        }
    }
}
