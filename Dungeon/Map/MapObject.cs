namespace Dungeon.Map
{
    using Dungeon.Data;
    using Dungeon.Data.Attributes;
    using Dungeon.Data.Region;
    using Dungeon.Entities.Animations;
    using Dungeon.Game;
    using Dungeon.Map.Infrastructure;
    using Dungeon.Merchants;
    using Dungeon.Physics;
    using Dungeon.Transactions;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [DataClass(typeof(RegionPart))]
    public class MapObject : PhysicalObject<MapObject>, IGameComponent
    {
        public GameMap Gamemap { get; set; }

        public Action Die;

        public Action Destroy { get; set; }

        public virtual bool Obstruction { get; set; }

        public bool Animated => this.Animation != null;

        public virtual AnimationMap Animation { get; }

        public Point Location { get; set; }

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
            };

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

        private static readonly Dictionary<string, (Type type, Type dataclass)> TypeCache = new Dictionary<string, (Type type, Type dataclass)>();

        public static MapObject Create(RegionPart regionPart)
        {
            if (string.IsNullOrEmpty(regionPart.Icon))
            {
                if (regionPart.Obstruct)
                {
                    regionPart.Icon = "~";
                }
                else
                {
                    regionPart.Icon = ".";
                }
            }

            if (!TypeCache.TryGetValue(regionPart.Icon, out var @class))
            {
                var type = typeof(MapObject).AllAssignedFrom().FirstOrDefault(x =>
                    {
                        var attr = (TemplateAttribute)Attribute.GetCustomAttribute(x, typeof(TemplateAttribute));
                        if (attr == null)
                            return false;

                        return attr.Template == regionPart.Icon;
                    });

                var dataClassAttr = (DataClassAttribute)Attribute.GetCustomAttribute(type, typeof(DataClassAttribute));

                @class = (type, dataClassAttr.DataType);

                TypeCache.Add(regionPart.Icon, (type, dataClassAttr.DataType));
            }

            var mapObject = @class.type.NewAs<MapObject>();
            mapObject.Load(regionPart);

            return mapObject;
        }
        
        public virtual bool CameraAffect { get; set; } = false;

        public Point SceenPosition { get; set; }

        public virtual ISceneObject View(GameState gameState) => default;

        protected virtual void Load(RegionPart regionPart)
        {
            this.Location = new Point(regionPart.Position.X, regionPart.Position.Y);

            if (regionPart.Region == null)
            {
                var measure = Global.DrawClient.MeasureImage(regionPart.Image);
                this.Size = new PhysicalSize
                {
                    Width = measure.X,
                    Height = measure.Y
                };
            }
        }

        protected TPersist LoadData<TPersist>(object dataClass) where TPersist : Persist
        {
            if (dataClass is RegionPart regionPart)
            {
                return Database.Entity<TPersist>(x => x.IdentifyName == regionPart.IdentifyName).FirstOrDefault();
            }

            return default;
        }

        public Merchant Merchant { get; set; }

        public virtual double MovementSpeed { get; set; } = 0.04;

        public virtual Point VisionMultiple { get; set; } = new Point(1.2, 1.2);

        public MapObject Vision => new MapObject
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
