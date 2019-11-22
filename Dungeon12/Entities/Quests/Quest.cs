using Dungeon;
using Dungeon.Entities;
using Dungeon.Entities.Alive.Proxies;
using Dungeon.Game;
using Dungeon.Map;
using Dungeon.Network;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.Database.Quests;
using System.Linq;

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
        [Proxied(typeof(NetProxy))]
        public long Progress { get => Get(___Progress, typeof(Quest<>).AssemblyQualifiedName); set => Set(value, typeof(Quest<>).AssemblyQualifiedName); }
        private long ___Progress;


        /// <summary>
        /// 
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

        protected override void Init(TQuestData dataClass)
        {
            this.Name = dataClass.Name;
            this.Description = dataClass.Description;
            this.MaxProgress = dataClass.MaxProgress;
            this.Reward = Reward.Load(dataClass.RewardIdentify);
            this.IdentifyName = dataClass.IdentifyName;
        }

        protected Dungeon12Class _class;
        protected GameMap _gameMap;

        /// <summary>
        /// Можно выполнять несколько раз
        /// </summary>
        protected virtual bool Reactivated => false;

        public void Bind(Dungeon12Class @class, GameMap gameMap)
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
                _class.ActiveQuests.Add(this);
            }
        }

        public override ISceneObject Visual(GameState gameState)
        {
            return new TextControl(ProgressText.AsDrawText().Montserrat())
            {
                OnUpdate = x => x.Text.SetText(ProgressText)
            };
        }

        private string ProgressText => $"Прогресс: {Progress}/{MaxProgress}";

        public int Id { get; set; }

        public int ObjectId { get; set; }

        public bool IsCompleted() => Progress == MaxProgress;

        public virtual void Complete()
        {
            _class[IdentifyName] = true;
            _class.ActiveQuests.Remove(this);
            this.Reward.GiveReward.Trigger(this.Reward, _class, _gameMap);
            var q = _class.Journal.Quests.First(qu => qu.IdentifyName == this.IdentifyName);
            _class.Journal.Quests.Remove(q);
            _class.Journal.QuestsDone.Add(q);
        }
    }
}