using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Perks;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12.SceneObjects.UserInterface.OriginSelect
{
    public class ZonePerk : EmptySceneControl, ITooltipedCustom
    {
        public ZonePerk()
        {
            this.Width = 50;
            this.Height = 50;

            this.AddChild(new ImageObject("Perks/perkborder1.png".AsmImg()));
        }

        public override void Focus()
        {
            base.Focus();
        }

        private Perk perk;
        public void Bind(Perk perk)
        {
            this.perk = perk;
            this.Image = perk?.Icon;
            if (this.Image != default)
                this.Image = $"Perks/{this.Image}".AsmImg();
        }

        public bool ShowTooltip => true;

        public ISceneObject GetTooltip() => new GameTooltip(perk?.Name, perk?.Description, 250);
    }
}
