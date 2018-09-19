namespace Rogue.Entites.Alive
{
    using Rogue.Transactions;

    /// <summary>
    /// Изменяемый, постоянно или временно
    /// </summary>
    public class Modified : Capable
    {
        public void Add(Applicable modifier)
        {
            modifier.Apply(this);
        }

        public void Remove(Applicable modifier)
        {
            modifier.Discard(this);
        }
    }
}