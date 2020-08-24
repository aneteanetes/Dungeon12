using Dungeon.Data;
using Dungeon.Engine.Utils;
using Dungeon.Utils;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Dungeon.Engine.Projects
{
    public class DungeonEngineScene : Persist
    {
        public string Name { get; set; }

        [BsonIgnore]
        public string Text => Name;

        [Hidden]
        public List<object> SceneObjects { get; set; }

        public bool StartScene { get; set; }
    }
}
