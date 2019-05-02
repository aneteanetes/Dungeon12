﻿namespace Rogue.Drawing.SceneObjects.UI
{
    using Rogue.Entites.Alive;

    public class ObjectHpBar : ImageControl
    {
        private Alive alive;

        public ObjectHpBar(Alive alive)
            :base("Rogue.Resources.Images.GUI.hpbar.png")
        {
            this.Top += 0.025;
            this.Left += 0.05;
            this.Height = 0.19;
            this.alive = alive;
        }

        public override double Width
        {
            get => (0.8 * ((double)alive.HitPoints / ((double)alive.MaxHitPoints) * 100)) / 100;
            set { }
        }

        public override bool CacheAvailable => false;
    }

    public class ObjectHpBarBack : ImageControl
    {
        public ObjectHpBarBack(Alive alive)
            : base("Rogue.Resources.Images.GUI.hpbar_e.png")
        {
            this.Top -= 0.23;
            this.Left += 0.06;
            this.Height = 0.25;
            this.Width = 0.88;

            this.AddChild(new ObjectHpBar(alive));
        }

        public override bool CacheAvailable => false;
    }
}