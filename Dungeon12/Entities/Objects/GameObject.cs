namespace Dungeon12.Entities.Objects
{
    internal class GameObject
    {
        public virtual string Name { get; set; }

        /// <summary>
        /// Absolute path
        /// </summary>
        public virtual string Image { get; set; }

        public virtual string Chip { get; set; }
    }
}
