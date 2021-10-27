namespace Dungeon12.Map.Objects
{
    using System;
    using Dungeon12.Map.Infrastructure;
    using Dungeon.Types;

    [Template("#")]
    public class Wall : MapObject
    {
        public override string Tileset => "Dungeon12.Resources.Images.Tiles.dblue.png";

        public override bool Obstruction { get => true; set { } }

        protected override MapObject Self => this;
    }
}