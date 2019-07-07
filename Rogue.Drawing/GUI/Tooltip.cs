namespace Rogue.Drawing.GUI
{
    using Rogue.Control.Events;
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.SceneObjects.Base;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;

    public class Tooltip : DarkRectangle
    {
        public override bool CacheAvailable => false;

        public override bool Interface => true;

        public Tooltip(string text, Point position, IDrawColor drawColor)
        {
            var drawText = new DrawText(text, drawColor ?? new DrawColor(ConsoleColor.White)) { Size = 12 };
            drawText.FontName = "Montserrat";

            Opacity = 0.8;


            var textSize = this.MeasureText(drawText);

            this.Width = (textSize.X / 32) + 0.5;
            this.Height = textSize.Y / 32;

            this.AddTextCenter(drawText);

            base.Left = position.X - this.Width / 2.4;
            this.Top = position.Y;
        }

        public override double Left
        {
            get => base.Left;
            set => base.Left = value - this.Width / 2.4;
        }

        protected override ControlEventType[] Handles => new ControlEventType[] { };
    }
}