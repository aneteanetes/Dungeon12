namespace Rogue.Drawing.SceneObjects.Dialogs.Origin
{
    using Rogue.Control.Pointer;
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.SceneObjects.Base;
    using Rogue.Entites.Alive.Enums;
    using System;

    public class OriginSelector : ColoredRectangle
    {
        private readonly Action<Origins> select;
        public readonly Origins origin;

        public OriginSelector(Origins origin, Action<Origins> select)
        {
            this.origin = origin;
            this.select = select;

            Color = ConsoleColor.Black;
            Depth = 1;
            Fill = true;
            Opacity = 0.5;
            Round = 5;

            this.AddChild(new ImageControl($"Rogue.Resources.Images.Origin.{origin.ToString()}.png"));
            this.Height = 3;
            this.Width = 8;
            this.AddTextCenter(new DrawText(origin.ToDisplay())
            {
                Size =25
            });
        }
       
        public override void Click(PointerArgs args)
        {
            select?.Invoke(origin);
        }
    }
}