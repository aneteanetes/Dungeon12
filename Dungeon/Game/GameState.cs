using Dungeon.Drawing.SceneObjects.Map;
using Dungeon.Map;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Game
{
    public class GameState
    {
        public GameMap Map { get; set; }

        public PlayerSceneObject Player { get; set; }
    }
}
