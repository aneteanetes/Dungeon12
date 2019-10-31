namespace Dungeon.Map.Objects
{
    using System;
    using Dungeon.Map.Infrastructure;
    using Dungeon.Types;

    [Template("#")]
    public class Wall : MapObject
    {
        public override string Tileset => "Dungeon.Resources.Images.Tiles.dblue.png";

        public override bool Obstruction { get => true; set { } }

        protected override MapObject Self => this;
    }
}