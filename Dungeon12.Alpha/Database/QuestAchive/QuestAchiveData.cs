using Dungeon12.Database.Quests;

namespace Dungeon12.Database.QuestAchive
{
    public class QuestAchiveData : QuestData
    {
        public string TriggerName { get; set; }

        public string[] TriggerArguments { get; set; }
    }
}
