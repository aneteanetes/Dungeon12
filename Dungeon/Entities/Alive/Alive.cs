using Dungeon.Drawing;
using Dungeon.Entities.Alive.Proxies;
using Dungeon.Map;
using Dungeon.Network;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon.View.Interfaces;

namespace Dungeon.Entities.Alive
{
    /// <summary>
    /// Живой, с уровнем
    /// </summary>
    public class Alive : Entity, IFlowable
    {
        public int Level { get; set; } = 1;

        public virtual long ExpGain => Level * 10;

        public virtual bool ExpGainer => false;

        public long EXP { get; set; }

        public long MaxExp => Level * 100;

        public void Exp(long amount)
        {
            if (!ExpGainer)
                return;

            EXP += amount;

            var text = $"{amount} опыта!".AsDrawText()
                .InColor(DrawColor.DarkMagena)
                .InSize(12);

            var popup = new PopupString(text, this.MapObject?.Location, 25, 0.06).InList<ISceneObject>();
            this.MapObject.SceneObject.ShowEffects(popup);
        }

        public override string ProxyId => this.Uid;

        /// <summary>
        /// 
        /// <para>
        /// [Рассчётное через сеть]
        /// </para>
        /// <para>
        /// [Лимит 0-Max]
        /// </para>
        /// </summary>
        [Proxied(typeof(NetProxy), typeof(Limit))]
        public long HitPoints { get => Get(___HitPoints, typeof(Alive).AssemblyQualifiedName); set => Set(value, typeof(Alive).AssemblyQualifiedName); }
        private long ___HitPoints;
        
        /// <summary>
        /// 
        /// <para>
        /// [Рассчётное через сеть]
        /// </para>
        /// </summary>
        [Proxied(typeof(NetProxy))]
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

        public void Die()
        {
            this.SceneObject?.Destroy?.Invoke();
            this.MapObject?.Destroy?.Invoke();
        }
    }
}