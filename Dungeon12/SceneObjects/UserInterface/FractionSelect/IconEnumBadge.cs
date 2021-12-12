using Dungeon;
using Dungeon.SceneObjects;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Enums;
using Dungeon12.Entities.FractionPolygons;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12.SceneObjects.UserInterface.FractionSelect
{
    public class IconEnumBadge : EmptySceneControl, ITooltipedCustom
    {
        public bool ShowTooltip => true;

        private string title;
        private string description;

        public IconEnumBadge(FractionAbility ability) : this()
        {
            title = ability.ToDisplay();
            description = "Приносит 1 Влияния каждый ход.\r\n\r\n" + ability.ToValue<string>();
            this.Image = $"Icons/FractionAbilities/{ability}.png".AsmImg();
        }

        public IconEnumBadge(Spec spec) : this()
        {
            title = spec.ToDisplay();
            this.Image = $"Icons/Specs/{spec}.png".AsmImg();
        }

        public IconEnumBadge()
        {
            Width = 50;
            Height = 50;
        }

        public Tooltip GetTooltip()
        {
            return new GameTooltip(title, description, description==null ? 0 : 325, description==null);
        }
    }
}