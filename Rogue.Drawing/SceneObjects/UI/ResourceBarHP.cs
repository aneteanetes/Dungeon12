namespace Rogue.Drawing.SceneObjects.UI
{
    using Rogue.Entites.Alive.Character;

    public class ResourceBarHP : ResourceBar<Player>
    {
        public ResourceBarHP(Player avatar) : base(avatar)
        {
        }

        protected override string BarTile => "Rogue.Resources.Images.ui.player.hp.png";

        public override double Width
        {
            get => (0.8 * ((double)Player.HitPoints / ((double)Player.MaxHitPoints) * 100)) / 100;
            set { }
        }

        public override bool CacheAvailable => false;
    }
}