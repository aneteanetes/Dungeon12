using Dungeon.GameObjects;

namespace InTheWood.Entities.MapScreen
{
    public class Segment : GameComponent
    {
        public Segment() { }

        public Segment(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }

        public int Y { get; set; }


        public bool IsAlive { get; set; }

        public bool Immune { get; set; }

        public MapStatus Status { get; set; }

        public bool IsEdge { get; set; }
    }
}
