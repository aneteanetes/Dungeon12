using Dungeon;
using Dungeon.GameObjects;
using InTheWood.Entities.Abilities;
using System.Linq;

namespace InTheWood.Entities.MapScreen
{
    public class Sector : GameComponent
    {
        public Sector()
        {
            this.Segments = Enumerable.Range(0, 7).Select(_ => new Segment()).ToArray();
        }

        public MapStatus Status
        {
            get
            {
                var f = Segments.Count(x => x.Status == MapStatus.Friendly);
                var h = Segments.Count(x => x.Status == MapStatus.Hostile);

                if (f == Segments.Count())
                {
                    return MapStatus.Friendly;
                }

                if (h == Segments.Count())
                {
                    return MapStatus.Hostile;
                }

                if (f + h == 0)
                    return MapStatus.Neutral;
                else
                    return MapStatus.Conflict;
            }
            set
            {
                Segments.ForEach(s => s.Status = value);
            }
        }

        public Ability Ability { get; set; }

        public Segment[] Segments { get; set; }
    }
}