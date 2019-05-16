namespace Rogue.Drawing.SceneObjects.Dialogs.Origin
{
    using Rogue.Drawing.SceneObjects.Base;
    using System;

    public class Scrollbar : ColoredRectangle
    {
        public Scrollbar(Func<bool> OnUp, Func<bool> OnDown)
        {
            Color = ConsoleColor.Black;
            Depth = 1;
            Fill = true;
            Opacity = 0.5;
            Round = 5;

            Height = 13.5;
            Width = 1;

            this.Up = new Arrow(OnUp, "▲") { Top = -.35 };
            this.Down = new Arrow(OnDown, "▼") { Top = this.Top + this.Height - 1.2, };

            this.AddChild(Up);
            this.AddChild(Down);
        }

        public Arrow Up { get; set; }

        public Arrow Down { get; set; }
    }
}
