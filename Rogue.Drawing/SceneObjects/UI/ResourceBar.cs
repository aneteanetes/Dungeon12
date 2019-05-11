namespace Rogue.Drawing.SceneObjects.UI
{
    using Rogue.Entites.Alive.Character;

    public abstract class ResourceBar<T> : ImageControl
        where T : Player
    {
        protected T Player { get; }

        public ResourceBar(T avatar)
            :base(null)
        {
            this.Player = avatar;
        }

        protected abstract string BarTile { get; }

        public override string Image => BarTile;
    }
}