using Dungeon;
using Dungeon.Classes;

namespace Dungeon12
{
    public abstract class Talant<TClass> : Dungeon.Abilities.Talants.Talant<TClass>
        where TClass : Character
    {
        public Talant(int order) : base(order)
        {
        }

        public override string Image
        {
            get
            {
                return $"Dungeon12.{typeof(TClass).Name}.Resources.Images.Talants.{GetType().Name}.png";
            }
        }
    }
}
