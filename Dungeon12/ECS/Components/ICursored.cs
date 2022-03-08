using Dungeon12.Entities.Enums;

namespace Dungeon12.ECS.Components
{
    public interface ICursored
    {
        CursorImage Cursor { get; }
    }
}
