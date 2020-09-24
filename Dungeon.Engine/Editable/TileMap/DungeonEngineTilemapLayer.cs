using Dungeon.Utils;
using Dungeon.View.Interfaces;
using LiteDB;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dungeon.Engine.Editable.TileMap
{
    public class DungeonEngineTilemapLayer : IGameComponent
    {
        public string Name { get; set; }

        [Display(Name="Уровень игрока", Description ="Игрок будет рисоваться на этом уровне")]
        public bool PlayerLevel { get; set; }

        [Hidden]
        public List<DungeonEngineTilemapTile> Tiles { get; set; } = new List<DungeonEngineTilemapTile>();

        [Hidden]
        public byte[] Batched { get; set; }

        [Hidden]
        [BsonIgnore]
        public bool BorderMode { get; set; }

        [BsonIgnore]
        [Hidden]
        public ISceneObject SceneObject { get; set; }

        public void SetView(ISceneObject sceneObject)
        {
            SceneObject = sceneObject;
        }
    }
}
