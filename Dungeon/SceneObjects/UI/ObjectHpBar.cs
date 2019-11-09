namespace Dungeon.Drawing.SceneObjects.UI
{
    using Dungeon.Entities.Alive;
    using Dungeon.Proxy;
    using System;

    public class ObjectHpBar : Dungeon.Drawing.SceneObjects.ImageControl
    {
        public ObjectHpBar(Alive alive)
            : base("Dungeon12.Resources.Images.GUI.hpbar_e.png")
        {
            this.Top -= 0.23;
            this.Left += 0.06;
            this.Height = 0.25;
            this.Width = 0.88;

            this.AddChild(new ObjectHpBarGreen(alive));
        }

        public override bool CacheAvailable => false;

        private class ObjectHpBarGreen : Dungeon.Drawing.SceneObjects.ImageControl
        {
            private Alive alive;

            public ObjectHpBarGreen(Alive alive)
                : base("Dungeon12.Resources.Images.GUI.hpbar.png")
            {
                this.Top += 0.025;
                this.Left += 0.05;
                this.Height = 0.19;
                this.alive = alive;
            }

            public override double Width
            {
                get => (0.8 * (alive.HitPoints / ((double)alive.MaxHitPoints) * 100)) / 100;
                set { }
            }

            public override bool CacheAvailable => false;
        }
    }
}