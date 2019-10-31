namespace Dungeon.Map.Objects
{
    using Dungeon.Map.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Text;

    [Template("~")]
    public class Obstruct : MapObject
    {
        public override bool Obstruction { get => true; set { } }

        protected override MapObject Self => this;
    }
}
