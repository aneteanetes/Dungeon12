namespace Rogue.Drawing.GUI
{
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.SceneObjects;
    using Rogue.Types;
    using System;

    public class PopupString : SceneObject
    {
        public override bool CacheAvailable => false;

        private int maxFrames = 0;
        private int frames = 0;

        private double speed;

        public PopupString(string text,ConsoleColor color, Point position, int frames, double speed=0.2)
        {
            this.maxFrames = frames;
            this.Text = new DrawText(text, color) { Size = 17 };
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