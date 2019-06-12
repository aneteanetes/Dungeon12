namespace Rogue.Entites.Enemy
{
    using Rogue.Entites.Alive;
    using Rogue.Types;

    public class Enemy : Moveable
    {
        public bool Aggressive { get; set; }

        public string DieImage { get; set; }

        public Rectangle DieImagePosition { get; set; }
    }
}