namespace Rogue.Transactions
{
    public class Transactional
    {
        public virtual void Commit() { }

        public virtual void Rollback() { }
    }
}