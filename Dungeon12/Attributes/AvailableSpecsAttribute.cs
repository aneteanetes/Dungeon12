using Dungeon;
using Dungeon12.Entities.Enums;

namespace Dungeon12.Attributes
{
    internal class AvailableSpecsAttribute : ValueAttribute
    {
        public AvailableSpecsAttribute(params Spec[] specs) : base(specs)
        {
        }
    }
}
