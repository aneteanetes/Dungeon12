namespace Rogue.Drawing.SceneObjects.Main.Character
{
    public class StatsInfo : HandleSceneControl
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        public StatsInfo()
        {
            this.Image = "Rogue.Resources.Images.ui.stats.png";
            this.Width = 6;
            this.Height = 16;
        }
    }
}
