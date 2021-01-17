using System.Xml.Linq;

namespace Dungeon.Utils.XElementExtensions
{
    public static class GetAttributeValueExtension
    {

        public static int IA(this XElement xElement, string attr)
        {
            int.TryParse(xElement.Attribute(attr).Value, out int value);
            return value;
        }
    }
}
