using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Perks;
using System;

namespace Dungeon12.SceneObjects.UserInterface.OriginSelect
{
    public class ZonePerk : EmptySceneControl, ITooltiped
    {
        public ZonePerk()
        {
            this.Width = 50;
            this.Height = 50;

            this.AddChild(new ImageObject("Perks/perkborder.png".AsmImg()));
        }

        public IDrawText TooltipText => GetPerkTooltip();

        public bool ShowTooltip => true;

        private Perk perk;
        public void Bind(Perk perk)
        {
            this.perk = perk;
            //this.Image = perk.Image;
        }

        private IDrawText GetPerkTooltip()
        {
            var str = perk.Name + Environment.NewLine;

            str += perk.Description;

            return str.AsDrawText().Gabriela().InSize(12);
        }
    }
}
