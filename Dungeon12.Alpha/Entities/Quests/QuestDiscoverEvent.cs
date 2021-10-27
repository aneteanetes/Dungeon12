using Dungeon.Events;

namespace Dungeon12.Entities.Quests
{
    public class QuestDiscoverEvent : IEvent
    {
        public IQuest Quest { get; set; }

        public QuestDiscoverEvent(IQuest quest) => Quest = quest;
    }
}
