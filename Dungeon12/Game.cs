using Dungeon12.Entities;
using Dungeon12.Entities.Journal;
using Dungeon12.Entities.Map;
using Dungeon12.Entities.Quests;
using Dungeon12.SceneObjects.UI;
using System.Collections.Generic;

namespace Dungeon12
{
    internal class Game
    {
        public Party Party { get; set; }
        
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

        public Calendar Calendar { get; set; }

        public GameLog Log { get; set; }

        public GameState State { get; set; } = new GameState();
    }
}
