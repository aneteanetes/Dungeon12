using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.View.Interfaces
{
    public interface IScene
    {
        ISceneObject[] Objects { get; }
    }
}
