using Dungeon;
using Dungeon.Scenes;

namespace Dungeon12.Extensions.Resources
{
    internal static class ResourceLoadingExtensions
    {
        public static void LoadBorders(this Scene scene)
        {
            var basePath = "UI/border/";
            new[]
            {
                $"{basePath}leftup.png",
                $"{basePath}rightup.png",
                $"{basePath}leftdown.png",
                $"{basePath}rightdown.png",
                $"{basePath}left.png",
                $"{basePath}right.png",
                $"{basePath}down.png",
                $"{basePath}up.png"
            }.ForEach(s =>
            {
                scene.Resources.Load(s.AsmImg());
            });
        }
    }
}
