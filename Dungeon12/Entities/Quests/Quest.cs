using Dungeon;
using Dungeon.Entities;
using Dungeon.Entities.Alive.Proxies;
using Dungeon.Network;
using Dungeon12.Database.Quests;

namespace Dungeon12.Entities.Quests
{
    public class Quest : DataEntity<Quest, QuestData>
    {
        /// <summary>
        /// Текущий прогресс
        /// <para>
        /// [Рассчётное через сеть] [Лимитированое]
        /// </para>
        /// </summary>
        [Proxied(typeof(NetProxy), typeof(Limit))]
        public long Progress { get => Get(___Progress, typeof(Quest).AssemblyQualifiedName); set => Set(value, typeof(Quest).AssemblyQualifiedName); }
        private long ___Progress;

        /// <summary>
        /// Нужный прогресс
        /// <para>
        /// [Рассчётное через сеть]
        /// </para>
        /// </summary>
        [Proxied(typeof(NetProxy))]
        public long MaxProgress { get => Get(___MaxProgress, typeof(Quest).AssemblyQualifiedName); set => Set(value, typeof(Quest).AssemblyQualifiedName); }
        private long ___MaxProgress;

        public string Description { get; set; }

        protected override void Init(QuestData dataClass)
        {
            this.Name = "";
            this.Description = "";
            this.MaxProgress++;
        }
    }
}