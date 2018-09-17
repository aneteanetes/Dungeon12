namespace Rogue.Entites.Alive
{
    /// <summary>
    /// Умеет атаковать
    /// </summary>
    public class Attackable : Defensible
    {
        public long MinDMG { get; set; }

        public long MaxDMG { get; set; }
    }
}