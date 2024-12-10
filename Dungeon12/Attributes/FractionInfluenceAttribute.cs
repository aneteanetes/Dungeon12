using Dungeon;
using Nabunassar.Entities.Enums;
using Nabunassar.Entities.FractionPolygons;

namespace Nabunassar.Attributes
{
    internal class FractionInfluenceAttribute : ValueAttribute
    {
        public FractionInfluenceAttribute(FractionInfluenceAbility ability) : base(ability)
        {
        }
    }
}
