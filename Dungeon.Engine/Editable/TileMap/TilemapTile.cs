using Dungeon.Utils;
using Dungeon.View.Interfaces;

namespace Dungeon.Engine.Editable.TileMap
{
    public class TilemapTile : IGameComponent
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
        public TilemapTileBounds Bounds { get; set; } = new TilemapTileBounds();

        public MapObject Object { get; set; }

        [Hidden]
        public ISceneObject SceneObject { get; set; }

        public void SetView(ISceneObject sceneObject)
        {
        }
    }
}