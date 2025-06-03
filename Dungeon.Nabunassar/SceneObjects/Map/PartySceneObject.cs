namespace Nabunassar.Drawing.SceneObjects.Map
{
    using Dungeon.Control;
    using Dungeon.SceneObjects;
    using Nabunassar.Entities;

    internal class PartySceneObject : SceneControl<Party>
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
            
        }

        public override double Left => 0; // ((Global.Game?.Location?.Position?.X ?? 0)+417.5d)-30;

        public override double Top => 0; // ((Global.Game?.Location?.Position?.Y ?? 0)+127d)-35;
    }
}