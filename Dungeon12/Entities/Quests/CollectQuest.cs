using Dungeon;
using Dungeon.Classes;
using Dungeon.Inventory;
using Dungeon.Items;
using Dungeon.Loot;
using Dungeon.Types;
using Dungeon12.Database.QuestCollect;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.Entities.Quests
{
    public class CollectQuest : Quest<QuestCollectData>
    {
        private List<LootDrop> LootDrops = new List<LootDrop>();

        public Dictionary<string, Pair<int, int>> Targets { get; set; } = new Dictionary<string, Pair<int, int>>();

        protected override void Init(QuestCollectData dataClass)
        {
            base.Init(dataClass);
            dataClass.ItemsIdentify.ForEach((id, i) =>
            {
                Targets.Add(id, new Pair<int, int>(dataClass.Amount[i], 0));
            });

            dataClass.LootDropsIdentify.ForEach(x =>
            {
                var lootDrop = Dungeon.Data.Database.Entity<LootDrop>(drop => drop.IdentifyName == x).FirstOrDefault();
                LootDrops.Add(lootDrop);
                LootTable.GetLootTable(lootDrop.LootTableIdentify).LootDrops.Add(lootDrop);
            });

            MaxProgress = Targets.Sum(a => a.Value.Second);
        }

        public override void Complete()
        {
            LootDrops.ForEach(lootDrop =>
            {
                LootTable.GetLootTable(lootDrop.LootTableIdentify).LootDrops.Remove(lootDrop);
            });
            LootDrops.Clear();

            foreach (var target in Targets)
            {
                var itemForRemove = _class.Backpack.GetItems().FirstOrDefault(i => i.IdentifyName == target.Key);
                _class.Backpack.Remove(itemForRemove, _class, target.Value.First);
            }

            base.Complete();
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
                if (Targets.TryGetValue(itemPickedUpEvent.Item.IdentifyName, out var progress))
                {
                    progress.Second++;
                    if (progress.First > progress.Second)
                    {
                        this.Progress++;
                    }
                }
            }
        }

        public void OnEvent(ItemDropOffEvent itemDropOffEvent)
        {
            if (_class == default)
                return;

            if (itemDropOffEvent.Owner == _class)
            {
                if (Targets.TryGetValue(itemDropOffEvent.Item.IdentifyName, out var progress))
                {
                    progress.Second--;
                    if (progress.Second < progress.First)
                    {
                        this.Progress--;
                    }
                }
            }
        }

        protected override void CallOnEvent(dynamic obj) => OnEvent(obj);
    }
}