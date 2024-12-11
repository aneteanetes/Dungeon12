using Dungeon.ECS;
using Dungeon.Scenes;
using System.Collections.Generic;
using System.Diagnostics;

namespace Dungeon.View.Interfaces
{
    public interface ISceneLayer
    {
        IScene Scene { get; }

        List<ISceneObject> Objects { get; }

        List<ISceneObject> ActiveObjects { get; }

        List<ISceneControl> ActiveObjectControls { get; }

        List<ISceneObject> UpdatableObjects { get; }

        IEffect[] SceneGlobalEffects { get; }

        void Update(GameTimeLoop gameTime);

        double Width { get; }

        double Height { get; }

        double Left { get; }

        double Top { get; }

        bool Destroyed { get; }

        void Switch<T>() where T : GameScene;

        TSceneObject AddObject<TSceneObject>(TSceneObject sceneObject) where TSceneObject : ISceneObject;

        void AddExistedControl(ISceneControl sceneObjectControl);

        TSceneObject AddObjectCenter<TSceneObject>(TSceneObject sceneObject, bool horizontal = true, bool vertical = true)
            where TSceneObject : ISceneObject;

        void RemoveObject(ISceneObject sceneObject);

        bool AbsoluteLayer { get; }

    }
}