using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Physics
{
    public class PhysicalObjectProjection
    {
        public virtual PhysicalSize Size { get; set; }

        public virtual PhysicalPosition Position { get; set; }

        public PhysicalObject PhysicalObject => new PhysicalObject() { Size = this.Size, Position = this.Position };
    }
}
