using Nabunassar.Entities.Stats.PrimaryStats;

namespace Nabunassar.Entities.Dices
{
    public class DiceTower
    {
        private Random Random = new Random();
        static readonly object Padlock = new object();

        public int Throw(Rank dice)
        {
            lock (Padlock)
            {
                return Global.Random.Next(1, (int)dice);
            }
        }
    }
}
