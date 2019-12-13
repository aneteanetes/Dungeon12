using Dungeon;
using Dungeon.Types;
using Dungeon12.SceneObjects;
using System;
using System.Linq;

namespace Dungeon12.Map.Triggers
{
    public class JournalUpdateTrigger : ITrigger<bool, string[]>
    {
        public bool Trigger(string[] arg1)
        {
            var additionalText = arg1.ElementAtOrDefault(0);
            var identifyName = arg1.ElementAtOrDefault(1);
            bool.TryParse(arg1.ElementAtOrDefault(2), out var replace);


            var journal = Global.GameState.Character.Journal;

            var entry = journal.Quests
                .Concat(journal.Details)
                .Concat(journal.World)
                .Concat(journal.QuestsDone)
                .FirstOrDefault(x => x.IdentifyName == identifyName);

            if (entry == default)
                return false;

            if (replace)
            {
                entry.Text = additionalText;
            }
            else
            {
                entry.Text += Environment.NewLine + Environment.NewLine + additionalText;
            }
            Toast.Show("Журнал обновлён!");

            return true;
        }
    }
}