using Dungeon12.Entities.Enums;

namespace Dungeon12.ECS.Components
{
    internal interface ICursored
    {
        CursorImage Cursor { get; }
    }
}
