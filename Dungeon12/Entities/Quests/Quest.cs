using Dungeon;
using Dungeon.Entities;
using Dungeon.Entities.Alive.Events;
using Dungeon.Entities.Alive.Proxies;
using Dungeon.Network;
using Dungeon12.Database.Quests;

namespace Dungeon12.Entities.Quests
{
    public abstract class Quest<TQuestData> : DataEntity<Quest<TQuestData>, TQuestData>, IQuest
        where TQuestData : QuestData
    {
        public override bool Events => true;

        /// <summary>
        /// Текущий прогресс
        /// <para>
        /// [Рассчётное через сеть] [Лимитированое]
        /// </para>
        /// </summary>
        [Proxied(typeof(NetProxy), typeof(Limit))]
        public long Progress { get => Get(___Progress, typeof(Quest<>).AssemblyQualifiedName); set => Set(value, typeof(Quest<>).AssemblyQualifiedName); }
        private long ___Progress;

        /// <summary>
        /// Нужный прогресс
        /// <para>
        /// [Рассчётное через сеть]
        /// </para>
        /// </summary>
        [Proxied(typeof(NetProxy))]
        public long MaxProgress { get => Get(___MaxProgress, typeof(Quest<>).AssemblyQualifiedName); set => Set(value, typeof(Quest<>).AssemblyQualifiedName); }
        private long ___MaxProgress;

        public string Description { get; set; }

        public Reward Reward { get; set; }

        public Dungeon12Class Character => throw new System.NotImplementedException();

        protected string IdentifyName { get; set; }

        protected override void Init(TQuestData dataClass)
        {
            this.Name = dataClass.Name;
            this.Description = dataClass.Description;
            this.MaxProgress = dataClass.MaxProgress;
            this.Reward = Reward.Load(dataClass.RewardIdentify);
            this.IdentifyName = dataClass.IdentifyName;
        }

        protected Dungeon12Class _class;

        /// <summary>
        /// Можно выполнять несколько раз
        /// </summary>
        protected virtual bool Reactivated => false;

        public void Bind(Dungeon12Class @class)
        {
            if (@class[IdentifyName] == default || Reactivated)
            {
                _class = @class;
            }
        } 

        public void Complete()
        {
            _class[IdentifyName] = true;
        }
    }
}