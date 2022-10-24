using Dungeon.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#pragma warning disable IDE1006 // Naming Styles

namespace Dungeon.Tiled
{
    public class TiledObject
    {
        public int id { get; set; }

        public uint gid { get; set; }

        public string file { get; set; }

        public double x { get; set; }

        public double y { get; set; }

        public int width { get; set; }

        public int height { get; set; }

        public bool collide => Properties?.FirstOrDefault(x => x.name == "collide")?.value == "true";

        public string objectgroup { get; set; }

        public List<TiledObjectProperty> Properties { get; set; } = new List<TiledObjectProperty>();

        public T GetPropValue<T>(string propName)
        {
            var p = this.Properties.FirstOrDefault(x => x.name.ToLower() == propName.ToLower());
            if (p == null)
                return default;

            return Convert.ChangeType(p.value, typeof(T)).As<T>();
        }

        public Dot[] Polygon { get; set; } = new Dot[0];

    }

    public class TiledObjectProperty
    {
        public string name { get; set; }

        public string value { get; set; }
    }
}
#pragma warning restore IDE1006 // Naming Styles