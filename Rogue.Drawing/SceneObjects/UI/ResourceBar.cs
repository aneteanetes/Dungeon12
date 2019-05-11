namespace Rogue.Drawing.SceneObjects.UI
{
    using Rogue.Entites.Alive.Character;

    public abstract class ResourceBar : ImageControl
    {
        protected Player Player { get; }

        public ResourceBar(Player avatar)
            : base(null)
        {
            this.Player = avatar;
        }

        protected abstract string BarTile { get; }

        public override string Image => BarTile;
    }
}