using Dungeon.Map.Infrastructure;
using Dungeon.Transactions;

namespace Dungeon.Map.Objects
{
    [Template("t")]
    public abstract class Totem : MapObject
    {
        public Totem()
        {
            this.Range = this;
        }

        public abstract bool CanAffect(MapObject @object);

        protected override MapObject Self => this;
        
        public abstract Applicable ApplicableEffect { get; }

        /// <summary>
        /// Радиус действия эффекта, по умолчанию равен размерам и положению объекта
        /// </summary>
        public Dungeon.Physics.PhysicalObject Range { get; }
    }
}
