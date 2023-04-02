using Dungeon.SceneObjects;
using Dungeon12.Entities;
using Dungeon12.Entities.Enums;

namespace Dungeon12.SceneObjects.Stats
{
    internal class InventorySceneObject : SceneControl<Inventory>
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        public InventorySceneObject(Inventory component, bool bindView = true) : base(component, bindView)
        {
            this.Height=358;
            this.Width=285;
        }

        public override void Init()
        {
            this.AddChild(new WearSceneObject(Component.Neck, ItemSlot.Neck));
            this.AddChild(new WearSceneObject(Component.Helm, ItemSlot.Helm) { Left=77 });
            this.AddChild(new WearSceneObject(Component.Mask, ItemSlot.Mask) { Left=153 });
            this.AddChild(new WearSceneObject(Component.Trinket, ItemSlot.Trinket) { Left=229 });

            this.AddChild(new WearSceneObject(Component.Shoulder, ItemSlot.Shoulder) { Top=76 });
            this.AddChild(new WearSceneObject(Component.Cloak, ItemSlot.Cloak) { Top=76, Left=229 });
            this.AddChild(new WearSceneObject(Component.Chest, ItemSlot.Chest) { Top=151 });
            this.AddChild(new WearSceneObject(Component.Legs, ItemSlot.Legs) { Top=151, Left=229 });
            this.AddChild(new WearSceneObject(Component.Gloves, ItemSlot.Gloves) { Top=226 });
            this.AddChild(new WearSceneObject(Component.Boots, ItemSlot.Boots) { Top=226, Left=229 });

            this.AddChild(new WearSceneObject(Component.Ranged, ItemSlot.Ranged) { Top=302 });
            this.AddChild(new WearSceneObject(Component.LeftHand, ItemSlot.LeftHand) { Top=302, Left=77 });
            this.AddChild(new WearSceneObject(Component.RightHand, ItemSlot.RightHand) { Top=302, Left=153 });
            this.AddChild(new WearSceneObject(Component.Ammunition, ItemSlot.Ammunition) { Top=302, Left=229 });
        }
    }
}
