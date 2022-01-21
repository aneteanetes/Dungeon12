namespace Dungeon12.Drawing.SceneObjects.Map
{
    using Dungeon;
    using Dungeon.Control;
    using Dungeon.Control.Keys;
    using Dungeon.SceneObjects;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using Dungeon12.Entities;
    using System.Collections.Generic;

    public class PartySceneObject : SceneControl<Party>
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

        private HeroSceneObject Hero1 { get; set; }

        /// <summary>
        /// left from main
        /// </summary>
        private HeroSceneObject Hero2 { get; set; }

        /// <summary>
        /// Right from main
        /// </summary>
        private HeroSceneObject Hero3 { get; set; }

        /// <summary>
        /// Always back
        /// </summary>
        private HeroSceneObject Hero4 { get; set; }

        private HeroSceneObject[] Heroes { get; set; }

        public PartySceneObject(Party party) : base(party)
        {
            Hero1 = new HeroSceneObject(party.Hero1)
            {
                LayerLevel = 4
            };
            Hero2 = new HeroSceneObject(party.Hero2)
            {
                LayerLevel = 1
            };
            Hero3 = new HeroSceneObject(party.Hero3)
            {
                LayerLevel = 2
            };
            Hero4 = new HeroSceneObject(party.Hero4)
            {
                LayerLevel = 3
            };

            this.AddChild(Hero2);
            this.AddChild(Hero3);
            this.AddChild(Hero4);
            this.AddChild(Hero1);

            Heroes = new HeroSceneObject[] { Hero1, Hero2, Hero3, Hero4 };
        }

        public override double Left => /*(Global.Game?.Region?.PositionVisual?.X ?? 0) +*/ (Global.Game?.Location?.Position?.X ??0) + (25);

        public override double Top => (Global.Game?.Region?.PositionVisual?.Y ?? 0) + (Global.Game?.Location?.Position?.Y ?? 0) - (250 / 3);
    }
}