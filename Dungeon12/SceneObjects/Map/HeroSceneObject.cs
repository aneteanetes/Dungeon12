namespace Dungeon12.Drawing.SceneObjects.Map
{
    using Dungeon;
    using Dungeon.Control;
    using Dungeon.Control.Keys;
    using Dungeon.SceneObjects;
    using Dungeon.Types;
    using Dungeon.View;
    using Dungeon12.Entities;
    using System.Collections.Generic;

    public class HeroSceneObject : SceneControl<Hero>
    {
        public override bool Events => true;

        public override bool Shadow => true;

        public override bool Updatable => base.Updatable;

        public override bool CacheAvailable => false;

        protected override ControlEventType[] Handles => new ControlEventType[]
        {
            ControlEventType.Focus,
            ControlEventType.Key,
            ControlEventType.GlobalMouseMove
        };

        public HeroSceneObject(Hero hero) : base(hero)
        {
            if (hero == null)
                return;

            this.Image = hero.WalkSpriteSheet.Image;
            this.ImageRegion = new Rectangle()
            {
                Height = hero.WalkSpriteSheet.Height,
                Width = hero.WalkSpriteSheet.Width,
                X = hero.WalkSpriteSheet.DefaultFramePosition.X,
                Y = hero.WalkSpriteSheet.DefaultFramePosition.Y
            };
        }

        //public override double Left => Component ==null ? 0 : Component.PhysicalObject.Position.X;

        //public override double Top => Component == null ? 0 : Component.PhysicalObject.Position.Y;

        public override double Height => Component == null ? 0 : Component.WalkSpriteSheet.Height;

        public override double Width => Component == null ? 0 : Component.WalkSpriteSheet.Width;
    }
}