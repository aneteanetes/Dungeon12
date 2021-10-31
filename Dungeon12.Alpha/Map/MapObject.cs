namespace Dungeon12.Map
{
    using Dungeon.Data;
    using Dungeon.Data.Attributes;
    using Dungeon12.Data.Region;
    using Dungeon.Entities;
    using Dungeon12.Game;
    using Dungeon12.Map.Infrastructure;
    using Dungeon12.Merchants;
    using Dungeon.Physics;
    using Dungeon.Transactions;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Dungeon;
    using Dungeon12.Entities;
    using Dungeon12.Loot;
    using Dungeon12.Map.Objects;
    using Force.DeepCloner;
    using Dungeon.View;

    [DataClass(typeof(RegionPart))]
    public class MapObject : PhysicalObject<MapObject>, IGameComponent
    {
        public MapObject()
        {
            this.Destroy += () => Destroyed = true;
        }

        /// <summary>
        /// Метод перезагрузки сущности, как правило для сохраняемых
        /// </summary>
        public virtual void Reload() { }

        /// <summary>
        /// Метод перезагрузки сущности, как правило для сохраняемых
        /// </summary>
        public virtual void Reload(HashSet<MapObject> objects) { }

        public virtual EntityFraction BindedEntity => default;

        /// <summary>
        /// Признак указывающий что этот объект сохраняемый
        /// <para>
        /// Желательно что бы это работало только для типа, а не для экземпляров
        /// </para>
        /// </summary>
        public virtual bool Saveable => false;

        public GameMap Gamemap { get; set; }

        public Action Die;

        public Action Destroy { get; set; }

        public bool Destroyed { get; set; }

        public virtual bool Obstruction { get; set; }

        public bool Animated => this.Animation != null;

        public virtual Animation Animation { get; set; }

        public Point Location { get; set; }

        public virtual bool Interactable { get; set; }

        /// <summary>
        /// Dynamic dispatch
        /// </summary>
        /// <param name="target"></param>
        public virtual void Interact(object target) => CallInteract(target as dynamic);

        protected virtual void CallInteract(dynamic obj) => Interact(obj);

        protected virtual PhysicalSize _Size { get; set; }

        public override PhysicalSize Size
        {
            get
            {
                if (_Size == default)
                {
                    _Size = new PhysicalSize
                    {
                        Height = 32,
                        Width = 32
                    };
                }

                return _Size;
            }

            set => _Size = value;
        }

        protected virtual PhysicalPosition _Position { get; set; }

        public override PhysicalPosition Position
        {
            get
            {
                if(_Position==default)
                {
                    _Position = new PhysicalPosition
                    {
                        X = (this.Location?.X ?? 0) * 32,
                        Y = (this.Location?.Y ?? 0) * 32
                    };
                }
                else if (this.Location!=default)
                {
                    _Position.X = (this.Location?.X ?? 0) * 32;
                    _Position.Y = (this.Location?.Y ?? 0) * 32;
                }

                return _Position;
            }

            set => _Position = value;
        }

        protected override MapObject Self => this;

        private static readonly Dictionary<string, (Type type, Type dataclass)> TypeCache = new Dictionary<string, (Type type, Type dataclass)>();

        public static MapObject Create(RegionPart regionPart, bool saveable = true)
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
                        var attr = (TemplateAttribute)Attribute.GetCustomAttribute(x, typeof(TemplateAttribute),true);
                        if (attr == null)
                            return false;

                        return attr.Template == regionPart.Icon;
                    });

                var dataClassAttr = (DataClassAttribute)Attribute.GetCustomAttribute(type, typeof(DataClassAttribute), true);

                @class = (type, dataClassAttr.DataType);

                TypeCache.Add(regionPart.Icon, (type, dataClassAttr.DataType));
            }

            var mapObject = @class.type.NewAs<MapObject>();
            if (!mapObject.Saveable || (mapObject.Saveable && saveable))
            { 
                mapObject.Load(regionPart);
                return mapObject;
            }

            return default;
        }
        
        public virtual bool CameraAffect { get; set; } = false;

        public Point SceenPosition { get; set; }

        protected virtual void Load(RegionPart regionPart)
        {
            if (regionPart.Position != default)
            {
                this.Location = new Point(regionPart.Position.X, regionPart.Position.Y);
            }

            this.Obstruction = regionPart.Obstruct;

            if (regionPart.Region == null)
            {
                if (regionPart.Image != default)
                {
                    this.Image = regionPart.Image;
                    var measure = Global.DrawClient.MeasureImage(regionPart.Image);
                    this.Size = new PhysicalSize
                    {
                        Width = measure.X,
                        Height = measure.Y
                    };
                }
            }
            else if (regionPart.Image == default) // на всякий обезопасить
            {
                this.Size = new PhysicalSize()
                {
                    Height = regionPart.Region.Height*32,
                    Width = regionPart.Region.Width*32
                };
            }
        }

        protected TPersist LoadData<TPersist>(object dataClass) where TPersist : Persist
        {
            if (dataClass is RegionPart regionPart)
            {
                return Dungeon.Store.Entity<TPersist>(x => x.IdentifyName == regionPart.IdentifyName).FirstOrDefault();
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

        public void DropLoot(LootTable lootTable)
        {
            if (lootTable == default)
                return;

            List<MapObject> publishObjects = new List<MapObject>();

            var loot = lootTable.Generate();

            if (loot.Gold > 0)
            {
                var money = new Money() { Amount = loot.Gold };
                money.Location = Gamemap.RandomizeLocation(this.Location.DeepClone());
                money.Destroy += () => Gamemap.MapObject.Remove(money);
                Gamemap.MapObject.Add(money);

                publishObjects.Add(money);
            }

            foreach (var item in loot.Items)
            {
                var lootItem = new Loot()
                {
                    Item = item
                };

                lootItem.Location = Gamemap.RandomizeLocation(Location.DeepClone());
                lootItem.Destroy += () => Gamemap.MapObject.Remove(lootItem);

                publishObjects.Add(lootItem);
            }

            publishObjects.ForEach(Gamemap.PublishObject);
        }
    }
}
