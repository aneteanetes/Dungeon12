namespace Rogue.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Rogue.Entites.Animations;
    using Rogue.Map.Infrastructure;
    using Rogue.Physics;
    using Rogue.Types;
    using Rogue.Utils.ReflectionExtensions;
    using Rogue.View.Interfaces;

    public class MapObject : PhysicalObject<MapObject>, IDrawable
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

        public virtual void Interact(GameMap gameMap) { }

        public virtual PhysicalSize View => new PhysicalSize
        {
            Height = this.Region.Height / 2,
            Width = this.Region.Width / 2
        };

        protected virtual PhysicalSize _Size { get; set; }

        public override PhysicalSize Size
        {
            get => _Size ?? new PhysicalSize
            {
                Height = 32,
                Width = 32
            }
;

            set => _Size = value;
        }

        protected virtual PhysicalPosition _Position { get; set; }

        public override PhysicalPosition Position
        {
            get => _Position ?? new PhysicalPosition
            {
                X = this.Location.X * 32,
                Y = this.Location.Y * 32
            }
;

            set => _Position = value;
        }

        protected override MapObject Self => this;

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

            var mapObject = (MapObject)type.New();
            mapObject.Icon = icon;

            return mapObject;
        }

        public virtual bool CameraAffect => false;

        public virtual double MovementSpeed => 0.04;

        public override MapObject Vision
        {
            get => new MapObject()
            {
                Size = View,
                Position = new PhysicalPosition
                {
                    X = Position.X + Position.X / 2,
                    Y = Position.Y + Position.Y / 2
                }
            };

            set { }
        }
    }
}