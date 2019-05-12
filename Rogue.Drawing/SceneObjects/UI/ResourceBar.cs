namespace Rogue.Drawing.SceneObjects.UI
{
    using Rogue.Entites.Alive.Character;

    public abstract class ResourceBar<T> : ResourceBar
        where T: Player
    {
        protected T Player { get; }

        public ResourceBar(T avatar)
        {
            this.Player = avatar;
        }
    }

    public abstract class ResourceBar : ImageControl
    {
        public ResourceBar() : base(null)
        {
        }

        protected abstract string BarTile { get; }

        public override string Image => BarTile;
    }
}