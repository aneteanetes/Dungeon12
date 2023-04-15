namespace Dungeon12.SceneObjects.Base
{
    using Dungeon.Control;
    using Dungeon.Drawing;
    using Dungeon.SceneObjects;
    using Dungeon.SceneObjects.Base;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using System;

    internal class Tooltip : DarkRectangle
    {
        public override bool Filtered => false;

        public override bool CacheAvailable => false;

        public override bool Interface => false;

        public Tooltip(string text, Dot position, IDrawColor drawColor)
            : this(new DrawText(text, drawColor ?? new DrawColor(ConsoleColor.White))
            {
                Size = 20,
                FontName = "Gabriela"
            }, position)
        { }

        public new TextObject Text { get; set; }

        public Tooltip(IDrawText drawText, Dot position, double opacity=.8)
        {
            if (position == default)
            {
                return;
            }

            //Opacity = opacity;

            var textSize = MeasureText(drawText);

            Width = textSize.X+5;
            Height = textSize.Y+5;


            this.AddBorderBack(drawText.Opacity == 1 ? opacity : drawText.Opacity);

            //var text = txt = AddTextCenter(drawText);
            //text.Filtered = false;

            //drawText.SetText(" " + drawText.StringData);

            Text = this.AddTextCenter(drawText.SegoeUIBold());
            //base.Left = position.X - Width / 2;
            //Top = position.Y;
        }

        public void SetPosition(Dot position)
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