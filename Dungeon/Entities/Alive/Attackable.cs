using Dungeon.Entities.Animations;

namespace Dungeon.Entities.Alive
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