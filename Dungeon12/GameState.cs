using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12
{
    public class GameState
    {
        public List<string> VisitedLocations { get; set; } = new List<string>();

        public List<string> Dialogues { get; set; } = new List<string>();
    }
}
