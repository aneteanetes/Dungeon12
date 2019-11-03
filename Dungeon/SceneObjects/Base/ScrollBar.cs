using Dungeon.Drawing.SceneObjects.Dialogs.Origin;

namespace Dungeon.SceneObjects.Base
{
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.SceneObjects.Mixins;
    using System;

    public class Scrollbar : ColoredRectangle, IScrollableMixin
    {
        public Scrollbar(double height, Func<bool> OnUp, Func<bool> OnDown)
        {
            Color = ConsoleColor.Black;
            Depth = 1;
            Fill = true;
            Opacity = 0.5;
            Round = 5;

            Height = height;// 13.5;
            Width = 1;

            Up = new Arrow(OnUp, "▲") { Top = -.35 };
            Down = new Arrow(OnDown, "▼") { Top = Top + Height - 1.2, };

            AddChild(Up);
            AddChild(Down);
        }

        public Arrow Up { get; set; }

        public Arrow Down { get; set; }
    }
}
