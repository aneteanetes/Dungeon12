using System;

namespace Dungeon
{
    public interface IAutoUnfreeze
    {
        Action Destroy { get; set; }
    }
}
