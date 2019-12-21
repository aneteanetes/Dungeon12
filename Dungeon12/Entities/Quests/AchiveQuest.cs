using Dungeon;
using Dungeon12.Database.QuestAchive;
using System.Linq;

namespace Dungeon12.Entities.Quests
{
    public class AchiveQuest : Quest<QuestAchiveData>
    {
        private ITrigger<bool, string[]> Trigger => TriggerName == default ? default : TriggerName.Trigger<ITrigger<bool, string[]>>();
        private string[] Arguments;

        public string TriggerName { get; set; }

        protected override void Init(QuestAchiveData dataClass)
        {
            base.Init(dataClass);
            MaxProgress = 1;

            if (dataClass.TriggerName != default)
            {
                TriggerName = dataClass.TriggerName;
                Arguments = dataClass.TriggerArguments;
            }
        }

        public override bool IsCompleted()
        {
            Progress = 1;
            if (Trigger != default)
            {
                Trigger.Trigger(Arguments);
            }
            return base.IsCompleted();
        }

        public new static AchiveQuest Load(string id)
        {
            var entity = new AchiveQuest();

            var dataClass = Store.Entity<QuestAchiveData>(x => x.IdentifyName == id).FirstOrDefault();
            if (dataClass != default)
            {
                entity.Init(dataClass);
                return entity;
            }

            return default;
        }
    }
}