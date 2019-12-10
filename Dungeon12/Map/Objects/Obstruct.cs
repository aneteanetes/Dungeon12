namespace Dungeon12.Map.Objects
{
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.SceneObjects;
    using Dungeon.View.Interfaces;
    using Dungeon12.Map.Infrastructure;
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
