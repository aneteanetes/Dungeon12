using Dungeon.Map;
using Dungeon.Network;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Entities
{
    public class Entity : NetObject
    {
        public ISceneObject SceneObject { get; set; }

        public MapObject MapObject { get; set; }
    }
}
