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
            dataClass.MobIdentify.ForEach((id, i) =>
            {
                Targets.Add(id, new Pair<int, int>(dataClass.Amount[i], 0));
            });

            dataClass.MaxProgress = Targets.Sum(a => a.Value.First);
        }

        public void OnEvent(AliveKillEvent aliveKillEvent)
        {
            if (_class == default)
                return;

            if (aliveKillEvent.Killer == _class)
            {
                if (Targets.TryGetValue(aliveKillEvent.Victim.Name, out var progress))
                {
                    progress.Second++;
                    if (progress.First < progress.Second)
                    {
                        this.Progress++;
                    }
                }
            }
        }
    }
}