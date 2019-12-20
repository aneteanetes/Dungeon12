using Dungeon;
using Dungeon12.Conversations;
using Dungeon.Drawing;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.Game;
using Dungeon12.Items;
using Dungeon12.Map;
using Dungeon12.Map.Infrastructure;
using Dungeon12.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.Entities.Items;
using System;
using System.Linq;

namespace Dungeon12.Map.Object
{
    [Template("DeadGuard")]
    public class DeadGuard : Dungeon12.Map.Objects.Loot
    {
        private int randomDeadGuardImageIndex = 0;
        public DeadGuard()
        {
            randomDeadGuardImageIndex = RandomDungeon.Range(1, 4);
        }

        public override string CustomLootImage => $"Objects/deadguards/{randomDeadGuardImageIndex}.png".AsmImgRes();

        public override IDrawColor CustomLootColor => DrawColor.IndianRed;

        public override void PickUp()
        {
            var journal = Global.GameState.Character.Journal;

            var active = journal.Quests.FirstOrDefault(x => x.Quest?.IdentifyName == "DeepCaveQuest");
            if (active != default)
            {
                Dungeon12.Global.GameState.Character["QuestCompleted_DeepCaveQuest"] = true;
                active.Text += Environment.NewLine + Environment.NewLine + "Вы нашли стражей которые пропали, вернитесь к капитану.";
                Toast.Show("Журнал обновлён!");

                active.Quest.Complete();
            }
        }

        public override ISceneObject Visual()
        {
            return new LootSceneObject(Global.GameState.Player, this, "Мёртвый страж");
        }
    }
}
