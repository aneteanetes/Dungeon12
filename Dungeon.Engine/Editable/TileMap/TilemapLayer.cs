using Dungeon.Utils;
using Dungeon.View.Interfaces;
using LiteDB;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dungeon.Engine.Editable.TileMap
{
    public class TilemapLayer : IGameComponent
    {
        public string Name { get; set; }

        [Display(Name="Уровень игрока", Description ="Игрок будет рисоваться на этом уровне")]
        public bool PlayerLevel { get; set; }

        [Hidden]
        public List<TilemapTile> Tiles { get; set; } = new List<TilemapTile>();

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
