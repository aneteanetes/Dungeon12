using Dungeon;
using Dungeon.SceneObjects;
using Nabunassar.ECS.Components;
using Nabunassar.Entities;
using Nabunassar.Entities.Enums;

namespace Nabunassar.SceneObjects.Stats
{
    internal class WearSceneObject : SceneControl<Item>, ITooltiped
    {
        ItemSlot type;

        public WearSceneObject(Item component, ItemSlot itemType) : base(component)
        {
            type = itemType;
            this.Width = 56;
            this.Height = 56;
        }

        public override string Image { get => Component.Id.IsEmpty() ? null : $"Items/{Component.Image}".AsmImg(); set => base.Image=value; }

        public string TooltipText => Component.Id.IsEmpty() ? Global.Strings[type] : null;
    }
}