using Dungeon.ECS;
using System.Diagnostics;

namespace Dungeon.View.Interfaces
{
    public interface ISceneLayer
    {
        IScene Scene { get; }

        ISceneObject[] Objects { get; }

        IEffect[] SceneGlobalEffects { get; }

        double Width { get; }

        double Height { get; }

        double Left { get; }

        double Top { get; }

        bool Destroyed { get; }

        void AddObject(ISceneObject sceneObject);

        void AddControl(ISceneControl sceneObjectControl);

        void AddObjectCenter<TSceneObject>(TSceneObject sceneObject, bool horizontal = true, bool vertical = true)
            where TSceneObject : ISceneObject;

        void RemoveObject(ISceneObject sceneObject);

        bool AbsoluteLayer { get; }

        void AddSystem(ISystem system);

        void RemoveSystem(ISystem system);

    }
}