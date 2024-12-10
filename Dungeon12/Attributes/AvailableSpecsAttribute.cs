using Dungeon;
using Nabunassar.Entities.Enums;

namespace Nabunassar.Attributes
{
    internal class AvailableSpecsAttribute : ValueAttribute
    {
        public AvailableSpecsAttribute(params Spec[] specs) : base(specs)
        {
        }
    }
}
