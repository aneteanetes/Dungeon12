namespace Dungeon.Map.Objects
{
    using System;
    using System.Linq;
    using Dungeon.Data;
    using Dungeon.Data.Attributes;
    using Dungeon.Data.Region;
    using Dungeon.Drawing.SceneObjects.Map;
    using Dungeon.Entities.Alive;
    using Dungeon.Entities.Enemy;
    using Dungeon.Entities.NPC;
    using Dungeon.Game;
    using Dungeon.Map.Infrastructure;
    using Dungeon.Physics;
    using Dungeon.Settings;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using Dungeon12.Data.Homes;
    using Dungeon12.Data.Npcs;
    using Force.DeepCloner;

    [Template("Npc")]
    [DataClass(typeof(NPCData))]
    public class NPC : Сonversational
    {
        public override bool Saveable => true;

        public NPCMoveable NPCEntity { get; set; }

        public override string Icon { get => "N"; set { } }

        protected override MapObject Self => this;

        public override bool Obstruction => true;

        public override double MovementSpeed => base.MovementSpeed;

        public bool Moving { get; set; }

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

        protected override void Load(RegionPart npcData)
        {
            var data = Database.Entity<NPCData>(x => x.IdentifyName == npcData.IdentifyName).FirstOrDefault();

            this.NPCEntity = data.NPC.DeepClone();
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

            if (data.Merchant)
            {
                this.Merchant = new Merchants.Merchant();
                this.Merchant.FillBackpacks();
            }
            this.BuildConversations(data);

            if (this.NPCEntity.MoveRegion != null)
            {
                this.NPCEntity.MoveRegion = this.NPCEntity.MoveRegion * 32;
            }
        }
    }
}