namespace Rogue.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Rogue.Entites.Animations;
    using Rogue.Map.Infrastructure;
    using Rogue.Types;
    using Rogue.Utils.ReflectionExtensions;
    using Rogue.View.Interfaces;

    public abstract class MapObject : IDrawable
    {
        public virtual bool Obstruction { get; set; }

        public virtual string Icon { get; set; }
        public string Name { get; set; }
        public IDrawColor BackgroundColor { get; set; }
        public IDrawColor ForegroundColor { get; set; }

        public bool Animated => this.Animation != null;

        public virtual AnimationMap Animation { get; }

        public virtual string Tileset { get; set; }

        public virtual Rectangle TileSetRegion { get; set; }

        public virtual Rectangle Region { get; set; }

        public Point Location { get; set; }

        public bool Container => false;

        public abstract void Interact();

        private static readonly Dictionary<string, Type> TypeCache = new Dictionary<string, Type>();
        public static MapObject Create(string icon)
        {
            if (!TypeCache.TryGetValue(icon, out var type))
            {
                type = icon.GetTypeFromAssembly("Rogue.Map", (_icon, asm) =>
                 {
                     return asm.GetTypes().Where(x => typeof(MapObject).IsAssignableFrom(x))
                     .FirstOrDefault(x =>
                     {
                         var attr = (TemplateAttribute)Attribute.GetCustomAttribute(x, typeof(TemplateAttribute));
                         if (attr == null)
                             return false;

                         return attr.Template == icon;
                     });
                 });

                TypeCache.Add(icon, type);
            }

            var mapObject =(MapObject)type.New();
            mapObject.Icon = icon;

            return mapObject;
        }
    }
}