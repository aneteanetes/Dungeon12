namespace Rogue.Entites.Alive
{
    using Rogue.Entites.Animations;

    public class Moveable : Modified
    {
        public virtual AnimationMap MoveUp { get => default; }

        public virtual AnimationMap MoveDown { get => default; }

        public virtual AnimationMap MoveLeft { get => default; }

        public virtual AnimationMap MoveRight { get => default; }
    }
}