namespace Dungeon12.Drawing.SceneObjects.Dialogs.Origin
{
    using Dungeon.Data.Perks;
    using Dungeon.DataAccess;
    using Dungeon.Drawing.Impl;
    using Dungeon.Drawing.SceneObjects.Base;
    using Dungeon.Entites.Alive.Enums;
    using System;
    using System.Linq;

    public class OriginDescription : ColoredRectangle
    {
        public OriginDescription(Origins origin)
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

            var perk = Database.Entity<ValuePerk>(x => x.Identity == origin.ToString()).First();

            var desc = new TextControl(new DrawText(perk.Description) { Size = 20 }.Montserrat());
            desc.Left = .5;
            desc.Top = 2.5;

            this.AddChild(desc);

            double top = 10;
            foreach (var item in perk.Effects)
            {
                var color = new DrawColor(item.Positive ? ConsoleColor.Green : ConsoleColor.Red);
                var perkText = new TextControl(new DrawText(item.Property, color) { Size = 20 }.Montserrat());
                perkText.Left = .5;
                perkText.Top = top;

                top += 1;

                this.AddChild(perkText);
            }
        }
    }
}
