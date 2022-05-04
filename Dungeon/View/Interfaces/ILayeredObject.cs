namespace Dungeon.View.Interfaces
{
    using Dungeon.Types;
    using Dungeon.Utils;
    using Dungeon.View.Enums;
    using System;
    using System.Collections.Generic;

    [Hidden]
    public interface ILayeredObject : ISceneObject
    {

        ISceneObject AddChild(ISceneObject sceneObject);

        ISceneObject RemoveChild(ISceneObject sceneObject);

        bool PerPixelCollision { get; }
    }
}
