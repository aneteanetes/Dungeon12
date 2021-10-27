using Dungeon;
using Dungeon12.Entities.Alive.Events;
using Dungeon.Types;
using Dungeon12.Database.QuestsKill;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.Entities.Quests
{
    public class KillQuest : Quest<QuestKillData>
    {
        public Dictionary<string, Pair<int, int>> Targets { get; set; } = new Dictionary<string, Pair<int, int>>();

        protected override void Init(QuestKillData dataClass)
        {
            base.Init(dataClass);
            dataClass.MobIdentify.ForEach((id, i) =>
            {
                Targets.Add(id, new Pair<int, int>(dataClass.Amount[i], 0));
            });

            MaxProgress = Targets.Sum(a => a.Value.First);
        }

        public void OnEvent(AliveKillEvent aliveKillEvent)
        {
            if (_class == default)
                return;

            if (aliveKillEvent.Killer == _class)
            {
                if (Targets.TryGetValue(aliveKillEvent.Victim.IdentifyName, out var progress))
                {
                    progress.Second++;
                    if (progress.First >= progress.Second)
                    {
                        this.Progress++;
                    }
                }
            }
        }

        protected override void CallOnEvent(dynamic obj) => OnEvent(obj);

        public new static KillQuest Load(string id)
        {
            var entity = new KillQuest();

            var dataClass = Store.Entity<QuestKillData>(x => x.IdentifyName == id).FirstOrDefault();
            if (dataClass != default)
            {
                entity.Init(dataClass);
                return entity;
            }

            return default;
        }
    }
}