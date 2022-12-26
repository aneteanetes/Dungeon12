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

        TSceneObject AddObject<TSceneObject>(TSceneObject sceneObject) where TSceneObject : ISceneObject;

        void AddExistedControl(ISceneControl sceneObjectControl);

        TSceneObject AddObjectCenter<TSceneObject>(TSceneObject sceneObject, bool horizontal = true, bool vertical = true)
            where TSceneObject : ISceneObject;

        void RemoveObject(ISceneObject sceneObject);

        bool AbsoluteLayer { get; }

        void AddSystem(ISystem system);

        TSystem GetSystem<TSystem>() where TSystem : ISystem;

        void RemoveSystem(ISystem system);

    }
}