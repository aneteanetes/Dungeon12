namespace Dungeon12.Drawing.SceneObjects.UI
{
    public class HorizontalWindow : Dungeon.Drawing.SceneObjects.ImageControl
    {
        public HorizontalWindow(string img=null) : base(img ?? "Dungeon12.Resources.Images.ui.horizontal(20x13).png")
        {
            //this.Height = 13;
            //this.Width = 20;
        }
    }
}