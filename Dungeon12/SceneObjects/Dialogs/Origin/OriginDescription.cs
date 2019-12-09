namespace Dungeon12.Drawing.SceneObjects.Dialogs.Origin
{
    using Dungeon;
    using Dungeon.Data;
    using Dungeon12.Data.Perks;
    using Dungeon.Drawing;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon12.Entities.Alive.Enums;
    using Dungeon.GameObjects;
    using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
    using System;
    using System.Linq;

    public class OriginDescription : ColoredRectangle<EmptyGameComponent>
    {
        public OriginDescription(Origins origin):base(EmptyGameComponent.Empty)
        {
            Color = ConsoleColor.Black;
            Depth = 1;
            Fill = true;
            Opacity = 0.5;
            Round = 5;

            Height = 13.5;
            Width = 13.5;

            var title = this.AddTextCenter(new DrawText(origin.ToDisplay()) { Size = 40 }, true, false);
            title.Top = .5;

            var perk = Dungeon.Store.Entity<ValuePerk>(x => x.Identity == origin.ToString()).First();

            var desc = new TextControl(new DrawText(perk.Description,wordWrap:true).Montserrat());
            desc.Left = .5;
            desc.Top = 2.5;
            desc.Width = 12.5;

            this.AddChild(desc);

            double top = 10;
            foreach (var item in perk.Effects)
            {
                var color = new DrawColor(item.Positive ? ConsoleColor.Green : ConsoleColor.Red);

                if (perk.Effects.IndexOf(item) == 0)
                {
                    color = new DrawColor(ConsoleColor.Yellow);
                }

                var perkText = new TextControl(new DrawText(item.Property, color).Montserrat());
                perkText.Left = .5;
                perkText.Top = top;

                top += 1;

                this.AddChild(perkText);
            }
        }
    }
}
