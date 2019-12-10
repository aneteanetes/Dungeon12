using Dungeon;
using Dungeon.Entities;
using Dungeon12.Entities.Alive.Proxies;
using Dungeon12.Game;
using Dungeon12.Map;
using Dungeon.Network;
using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.Database.Quests;
using Dungeon12.SceneObjects.UI;
using System.Linq;
using Dungeon12.Classes;

namespace Dungeon12.Entities.Quests
{
    public abstract class Quest<TQuestData> : DataEntity<Quest<TQuestData>, TQuestData>, IQuest
        where TQuestData : QuestData
    {
        public override bool Events => true;

        /// <summary>
        /// 
        /// <para>
        /// [Рассчётное через сеть]
        /// </para>
        /// </summary>
        [Dungeon.Proxied(typeof(NetProxy),typeof(Limit))]
        public long Progress { get => Get(___Progress, typeof(Quest<>).AssemblyQualifiedName); set => Set(value, typeof(Quest<>).AssemblyQualifiedName); }
        private long ___Progress;


        /// <summary>
        /// 
        /// <para>
        /// [Рассчётное через сеть]
        /// </para>
        /// </summary>
        [Dungeon.Proxied(typeof(NetProxy))]
        public long MaxProgress { get => Get(___MaxProgress, typeof(Quest<>).AssemblyQualifiedName); set => Set(value, typeof(Quest<>).AssemblyQualifiedName); }
        private long ___MaxProgress;


        public string Description { get; set; }

        public Reward Reward { get; set; }

        public Character Character => throw new System.NotImplementedException();

        protected override void Init(TQuestData dataClass)
        {
            this.Name = dataClass.Name;
            this.Description = dataClass.Description;
            this.MaxProgress = dataClass.MaxProgress;
            this.Reward = Reward.Load(dataClass.RewardIdentify);
            this.IdentifyName = dataClass.IdentifyName;
        }

        protected Character _class;
        protected GameMap _gameMap;

        /// <summary>
        /// Можно выполнять несколько раз
        /// </summary>
        protected virtual bool Reactivated => false;

        public void Bind(Character @class, GameMap gameMap)
        {
            _gameMap = gameMap;
            if (@class[IdentifyName] == default || Reactivated)
            {
                _class = @class;
                _class.Journal.Quests.Add(new Entites.Journal.JournalEntry()
                {
                    IdentifyName = this.IdentifyName,
                    Group = gameMap.Name,
                    Display = this.Name,
                    Text = this.Description,
                    Quest=this
                });
                this.Discover = true;
                Global.Events.Raise(new QuestDiscoverEvent(this));
                _class.ActiveQuests.Add(this);
            }
        }

        public override ISceneObject Visual()
        {
            return new QuestDescoverSceneObject(this, true);
        }

        private string ProgressText => Done ? "Выполнено" : $"Прогресс: {Progress}/{MaxProgress}";

        public int Id { get; set; }

        public int ObjectId { get; set; }

        public bool IsCompleted() => Progress == MaxProgress;

        /// <summary>
        /// костыль потому что при выполнении какой-то непонятный прогресс показывает
        /// </summary>
        private bool Done { get; set; }

        public bool Discover { get; set; }

        public virtual void Complete()
        {
            Done = true;
            _class[IdentifyName] = true;
            _class.ActiveQuests.Remove(this);
            this.Reward.GiveReward.Trigger(this.Reward, _class, _gameMap);
            var q = _class.Journal.Quests.First(qu => qu.IdentifyName == this.IdentifyName);
            _class.Journal.Quests.Remove(q);
            _class.Journal.QuestsDone.Add(q);
            UnsubscribeEvents();

            Toast.Show("Задание выполнено!");
        }
    }
}