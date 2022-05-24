using Dungeon;
using Dungeon.SceneObjects;
using Dungeon12.ECS.Components;
using Dungeon12.Entities;
using Dungeon12.Entities.Enums;

namespace Dungeon12.SceneObjects.Stats
{
    internal class WearSceneObject : SceneControl<Item>, ITooltiped
    {
        ItemType type;

        public WearSceneObject(Item component, ItemType itemType) : base(component)
        {
            type = itemType;
            this.Width = 56;
            this.Height = 56;
        }

        public override string Image { get => Component.Id.IsEmpty() ? null : $"Items/{Component.Image}".AsmImg(); set => base.Image=value; }

        public string TooltipText => Component.Id.IsEmpty() ? Global.Strings[type.ToString()] : null;
    }
}