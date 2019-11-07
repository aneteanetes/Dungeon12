namespace Dungeon.Drawing.SceneObjects.UI
{
    using Dungeon.Classes;
    using Dungeon.Entities.Alive;

    public abstract class ResourceBar<T> : ResourceBar
        where T: Character
    {
        protected T Player { get; }

        public ResourceBar(T avatar)
        {
            this.Player = avatar;
        }
    }

    public abstract class ResourceBar : Dungeon.Drawing.SceneObjects.ImageControl
    {
        public ResourceBar() : base(null)
        {
        }

        protected abstract string BarTile { get; }

        public override string Image => BarTile;
    }
}