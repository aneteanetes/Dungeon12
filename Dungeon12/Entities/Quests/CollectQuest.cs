using Dungeon;
using Dungeon.Entities.Alive.Events;
using Dungeon.Entities.Alive.Proxies;
using Dungeon.Inventory;
using Dungeon.Network;
using Dungeon.Types;
using Dungeon12.Database.QuestCollect;
using Dungeon12.Database.QuestsKill;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.Entities.Quests
{
    public class CollectQuest : Quest<QuestCollectData>
    {
        public Dictionary<string, Pair<int, int>> Targets { get; set; } = new Dictionary<string, Pair<int, int>>();

        protected override void Init(QuestCollectData dataClass)
        {
            base.Init(dataClass);
            dataClass.ItemsIdentify.ForEach((id, i) =>
            {
                Targets.Add(id, new Pair<int, int>(dataClass.Amount[i], 0));
            });

            MaxProgress = Targets.Sum(a => a.Value.Key);
        }

        public new static CollectQuest Load(string id)
        {
            var entity = new CollectQuest();

            var dataClass = Dungeon.Data.Database.Entity<QuestCollectData>(x => x.IdentifyName == id, id).FirstOrDefault();
            if (dataClass != default)
            {
                entity.Init(dataClass);
            }

            return entity;
        }

        public void OnEvent(ItemPickedUpEvent itemPickedUpEvent)
        {
            if (_class == default)
                return;

            if (itemPickedUpEvent.Owner == _class)
            {
                if (Targets.TryGetValue(itemPickedUpEvent.Item.Name, out var progress))
                {
                    if (progress.Key < progress.Value)
                    {
                        progress.Value++;
                        this.Progress++;
                    }
                }
            }
        }
    }
}