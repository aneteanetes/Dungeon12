using System.Xml.Linq;

namespace Dungeon.Utils.XElementExtensions
{
    public static class GetAttributeValueExtension
    {
        /// <summary>
        /// Integer attribute value
        /// </summary>
        /// <param name="xElement"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static int TagAttrInteger(this XElement xElement, string attr)
        {
            int.TryParse(xElement.Attribute(attr).Value, out int value);
            return value;
        }

        public static string TagAttrString(this XElement xElement, string attr)
        {
            return xElement.Attribute(attr).Value;
        }
    }
}
