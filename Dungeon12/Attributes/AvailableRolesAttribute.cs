using Dungeon;
using Dungeon12.Entities.Enums;

namespace Dungeon12.Attributes
{
    public class AvailableRolesAttribute : ValueAttribute
    {
        public AvailableRolesAttribute(params Roles[] roles) : base(roles)
        {
        }
    }
}
