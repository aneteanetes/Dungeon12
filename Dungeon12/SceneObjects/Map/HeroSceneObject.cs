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

    internal class HeroSceneObject : SceneControl<Hero>
    {
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

        public Square _ImageRegion = new Square()
        {
            Height = 240,
            Width = 240,
            X = 0,
            Y = 0
        };

        public override double Width => 180;

        public override double Height => 180;

        public override Square ImageRegion => _ImageRegion;
    }
}