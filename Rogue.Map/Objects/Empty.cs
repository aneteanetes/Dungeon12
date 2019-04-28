namespace Rogue.Map.Objects
{
    using System;
    using Rogue.Map.Infrastructure;
    using Rogue.Types;

    [Template(".")]
    public class Empty : MapObject
    {
        public Empty()
        {
            var randomX = Rogue.Random.Next(0, 8);
            this.region = new Rectangle
            {
                X = 24 * randomX,
                Y = 0,
                Height = 24,
                Width = 24
            };
        }

        public override string Icon { get => "."; set { } }

        public override string Tileset => "Rogue.Resources.Images.Tiles.dblue.png";

        private readonly Rectangle region;
        public override Rectangle TileSetRegion => region;

        protected override MapObject Self => throw new NotImplementedException();

        public override void Interact(GameMap gameMap)
        {
            throw new NotImplementedException();
        }
    }
}