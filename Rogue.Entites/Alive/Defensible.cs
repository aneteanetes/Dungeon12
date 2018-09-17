namespace Rogue.Entites.Alive
{
    /// <summary>
    /// Умеет защищаться
    /// </summary>
    public class Defensible : Alive
    {
        public long Defence { get; set; }

        public long Barrier { get; set; }
    }
}
