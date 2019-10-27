using Rogue.Map.Infrastructure;
using Rogue.Transactions;

namespace Rogue.Map.Objects
{
    [Template("t")]
    public abstract class Totem : MapObject
    {
        public Totem()
        {
            this.Range = this;
        }

        protected override MapObject Self => this;
        
        public abstract Applicable ApplicableEffect { get; }

        /// <summary>
        /// Радиус действия эффекта, по умолчанию равен размерам и положению объекта
        /// </summary>
        public Physics.PhysicalObject Range { get; }
    }
}
