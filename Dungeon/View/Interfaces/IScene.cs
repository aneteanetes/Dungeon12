using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.View.Interfaces
{
    public interface IScene
    {
        ISceneObject[] Objects { get; }

        bool AbsolutePositionScene { get; }
    }
}
