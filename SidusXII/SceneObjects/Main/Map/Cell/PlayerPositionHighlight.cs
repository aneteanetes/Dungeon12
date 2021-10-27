using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using SidusXII.Models.Map;

namespace SidusXII.SceneObjects.Main.Map.Cell
{
    public class PlayerPositionHighlight : SceneObject<MapCellComponent>
    {
        public PlayerPositionHighlight(MapCellComponent component) : base(component, false)
        {
            AddChild(new ImageObject($"GUI/Parts/fog/select/C.png".AsmImg())
            {
                Width = Width,
                Height = Height,
                CacheAvailable = false
            });
            AddChild(new ImageObject("GUI/Parts/playercell.png".AsmImg())
            {
                Width = Width,
                Height = Height,
                CacheAvailable = false
            });
        }

        public override double Width => MapSceneObject.TileSize;

        public override double Height => MapSceneObject.TileSize;

        public override bool Visible => Component.Player;

        public override bool CacheAvailable => false;
    }
}
