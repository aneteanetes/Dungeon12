using Rogue.Drawing.Impl;

namespace Rogue.Drawing.SceneObjects
{
    public class Background : ImageControl
    {
        //public override bool AbsolutePosition => true;

        public override bool CacheAvailable => false;

        public Background(bool snow=false) : base("Rogue.Resources.Images.d12back.png")
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
