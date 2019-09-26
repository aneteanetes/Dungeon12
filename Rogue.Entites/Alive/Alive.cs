namespace Rogue.Entites.Alive
{
    /// <summary>
    /// Живой, с уровнем
    /// </summary>
    public class Alive : Drawable
    {
        public int Level { get; set; }

        private long hitPoints;
        public long HitPoints
        {
            get => hitPoints <= 0 ? 0 : hitPoints;
            set
            {
                hitPoints = value;
                if (hitPoints <= 0)
                {
                    Dead = true;
                }
            }
        }
        
        public long MaxHitPoints { get; set; }

        public bool Dead { get; private set; } = false;
    }
}