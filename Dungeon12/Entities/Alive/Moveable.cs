namespace Dungeon12.Entities.Alive
{
    using Dungeon.Entities.Animations;
    using Dungeon12.Entities.Alive;
    using Dungeon.Physics;
    using Dungeon.Types;
    using System.Numerics;

    public class Moveable : Interactable
    {
        /// <summary>
        /// Тащемта, это костыль, но для прототипа нормально
        /// </summary>
        public bool Static { get; set; }

        public virtual Animation Idle { get; set; }

        public virtual Animation MoveUp { get; set; }

        public virtual Animation MoveDown { get; set; }

        public virtual Animation MoveLeft { get; set; }

        public virtual Animation MoveRight { get; set; }

        public virtual Animation MoveUpLeft { get; set; }

        public virtual Animation MoveUpRight { get; set; }

        public virtual Animation MoveDownLeft { get; set; }

        public virtual Animation MoveDownRight { get; set; }

        public virtual PhysicalObject MoveRegion { get; set; }

        public virtual Range WalkDistance { get; set; }

        public virtual Rectangle DefaultFramePosition { get; set; }

        public virtual double WalkChance { get; set; }

        public virtual int WaitTime { get; set; }

        public virtual double Speed { get; set; } = 0.025;
    }
}