using Dungeon;
using Dungeon12.Classes;
using Dungeon12.Inventory;
using Dungeon12.Items;
using Dungeon12.Loot;
using Dungeon.Types;
using Dungeon12.Database.QuestCollect;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.Entities.Quests
{
    public class CollectQuest : Quest<QuestCollectData>
    {
        private List<LootDrop> LootDrops = new List<LootDrop>();

        public string[] LootDropsIdentify { get; set; }

        public Dictionary<string, Pair<int, int>> Targets { get; set; } = new Dictionary<string, Pair<int, int>>();

        protected override void Init(QuestCollectData dataClass)
        {
            base.Init(dataClass);
            dataClass.ItemsIdentify.ForEach((id, i) =>
            {
                Targets.Add(id, new Pair<int, int>(dataClass.Amount[i], 0));
            });

            this.LootDropsIdentify = dataClass.LootDropsIdentify;
            ReloadQuestLootDrops();

            MaxProgress = Targets.Sum(a => a.Value.First);
        }

        public void ReloadQuestLootDrops()
        {
            LootDropsIdentify.ForEach(x =>
            {
                var lootDrop = Store.Entity<LootDrop>(drop => drop.IdentifyName == x).FirstOrDefault();
                LootDrops.Add(lootDrop);
                LootTable.GetLootTable(lootDrop.LootTableIdentify).LootDrops.Add(lootDrop);
            });
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

            var dataClass = Store.Entity<QuestCollectData>(x => x.IdentifyName == id).FirstOrDefault();
            if (dataClass != default)
            {
                entity.Init(dataClass);
                return entity;
            }            

            return default;
        }

        public void OnEvent(ItemPickedUpEvent itemPickedUpEvent)
        {
            if (_class == default)
                return;

            if (itemPickedUpEvent.Owner == _class)
            {
                if (itemPickedUpEvent.Item.IdentifyName == default)
                    return;

                if (Targets.TryGetValue(itemPickedUpEvent.Item.IdentifyName, out var progress))
                {
                    progress.Second++;
                    if (progress.First >= progress.Second)
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
                if (itemDropOffEvent.Item.IdentifyName == default)
                    return;

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