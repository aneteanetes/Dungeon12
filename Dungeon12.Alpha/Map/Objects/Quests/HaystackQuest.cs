using Dungeon;
using Dungeon12.Conversations;
using Dungeon.Drawing;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.Game;
using Dungeon12.Items;
using Dungeon12.Map;
using Dungeon12.Map.Infrastructure;
using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.Entities.Items;
using System;
using System.Linq;

namespace Dungeon12.Map.Objects
{
    [Template("HaystackQuest")]
    public class HaystackQuest : Dungeon12.Map.Objects.Loot
    {
        public HaystackQuest()
        {
            Item = Dungeon.Store.EntitySingle<Item>(typeof(QuestItem).AssemblyQualifiedName, "WarlockDeckQuest");
        }

        public override string TakeTrigger => nameof(HaystackQuestTakeTrigger);

        public override string CustomLootImage => "Objects/haystack.png".AsmImg();

        public override IDrawColor CustomLootColor => DrawColor.LightYellow;

        public override ISceneObject Visual()
        {
            return new LootSceneObject(Global.GameState.Player, this, "Стог сена");
        }

        public static void UpdateHaystackQuest(string text)
        {
            var quest = Global.GameState.Character.Journal.Quests.FirstOrDefault(x => x.Quest.IdentifyName == "CardGetting");
            quest.Text += Environment.NewLine + Environment.NewLine + text;
            Toast.Show("Журнал обновлён!");
        }
    }

    public class HaystackQuestHideTrigger : ConversationTrigger
    {
        protected override IDrawText Trigger(PlayerSceneObject arg1, GameMap arg2, string[] arg3)
        {
            var haystack = arg2.One<HaystackQuest>(arg2.Range(59, 45, 2, 2));
            haystack.SceneObject.Destroy?.Invoke();
            haystack.Destroy?.Invoke();
            HaystackQuest.UpdateHaystackQuest("Крарн подозревает что постоялец в таверне хочет заполучить его колоду. Вместо того что бы отдать свою, он посоветовал заглянуть в глубокие пещеры и найти колоду у павшего стража деревни.");
            return default;            
        }
    }

    public class HaystackQuestTakeTrigger : ITrigger<bool, string[]>
    {
        public bool Trigger(string[] arg1)
        {
            Global.GameState.Character[nameof(HaystackQuest)] = true;
            HaystackQuest.UpdateHaystackQuest("Вы нашли колоду карт Крарна в ближайшем стоге сена. Если бы он играл честно - стал бы он прятать её?");
            return true;
        }
    }
}