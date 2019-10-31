using Dungeon.Drawing.Impl;
using Dungeon.Drawing.SceneObjects;

namespace Dungeon12.Drawing.SceneObjects
{
    public class Background : Dungeon.Drawing.SceneObjects.ImageControl
    {
        //public override bool AbsolutePosition => true;

        public override bool CacheAvailable => false;

        public Background(bool snow=false) : base("Dungeon12.Resources.Images.d12back.png")
        {
            if (snow)
            {
                this.Width = 1280 / 32;
                this.Height = 800 / 32;
                this.AddChild(new BackgroundSnow());
                this.AddChild(new BackgroundSnow()
                {
                    Left=-3
                });
                this.AddChild(new BackgroundSnow()
                {
                    Left = 25
                });
            }
        }

        private class BackgroundSnow : SceneObject
        {
            public BackgroundSnow()
            {
                this.Width = 1280 / 32;
                this.Height = 800 / 32;
                this.Left = this.Width / 2;
                this.Top = -2;
                this.Effects.Add(new ParticleEffect()
                {
                    Name = "SnowFast",
                    Scale = 0.8
                });
            }
        }
    }
}
