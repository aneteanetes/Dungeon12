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
        }

        public override string Image => Component?.Chip;

        public Rectangle _ImageRegion = new Rectangle()
        {
            Height = 240,
            Width = 240,
            X = 0,
            Y = 0
        };
        public override Rectangle ImageRegion => _ImageRegion;
    }
}