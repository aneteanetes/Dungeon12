using Dungeon;
using Dungeon12.Classes;

namespace Dungeon12
{
    public abstract class Talant<TClass> : Dungeon12.Abilities.Talants.Talant<TClass>
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
