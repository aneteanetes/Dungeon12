namespace Dungeon.Entites.Alive
{
    using Dungeon.Entites.Animations;
    using Dungeon.Physics;
    using Dungeon.Types;
    using System.Numerics;

    public class Moveable : Modified
    {
        /// <summary>
        /// Тащемта, это костыль, но для прототипа нормально
        /// </summary>
        public bool Static { get; set; }

        public virtual AnimationMap Idle { get; set; }

        public virtual AnimationMap MoveUp { get; set; }

        public virtual AnimationMap MoveDown { get; set; }

        public virtual AnimationMap MoveLeft { get; set; }

        public virtual AnimationMap MoveRight { get; set; }

        public virtual PhysicalObject MoveRegion { get; set; }

        public virtual Range WalkDistance { get; set; }

        public virtual Range WalkChance { get; set; }

        public virtual int WaitTime { get; set; }
    }
}