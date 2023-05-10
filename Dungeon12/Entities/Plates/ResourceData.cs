using Dungeon;
using Dungeon.Drawing;

namespace Dungeon12.Entities.Plates
{
    internal class ResourceData
    {
        public string Title { get; set; }

        public string Amount { get; set; }

        public DrawColor Color { get; set; }

        public override string ToString()
        {
            return $"{Amount} {Title}";
        }
    }
}
