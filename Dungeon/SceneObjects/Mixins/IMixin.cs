using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.SceneObjects.Mixins
{
    public interface IMixin : ISceneObject
    {
        void InitAsMixin(SceneObject owner);
    }
}
