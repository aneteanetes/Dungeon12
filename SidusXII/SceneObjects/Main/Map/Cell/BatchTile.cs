using Dungeon.SceneObjects;

namespace SidusXII.SceneObjects.Main.Map.Cell
{
    public class BatchTile : EmptySceneControl
    {
        private MapCellSceneObject imageTile;

        public BatchTile(MapCellSceneObject imageTile)
        {
            this.imageTile = imageTile;
        }

        public override void Focus()
        {
            imageTile.Focus(true);
        }

        public override void Unfocus()
        {
            imageTile.Unfocus(true);
        }
    }
}
