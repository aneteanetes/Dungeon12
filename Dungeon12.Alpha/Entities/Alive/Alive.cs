using Dungeon;
using Dungeon.Drawing;
using Dungeon.Network;
using Dungeon.View.Interfaces;
using Dungeon12.Entities.Alive.Proxies;
using Dungeon12.SceneObjects;
using Dungeon.SceneObjects;
using System;
using Newtonsoft.Json;

namespace Dungeon12.Entities.Alive
{
    /// <summary>
    /// Живой, с уровнем
    /// </summary>
    public class Alive : EntityFraction
    {
        public int Level { get; set; } = 1;

        public virtual long ExpGain => Level * 10;

        public virtual bool ExpGainer => false;

        public long EXP { get; set; }

        public long MaxExp { get; set; } = 100;

        public void Exp(long amount)
        {
            if (!ExpGainer)
                return;

            EXP += amount;

            var text = $"{amount} опыта!".AsDrawText()
                .InColor(DrawColor.DarkMagenta)
                .InSize(12);

            var popup = new PopupString(text, this.MapObject?.Location, 25, 0.06).InList<ISceneObject>();
            this.MapObject.SceneObject.ShowInScene(popup);

            if (EXP >= MaxExp)
            {
                LevelUp();
            }
        }

        protected virtual void LevelUp()
        {
            this.MaxExp = Level * 100 + EXP;
            this.Level++;
            FreeStatPoints += 5;

            this.MaxHitPoints += (int)Math.Ceiling(((this.InitialHP * HitPointsPercentPlus) / 100));

            var visual = this.SceneObject.ShowInScene;
            
            var txt = $"Вы достигли {this.Level} уровня!".AsDrawText().InSize(10).Montserrat();
            Global.AudioPlayer.Effect("level.wav".AsmSoundRes());
            Toast.Show(txt, visual);
        }

        public void RecalculateLevelHP()
        {
            for (int i = 1; i < this.Level; i++)
            {
                this.MaxHitPoints += (int)Math.Ceiling(((this.MaxHitPoints * HitPointsPercentPlus) / 100));
            }
        }

        public int FreeStatPoints { get; set; }

        public override string ProxyId => this.Uid;

        /// <summary>
        /// Процент на который увеличивается HP с уровнем
        /// </summary>
        public virtual double HitPointsPercentPlus => 2;


        public virtual int InitialHP => 100;

        /// <summary>
        /// 
        /// <para>
        /// [Рассчётное через сеть]
        /// </para>
        /// <para>
        /// [Лимит 0-Max]
        /// </para>
        /// </summary>
        [Dungeon.Proxied(typeof(NetProxy), typeof(Limit))]
        public long HitPoints { get => Get(___HitPoints, typeof(Alive).AssemblyQualifiedName); set => Set(value, typeof(Alive).AssemblyQualifiedName); }
        private long ___HitPoints;
        
        /// <summary>
        /// 
        /// <para>
        /// [Рассчётное через сеть]
        /// </para>
        /// </summary>
        [Dungeon.Proxied(typeof(NetProxy))]
        [JsonProperty(Order = -2)]
        public long MaxHitPoints { get => Get(___MaxHitPoints, typeof(Alive).AssemblyQualifiedName); set => Set(value, typeof(Alive).AssemblyQualifiedName); }
        private long ___MaxHitPoints;

        public bool Dead { get; set; } = false;
        

        private object flowContext = null;

        public T GetFlowProperty<T>(string property, T @default = default) => flowContext.GetProperty<T>(property, @default);

        public bool SetFlowProperty<T>(string property, T value)
        {
            try
            {
                flowContext.SetProperty(property, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SetFlowContext(object context) => flowContext = context;

        public object GetFlowContext() => flowContext;
        
        private IFlowable flowparent = null;

        public void SetParentFlow(IFlowable parent) => flowparent = parent;

        public IFlowable GetParentFlow() => flowparent;

        public Action OnDie { get; set; }

        public void Die()
        {
            OnDie?.Invoke();
            this.SceneObject?.Destroy?.Invoke();
            this.MapObject?.Destroy?.Invoke();
        }
    }
}