using Dungeon.Utils;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.SceneObjects.Mixins
{
    [Hidden]
    public interface IMixin : ISceneObject
    {
        void InitAsMixin(ISceneObject owner);
    }
}
