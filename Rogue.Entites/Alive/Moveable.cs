namespace Rogue.Entites.Alive
{
    using Rogue.Entites.Animations;
    using Rogue.Physics;
    using Rogue.Types;
    using System.Numerics;

    public class Moveable : Modified
    {
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