using Dungeon.Events;

namespace Dungeon12.Entities.Quests
{
    public class QuestDiscoverFlushedEvent : IEvent
    {
        public IQuest Quest { get; set; }

        public QuestDiscoverFlushedEvent(IQuest quest) => Quest = quest;
    }
}
