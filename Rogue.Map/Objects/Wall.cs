namespace Rogue.Map.Objects
{
    using System;
    using Rogue.Map.Infrastructure;
    using Rogue.Types;

    [Template("#")]
    public class Wall : MapObject
    {
        public override string Tileset => "Rogue.Resources.Images.Tiles.dblue.png";

        public override bool Obstruction { get => true; set { } }

        protected override MapObject Self => this;
    }
}