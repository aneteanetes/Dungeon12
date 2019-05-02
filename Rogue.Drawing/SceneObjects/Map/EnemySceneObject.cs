namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Entites.Enemy;
    using Rogue.Map.Objects;
    using Rogue.Types;
    using System;

    public class EnemySceneObject : AnimatedSceneObject
    {
        private readonly Mob enemy;

        public EnemySceneObject(Mob mob, Rectangle defaultFramePosition) : base(defaultFramePosition, null)
        {
            this.enemy = mob;
            this.Image = mob.Tileset;
            Left = mob.Location.X;
            Top = mob.Location.Y;
            Width = 1;
            Height = 1;

            this.AddChild(new ObjectHpBar(mob.Enemy));
        }

        protected override void DrawLoop()
        {
        }        
    }
}
