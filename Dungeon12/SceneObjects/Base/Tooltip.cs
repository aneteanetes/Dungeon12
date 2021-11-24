namespace Dungeon12.SceneObjects.Base
{
    using Dungeon.Control;
    using Dungeon.Drawing;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using System;

    public class Tooltip : DarkRectangle
    {
        public override bool Filtered => false;

        public override bool CacheAvailable => false;

        public override bool Interface => true;

        public Tooltip(string text, Point position, IDrawColor drawColor)
            : this(new DrawText(text, drawColor ?? new DrawColor(ConsoleColor.White))
            {
                Size = 12,
                FontName = "Gabriela"
            }, position)
        { }

        public IDrawText TooltipText { get; set; }

        public Tooltip(IDrawText drawText, Point position)
        {
            if (position == default)
            {
                return;
            }

            Opacity = 0.8;

            var textSize = MeasureText(drawText);

            Width = textSize.X+10;
            Height = textSize.Y+5;

            var text = AddTextCenter(drawText);
            text.Filtered = false;

            TooltipText = text.Text;

            //base.Left = position.X - Width / 2;
            //Top = position.Y;
        }

        public void SetPosition(Point position)
        {
            Left = position.X;
            Top = position.Y;
        }

        //public override double Left
        //{
        //    get => base.Left;
        //    set => base.Left = value - Width / 2;
        //}

        protected override ControlEventType[] Handles => new ControlEventType[] { };
    }
}