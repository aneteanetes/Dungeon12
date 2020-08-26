using LiteDB;
using System;
using System.Collections.ObjectModel;

namespace Dungeon.Engine.Projects
{
    public class DungeonEngineSceneObjectClass
    {
        public string Name { get; set; }

        public string ClassName { get; set; }

        public Type ClassType { get; set; }
    }
}