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

        public virtual AnimationMap Idle { get; set; }

        public virtual AnimationMap MoveUp { get; set; }

        public virtual AnimationMap MoveDown { get; set; }

        public virtual AnimationMap MoveLeft { get; set; }

        public virtual AnimationMap MoveRight { get; set; }

        public virtual PhysicalObject MoveRegion { get; set; }

        public virtual Range WalkDistance { get; set; }

        public virtual double WalkChance { get; set; }

        public virtual int WaitTime { get; set; }
    }
}