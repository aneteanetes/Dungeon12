namespace Dungeon.Entities.Enemy
{
    using Dungeon.Entities.Alive;
    using Dungeon.Types;

    public class Enemy : Moveable
    {
        public bool Aggressive { get; set; }

        public string DieImage { get; set; }

        public Rectangle DieImagePosition { get; set; }
    }
}