using Dungeon;
using Dungeon12.Classes;
using Dungeon.Data.Attributes;
using Dungeon12.Data.Region;
using Dungeon12.Entities.Alive;
using Dungeon12.Game;
using Dungeon12.Map;
using Dungeon12.Map.Infrastructure;
using Dungeon12.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.Database.Barrels;
using Dungeon12.SceneObjects.Map;
using System;
using System.Linq;
using Dungeon12.Database.Chest;
using Dungeon12.Loot;

namespace Dungeon12.Map.Objects
{
    [Template("Chest")]
    [DataClass(typeof(ChestData))]
    public class Chest : MapObject
    {
        private bool _saveable = true;
        public override bool Saveable => _saveable;

        public override bool Obstruction => true;
        
        public string IdentifyName { get; set; }

        public bool Used { get; set; }

        private LootTable LootTable { get; set; }

        protected override void Load(RegionPart regionPart)
        {            
            var data = LoadData<ChestData>(regionPart);

            LootTable = LootTable.LoadById<LootTable>(data.LootTable);

            this.Animation = data.Animation;
            this.Name = data.Name;
            this.IdentifyName = regionPart.IdentifyName;
            this.Location = regionPart.Position;
        }

        public override ISceneObject Visual()
        {
            return new ChestSceneObject(Global.GameState.Player, this);
        }

        public void Use(Character alive)
        {
            DropLoot(this.LootTable);
            _saveable = false;
        }
    }
}