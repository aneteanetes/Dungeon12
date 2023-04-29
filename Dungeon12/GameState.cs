namespace Dungeon12
{
    internal class GameState
    {
        public int RegionId { get; set; }

        public int PointId { get; set; } = 1;

        public ulong GlobalId { get; set; }

        public bool IsBattle { get; set; }
    }
}
