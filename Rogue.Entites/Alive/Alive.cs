using Rogue.Entites.Alive.Proxies;
using Rogue.Network;
using Rogue.Types;

namespace Rogue.Entites.Alive
{
    /// <summary>
    /// Живой, с уровнем
    /// </summary>
    public class Alive : NetObject, IFlowable
    {
        public int Level { get; set; } = 1;

        protected override string ProxyId => this.Uid;

        /// <summary>
        /// 
        /// <para>
        /// [Рассчётное через сеть]
        /// </para>
        /// </summary>
        [Proxied(typeof(NetProxy), typeof(Limit))]
        public long HitPoints { get => Get(___HitPoints, typeof(Alive).AssemblyQualifiedName); set => Set(value, typeof(Alive).AssemblyQualifiedName); }
        public long ___HitPoints;
        
        public long MaxHitPoints { get; set; }

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
    }
}