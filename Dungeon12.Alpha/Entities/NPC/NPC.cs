using Dungeon12.Conversations;
using Dungeon12.Entities.Alive;
using Dungeon12.Loot;
using Dungeon.Types;
using LiteDB;
using Dungeon.Audio;
using System;
using Dungeon12.Drawing.SceneObjects.Map;

namespace Dungeon12.Entities
{
    public class NPC : Moveable, ILootable
    {
        public bool Aggressive { get; set; }

        public string DieImage { get; set; }

        public Rectangle DieImagePosition { get; set; }

        public string LootTableName { get; set; }

        public Point AttackRange { get; set; }

        public Conversation Conversation { get; set; }

        public bool ChasingPlayers { get; set; }

        public bool ChasingEnemies { get; set; }

        public string DamageSound { get; set; }

        [BsonIgnore]
        public LootTable LootTable => LootTable.GetLootTable(this.IdentifyName ?? this.LootTableName);

        protected override long DamageProcess(Damage dmg, long amount)
        {
            if (this.ChasingPlayers)
                if (this?.MapObject?.SceneObject is NPCSceneObject npcSceneObject)
                {
                    npcSceneObject.Chasing(Global.GameState.PlayerAvatar);
                }

            if (DamageSound != default)
            {
                var pos = this.MapObject;
                var posP = Global.GameState.PlayerAvatar.Grow(12);

                if (!posP.IntersectsWithOrContains(this.MapObject))
                    return base.DamageProcess(dmg, amount);

                var opts = new AudioOptions()
                {
                    Volume=.2
                };

                var x1 = posP.Position.X/32;
                var x2 = pos.Position.X/32;
                var y1 = posP.Position.Y/32;
                var y2 = pos.Position.Y/32;

                var d = (((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2))) / 2;

                for (int i = 0; i < d; i+=1)
                {
                    opts.Volume -= 0.01;
                }

                Global.AudioPlayer.Effect($"{DamageSound}.wav".AsmSoundRes(), opts);
            }

            return base.DamageProcess(dmg, amount);
        }
    }
}
