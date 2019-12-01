namespace Dungeon12.Map.Objects
{
    using Dungeon.Data;
    using Dungeon.Data.Attributes;
    using Dungeon.Data.Region;
    using Dungeon.Entities.Fractions;
    using Dungeon.Game;
    using Dungeon.Map;
    using Dungeon.Map.Infrastructure;
    using Dungeon.Map.Objects;
    using Dungeon.Physics;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using Dungeon12.Data.Npcs;
    using Dungeon12.Drawing.SceneObjects.Map;
    using Dungeon12.Entities;
    using Force.DeepCloner;
    using System.Collections.Generic;
    using System.Linq;

    [Template("Npc")]
    [DataClass(typeof(NPCData))]
    public class NPCMap : EntityMapObject<NPC>
    {
        public NPCMap() : this(default) { }

        public NPCMap(NPC entity) : base(entity)
        {
            this.Destroy += Dying;
        }

        public override bool Saveable => true;

        public NPC NPC { get; set; }

        public override string Icon { get => "N"; set { } }

        protected override MapObject Self => this;

        public override bool Obstruction => true;

        public bool NoInteract { get; set; }

        public string NoInteractText { get; set; }

        public bool Moving { get; set; }

        public long Exp => 5;

        public bool IsChasing { get; set; }

        public override PhysicalSize Size
        {
            get => new PhysicalSize
            {
                Height = 24,
                Width = 24
            };
            set { }
        }

        public override PhysicalPosition Position
        {
            get => new PhysicalPosition
            {
                X = base.Position.X + 4,
                Y = base.Position.Y + 4
            };
            set { }
        }

        public override ISceneObject Visual(GameState gameState)
        {
            return new NPCSceneObject(gameState.Player, gameState.Map, this, this.TileSetRegion);
        }

        public string IdentifyName { get; set; }

        public Point AttackRangeMultiples { get; set; }

        public bool IsEnemy { get; set; }

        protected override void Load(RegionPart npcData)
        {
            this.IdentifyName = npcData.IdentifyName;

            var data = Database.Entity<NPCData>(x => x.IdentifyName == npcData.IdentifyName).FirstOrDefault();

            this.ReEntity(data.NPC.DeepClone());

            VisionMultiple = data.VisionMultiples;
            AttackRangeMultiples = data.AttackRangeMultiples;

            this.IsEnemy = data.IsEnemy;

            this.Moving = data.Moveable;
            this.Tileset = data.Tileset;
            this.FaceImage = data.Face;
            this.TileSetRegion = data.TileSetRegion;
            this.Name = data.Name;
            this.Size = new PhysicalSize()
            {
                Width = data.Size.X * 32,
                Height = data.Size.Y * 32
            };
            this.MovementSpeed = data.MovementSpeed;
            this.Location = npcData.Position;

            this.NoInteract = data.NoInteract;
            this.NoInteractText = data.NoInteractText;

            if (!IsEnemy)
            {

                if (data.Merchant)
                {
                    this.Merchant = new Dungeon.Merchants.Merchant();
                    this.Merchant.FillBackpacks();
                }
                this.BuildConversations(data);

                if (this.NPC.MoveRegion != null)
                {
                    this.NPC.MoveRegion = this.NPC.MoveRegion * 32;
                }
            }

            if (data.FractionIdentify != default)
            {
                this.NPC.Fraction = FractionView.Load(data.FractionIdentify).ToFraction();
            }
        }

        public override void Reload()
        {
            var data = Database.Entity<NPCData>(x => x.IdentifyName == IdentifyName).FirstOrDefault();
            this.BuildConversations(data);

            if (this.Merchant!=default)
            {
                this.Merchant.FillBackpacks();
            }
        }

        private void Dying()
        {
            List<MapObject> publishObjects = new List<MapObject>();

            var loot = this.Entity.LootTable.Generate();

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

            if (!Gamemap.MapObject.Remove(this))
            {
                throw new System.Exception("Объект не удаляется!");
            }

            Gamemap.Objects.Remove(this);

            publishObjects.ForEach(Gamemap.PublishObject);
        }

        public string DamageSound { get; set; }

        public MapObject AttackRange => new MapObject
        {
            Position = new PhysicalPosition
            {
                X = this.Position.X - ((this.Size.Width * this.AttackRangeMultiples.X) - this.Size.Width) / 2,
                Y = this.Position.Y - ((this.Size.Height * this.AttackRangeMultiples.Y) - this.Size.Height) / 2
            },
            Size = new PhysicalSize
            {
                Width = this.Size.Width * AttackRangeMultiples.X,
                Height = this.Size.Height * AttackRangeMultiples.Y
            }
        };
    }
}