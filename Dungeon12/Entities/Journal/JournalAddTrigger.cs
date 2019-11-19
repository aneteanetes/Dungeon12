using Dungeon;
using Dungeon.Conversations;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects.Map;
using Dungeon.Map;
using Dungeon.View.Interfaces;
using Dungeon12.Entites.Journal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities.Journal
{
    public class JournalAddTrigger : IConversationTrigger
    {
        public bool Storable => true;

        public IDrawText Trigger(PlayerSceneObject arg1, GameMap arg2, string[] arg3)
        {
            var category = arg3[0];
            var filter = arg3[1];
            var cat = arg1.Avatar.Character.As<Dungeon12Class>().Journal.GetPropertyExpr<List<JournalEntry>>(category);
            cat.AddRange(JournalEntry.LoadAll(x => x.IdentifyName.Contains(filter)));

            return new DrawText("Добавлены записи в журнал").Montserrat();
        }
    }
}
