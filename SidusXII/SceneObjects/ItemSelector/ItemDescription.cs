namespace Dungeon12.Drawing.SceneObjects.Dialogs.Origin
{
    using Dungeon.Drawing;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.GameObjects;
    using Dungeon.SceneObjects;
    using SidusXII;
    using System;

    public class ItemDescription : ColoredRectangle<EmptyGameComponent>
    {
        public ItemDescription(GameEnum origin):base(EmptyGameComponent.Empty)
        {
            Color = ConsoleColor.Black;
            Depth = 1;
            Fill = true;
            Opacity = 0.5;
            Round = 5;

            Height = 13.5*32;
            Width = 13.5*32;

            var title = this.AddTextCenter(new DrawText(origin.Display) { Size = 40 }.Triforce(), true, false);
            title.Top = .5*32;

            var desc = new TextControl(new DrawText(/*origin.Description*/" Description ",wordWrap:true).Montserrat());
            desc.Left = .5*32;
            desc.Top = 2.5*32;
            desc.Width = 12.5*32;

            this.AddChild(desc);

            double top = 10;
            //foreach (var item in perk.Effects)
            //{
            //    var color = new DrawColor(item.Positive ? ConsoleColor.Green : ConsoleColor.Red);

            //    if (perk.Effects.IndexOf(item) == 0)
            //    {
            //        color = new DrawColor(ConsoleColor.Yellow);
            //    }

            //    var perkText = new TextControl(new DrawText(item.Property, color).Montserrat());
            //    perkText.Left = .5;
            //    perkText.Top = top;

            //    top += 1;

            //    this.AddChild(perkText);
            //}
        }
    }
}
