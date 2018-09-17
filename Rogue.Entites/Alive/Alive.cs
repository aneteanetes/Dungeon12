namespace Rogue.Entites.Alive
{
    /// <summary>
    /// Живой, с уровнем
    /// </summary>
    public class Alive : Drawable
    {
        public int Level { get; set; }

        public long HitPoints { get; set; }
    }
}