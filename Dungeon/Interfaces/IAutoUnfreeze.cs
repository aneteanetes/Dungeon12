using System;

namespace Dungeon
{
    public interface IAutoUnfreeze
    {
        Action OnDestroy { get; set; }

        void Destroy();
    }
}
