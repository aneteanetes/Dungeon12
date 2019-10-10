using Rogue.Types;

namespace Rogue.Entites.Alive
{
    /// <summary>
    /// Живой, с уровнем
    /// </summary>
    public class Alive : Drawable, IFlowable
    {
        public int Level { get; set; } = 1;

        private long hitPoints;
        public long HitPoints
        {
            get => hitPoints <= 0 ? 0 : hitPoints;
            set
            {
                hitPoints = value;
                if (hitPoints <= 0)
                {
                    Dead = true;
                }
            }
        }
        
        public long MaxHitPoints { get; set; }

        public bool Dead { get; private set; } = false;
        

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