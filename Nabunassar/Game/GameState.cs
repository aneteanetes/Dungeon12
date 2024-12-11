using Dungeon.Varying;
using Nabunassar.Entities;
using Nabunassar.Entities.Journal;
using Nabunassar.Entities.Map;
using Nabunassar.Entities.Quests;

namespace Nabunassar.Game
{
    internal class GameState
    {
        public Creation Creation { get; set; } = new();

        public World World { get; set; }

        public Party Party { get; set; } = new();

        public QuestBook QuestBook { get; set; }

        public MapRegion MapRegion { get; set; }

        public Region Region { get; set; }

        public Location Location { get; set; }

        public Polygon FocusPolygon { get; set; }

        public void SelectPolygon(Polygon polygon)
        {
            FocusPolygon = polygon;
            OnSelectPolygon?.Invoke(polygon);
        }
        public Action<Polygon> OnSelectPolygon { get; } = delegate { };

        public Calendar Calendar { get; set; } = new Calendar();

        public GameLog Log { get; set; }

        public Variables Variables { get; set; } = new();

        public GameVariables Vars { get; set; } = new();
    }


}
