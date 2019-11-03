using Dungeon.SceneObjects.Mixins;

namespace Dungeon.Proxy
{
    public interface IMixinContainer
    {
        void SetMixinValue<T>(string property, T value);

        T GetMixinValue<T>(string property);

        T Mixin<T>() where T: IMixin;
    }
}
