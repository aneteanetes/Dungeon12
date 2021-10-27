using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using SidusXII.Models.Map;

namespace SidusXII.SceneObjects.Main.Map.Cell
{
    public class PathSelector : SceneObject<MapCellComponent>
    {
        public ImageObject C { get; set; }
        public ImageObject L { get; set; }
        public ImageObject LT { get; set; }
        public ImageObject LB { get; set; }
        public ImageObject R { get; set; }
        public ImageObject RT { get; set; }
        public ImageObject RB { get; set; }

        private string ImgPath(MapCellPart part) => $"GUI/Parts/fog/select/{part}.png".AsmImg();

        public PathSelector(MapCellComponent cell) : base(cell, false)
        {
            typeof(MapCellPart).All<MapCellPart>().ForEach(x =>
            {
                var imageObj = new ImageObject(ImgPath(x))
                {
                    Visible = false,
                    CacheAvailable = false
                };
                this.SetPropertyExpr(x.ToString(), imageObj);
                this.AddChild(imageObj);
            });
        }

        public override double Width => MapSceneObject.TileSize;

        public override double Height => MapSceneObject.TileSize;

        public override bool CacheAvailable => false;

        public void SetEdge(MapCellPart part)
        {
            if (!C.Visible)
                C.Visible = true;

            var img = this.GetPropertyExpr<ImageObject>(part.ToString());
            img.Visible = true;
        }

        public void ResetEdges()
        {
            C.Visible = false;
            L.Visible = false;
            LT.Visible = false;
            LB.Visible = false;
            R.Visible = false;
            RT.Visible = false;
            RB.Visible = false;
        }
    }
}
