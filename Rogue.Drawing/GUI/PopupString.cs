namespace Rogue.Drawing.GUI
{
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.SceneObjects;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;

    public class PopupString : SceneObject
    {
        public override bool CacheAvailable => false;

        public override bool Interface => true;

        public override int Layer { get; set; } = 20;

        private int maxFrames = 0;
        private int frames = 0;

        private double speed;

        public PopupString(string text, ConsoleColor color, Point position, int frames, float size = 10, double speed = 0.2)
            : this(text, new DrawColor(color), position, frames, size, speed) { }

        public PopupString(string text, ConsoleColor color, Point position, bool big=false)
            : this(text, color, position, 25, big ? 19 : 17, 0.06)
        { }

        public PopupString(string text, IDrawColor color, Point position, bool big=false)
            : this(text, color, position, 25, big ? 19 : 17, 0.06)
        { }

        public PopupString(string text, IDrawColor color, Point position, int frames, float size = 10, double speed = 0.2)
        {
            this.maxFrames = frames;
            this.Text = new DrawText(text, color) { Size = size };
            this.Text.FontName = "Montserrat";
            this.Text.FontAssembly = "Rogue.Resources";
            this.Text.FontPath = "Rogue.Resources.Fonts.Mont.otf";

            this.speed = speed;

            this.Left = position.X;
            this.Top = position.Y;

            this.Width = 1;
            this.Height = 1;
        }

        protected Rectangle FramePosition;

        public override Rectangle Position
        {
            get
            {
                FrameCounter++;

                DrawLoop();

                return base.Position;
            }
            
        }

        private int FrameCounter = 0;

        protected void DrawLoop()
        {
            if (frames >= maxFrames)
            {
                this.Destroy?.Invoke();
                return;
            }

            if (((double)frames / (double)maxFrames) * 100 > 20)
            {
                if (this.Text.Opacity > 0)
                {
                    this.Text.Opacity -= 0.005;
                }
            }

            if (FrameCounter % (60 / 14) == 0)
            {
                frames++;
                this.Top -= speed;
            }
        }
    }
}