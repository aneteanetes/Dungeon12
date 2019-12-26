namespace Dungeon12.Map.Objects
{
    using Dungeon12.Classes;
    using Dungeon12.Map.Infrastructure;
    using Dungeon.Physics;
    using Dungeon.Types;
    using System;
    using Dungeon.Scenes.Manager;
    using Dungeon12.Scenes.Menus;

    [Template("@")]
    public class Avatar : EntityMapObject<Character>
    {
        public override double MovementSpeed { get; set; } = 0.04;

        public override bool CameraAffect { get; set; } = true;

        public Character Character { get; set; }

        public override string Tileset => Character.Tileset;

        public override Rectangle TileSetRegion => Character.TileSetRegion;

        public override string Icon { get => "@"; set { } }

        public override bool Obstruction => true;

        public Direction VisionDirection { get; set; }

        public Avatar(Character character):base(character)
        {
            Character = character;
            this.ForegroundColor = new MapObjectColor
            {
                R = 255,
                A = 255
            };
        }

        public override PhysicalSize Size
        {
            get => new PhysicalSize
            {
                Height = 16,
                Width = 16
            };
            set { }
        }

        public override PhysicalPosition Position
        {
            get => new PhysicalPosition
            {
                X = base.Position.X + 8,
                Y = base.Position.Y + 14
            };
            set { }
        }

        protected override MapObject Self => this;

        public bool SafeMode { get; set; }

        public Action OnMove { get; set; }

        public Action<Direction> OnMoveStop { get; set; }

        public void MoveStep(bool forward) { }
    }
}