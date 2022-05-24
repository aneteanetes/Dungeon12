namespace Dungeon12.SceneObjects.Base
{
    using Dungeon.Control;
    using Dungeon.Drawing;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.SceneObjects;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using System;

    internal class Tooltip : DarkRectangle
    {
        public override bool Filtered => false;

        public override bool CacheAvailable => false;

        public override bool Interface => false;

        public Tooltip(string text, Point position, IDrawColor drawColor)
            : this(new DrawText(text, drawColor ?? new DrawColor(ConsoleColor.White))
            {
                Size = 12,
                FontName = "Gabriela"
            }, position)
        { }

        public IDrawText TooltipText { get; set; }

        public override IDrawText Text => txt.Text;

        TextObject txt;

        public Tooltip(IDrawText drawText, Point position, double opacity=.8)
        {
            if (position == default)
            {
                return;
            }

            //Opacity = opacity;

            var textSize = MeasureText(drawText);

            Width = textSize.X + 10;
            Height = textSize.Y + 5;

            this.AddBorder(opacity);

            //var text = txt = AddTextCenter(drawText);
            //text.Filtered = false;

            drawText.SetText(" " + drawText.StringData);

            txt = this.AddText(drawText.SegoeUIBold(), 0, 0);
            TooltipText = txt.Text;

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