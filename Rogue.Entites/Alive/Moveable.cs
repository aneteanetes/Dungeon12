namespace Rogue.Entites.Alive
{
    using Rogue.Entites.Animations;

    public class Moveable : Modified
    {
        public virtual AnimationMap Idle { get; set; }

        public virtual AnimationMap MoveUp { get; set; }

        public virtual AnimationMap MoveDown { get; set; }

        public virtual AnimationMap MoveLeft { get; set; }

        public virtual AnimationMap MoveRight { get; set; }
    }
}