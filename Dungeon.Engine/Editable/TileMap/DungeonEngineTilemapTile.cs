using Dungeon.Utils;
using Dungeon.View.Interfaces;

namespace Dungeon.Engine.Editable.TileMap
{
    public class DungeonEngineTilemapTile : IGameComponent
    {
        [Hidden]
        public string SourceImage { get; set; }

        [Hidden]
        public int XPos { get; set; }

        [Hidden]
        public int YPos { get; set; }

        [Hidden]
        public int OffsetX { get; set; }

        [Hidden]
        public int OffsetY { get; set; }

        [Hidden]
        public DungeonEngineTilemapTileBounds Bounds { get; set; } = new DungeonEngineTilemapTileBounds();

        public DungeonEngineMapObject Object { get; set; }

        [Hidden]
        public ISceneObject SceneObject { get; set; }

        public void SetView(ISceneObject sceneObject)
        {
        }
    }
}