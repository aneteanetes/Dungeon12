using Dungeon;
using Dungeon.SceneObjects;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Enums;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12.SceneObjects.UserInterface.FractionSelect
{
    public class IconEnumBadge : EmptySceneControl, ITooltipedCustom
    {
        public bool ShowTooltip => true;

        private string title;

        public IconEnumBadge(Roles role) : this()
        {
            title = role.ToDisplay();
            this.Image = $"Icons/Roles/{role}.png".AsmImg();
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
            return new GameTooltip(title, "", nodesc: true);
        }
    }
}
