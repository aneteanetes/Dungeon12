namespace Dungeon.Map.Objects
{
    using System;
    using Dungeon.Data.Attributes;
    using Dungeon.Data.Region;
    using Dungeon.Map.Infrastructure;
    using Dungeon.Types;

    [Template(".")]
    public class Empty : MapObject
    {
        public Empty()
        {
            var randomX = Dungeon.RandomRogue.Next(0, 8);
            this.region = new Rectangle
            {
                X = 24 * randomX,
                Y = 0,
                Height = 24,
                Width = 24
            };
        }

        public override string Icon { get => "."; set { } }

        public override string Tileset => "Dungeon12.Resources.Images.Tiles.dblue.png";

        private readonly Rectangle region;
        public override Rectangle TileSetRegion => region;

        protected override MapObject Self => throw new NotImplementedException();
    }
}