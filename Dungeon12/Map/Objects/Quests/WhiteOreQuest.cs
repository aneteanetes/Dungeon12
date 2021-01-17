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
    [Template("WhiteOreQuest")]
    public class WhiteOreQuest : Dungeon12.Map.Objects.Loot
    {
        public WhiteOreQuest()
        {
            Item = Dungeon.Store.EntitySingle<Item>(typeof(QuestItem).AssemblyQualifiedName, "WhiteOre");
        }

        public override string CustomLootImage => "Objects/haystack.png".AsmImg();

        public override IDrawColor CustomLootColor => DrawColor.SaddleBrown;

        public override ISceneObject Visual()
        {
            return new LootSceneObject(Global.GameState.Player, this, "Белая руда");
        }
    }
}
