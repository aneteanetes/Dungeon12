using Dungeon.View.Interfaces;
using System.Linq;

namespace Dungeon.ECS
{
    public static class ECSExtensions
    {
        public static bool IsComponent<TECSComponent>(this ISceneObject sceneObject)
            where TECSComponent : IComponent
        {
            if (sceneObject is TECSComponent)
                return true;


            return sceneObject.Components.FirstOrDefault(c => c.Type == typeof(TECSComponent)) !=default;
        }

        public static bool IsComponent<T1C, T2C>(this ISceneObject sceneObject)
            where T1C : IComponent
            where T2C : IComponent
        => sceneObject.IsComponent<T1C>() || sceneObject.IsComponent<T2C>();

        public static bool IsComponent<T1C, T2C, T3C>(this ISceneObject sceneObject)
            where T1C : IComponent
            where T2C : IComponent
            where T3C : IComponent
        => sceneObject.IsComponent<T1C,T2C>() || sceneObject.IsComponent<T3C>();
    }
}
