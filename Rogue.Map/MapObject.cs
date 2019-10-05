namespace Rogue.Map
{
    using Rogue.Entites.Animations;
    using Rogue.Map.Infrastructure;
    using Rogue.Physics;
    using Rogue.Transactions;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class MapObject : PhysicalObject<MapObject>, IDrawable
    {
        public Action Die;

        public Action Destroy { get; set; }
        
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

        public virtual bool Interactable { get; set; }

        public virtual void Interact(object target) => CallInteract(target as dynamic);

        protected virtual void CallInteract(dynamic obj) => Interact(obj);

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

        public Point SceenPosition { get; set; }

        public virtual double MovementSpeed { get; set; } = 0.04;

        public virtual Point VisionMultiple { get; set; } = new Point(1.2, 1.2);

        public override MapObject Vision => new MapObject
        {
            Position = new PhysicalPosition
            {
                X = this.Position.X - (this.Size.Width * this.VisionMultiple.X)/2,
                Y = this.Position.Y - (this.Size.Height * this.VisionMultiple.Y)/2
            },
            Size = new PhysicalSize
            {
                Width = this.Size.Width * VisionMultiple.X,
                Height = this.Size.Height * VisionMultiple.Y
            }
        };

        public Action<Applicable> StateAdded;

        public Action<Applicable> StateRemoved;

        public List<Applicable> States = new List<Applicable>();

        public void AddState(Applicable buff)
        {            
            buff.Apply(this);
            States.Add(buff);
            StateAdded?.Invoke(buff);
        }

        public void RemoveState(Applicable buff)
        {
            buff.Discard(this);
            States.Remove(buff);
            StateRemoved?.Invoke(buff);
        }
    }
}
