using Dungeon;
using Dungeon.Drawing;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using System;

namespace Dungeon12.SceneObjects.UserInterface.Common
{
    internal class PopupText : SceneObject<IDrawText>
    {
        public override bool Filtered => false;

        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        public override int LayerLevel { get; set; } = 20;

        private int maxFrames = 0;
        private int frames = 0;

        private double speed;

        public PopupText(string text, ConsoleColor color, Point position, int frames = 25, float size = 12, double speed = 0.06)
            : this(text, new DrawColor(color), position, frames, size, speed) { }

        public PopupText(string text, ConsoleColor color, Point position, bool big = false)
            : this(text, color, position, 25, big ? 14 : 12, 0.06)
        { }

        public PopupText(string text, IDrawColor color, Point position, bool big = false)
            : this(text, color, position, 25, big ? 14 : 12, 0.06)
        { }

        public PopupText(string text, IDrawColor color, Point position, int frames, float size = 10, double speed = 0.06)
            : this(new DrawText(text, color) { Size = size }, position, frames, speed)
        {
        }

        public PopupText(IDrawText drawText, Point position, int frames = 30, double speed = 0.06) : base(drawText)
        {
            maxFrames = frames;
            this.Text = Component;
            this.Text.FontName = "Montserrat";
            this.Text.FontAssembly = "Rogue.Resources";
            this.Text.FontPath = "Rogue.Resources.Fonts.Mont.otf";

            this.speed = speed;

            this.Left = position.X;
            this.Top = position.Y;

            this.Width = 1;
            this.Height = 1;
        }

        private int FrameCounter = 0;

        public TimeSpan Time { get; set; }

        protected void DrawLoop()
        {
            if (frames >= maxFrames)
            {
                this.Destroy();
                return;
            }

            if (frames / (double)maxFrames * 100 > 20)
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

        public override bool Updatable => true;

        public override void InternalUpdate(GameTimeLoop gameTime)
        {
            if (Time == default(TimeSpan) || Time < default(TimeSpan))
            {
                this.Destroy();
                OnAfter?.Invoke();
                return;
            }

            Time -= gameTime.ElapsedGameTime;
            this.Top -= speed;
        }

        public Action OnAfter;

        public void After(Action action) => OnAfter = action;
    }
}