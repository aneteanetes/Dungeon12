namespace Dungeon.Drawing
{
    using Dungeon.View.Interfaces;

    public class Light : ILight
    {
        private IDrawColor _color;

        public IDrawColor Color
        {
            get => _color;
            set
            {
                _color = value;
                Updated = true;
            }
        }

        private float _range;

        /// <summary>
        /// Relative range
        /// </summary>
        public float Range
        {
            get => _range;
            set
            {
                _range = value;
                Updated = true;
            }
        }

        public bool Updated { get; set; }
    }
}