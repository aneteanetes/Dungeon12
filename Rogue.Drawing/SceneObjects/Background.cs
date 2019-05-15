namespace Rogue.Drawing.SceneObjects
{
    public class Background : ImageControl
    {
        public override bool AbsolutePosition => true;

        public Background() : base("Rogue.Resources.Images.d12back.png")
        {
            //this.AddChild(new ImageControl("Rogue.Resources.Images.ui.globalborder.png"));
        }
    }
}
