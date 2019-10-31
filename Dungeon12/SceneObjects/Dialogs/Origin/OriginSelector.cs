namespace Dungeon12.Drawing.SceneObjects.Dialogs.Origin
{
    using Dungeon.Control.Pointer;
    using Dungeon.Drawing.Impl;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.Entites.Alive.Enums;
    using System;using Dungeon;using Dungeon.Drawing.SceneObjects;

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

            this.AddChild(new ImageControl($"Dungeon.Resources.Images.Origin.{origin.ToString()}.png"));
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