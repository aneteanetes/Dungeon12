using Dungeon.Data;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Engine.Projects
{
    public class DungeonEngineScene : Persist
    {
        public string Name { get; set; }

        [BsonIgnore]
        public string Text => Name;

        public List<object> SceneObjects { get; set; }
    }
}
