using Dungeon.Utils;
using Dungeon.View.Interfaces;

namespace Dungeon.GameObjects
{
    public abstract class GameComponent<TSceneObject> : GameComponent
        where TSceneObject : ISceneObject
    {
        [Newtonsoft.Json.JsonIgnore]
        [Hidden]
        public TSceneObject View
        {
            get => SceneObject.As<TSceneObject>();
            set => SceneObject = value;
        }
    }
}
