using Dungeon;
using Dungeon.Types;

namespace Dungeon12.Attributes
{
    internal class EnumDotAttribute : ValueAttribute
    {
        public EnumDotAttribute(int x, int y) : base(new Dot(x,y))
        {
        }
    }
}
