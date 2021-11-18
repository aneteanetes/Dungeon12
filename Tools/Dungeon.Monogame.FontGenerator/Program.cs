using Dungeon.Resources;
using System;
using System.Linq;
using System.Reflection;

namespace Dungeon.Monogame
{
    public class Program
    {
        static void Main(string[] args)
        {
            DungeonGlobal.GameAssembly = typeof(Program).Assembly;
            ResourceLoader.LoadAllAssembliesInFolder();
            ResourceLoader.ResourceResolvers.Add(new EmbeddedResourceResolver(Assembly.GetExecutingAssembly()));

            var contentFilePath = args.ElementAtOrDefault(0);
            var fontNames = args.ElementAtOrDefault(1);
            var fontsizeMin = args.ElementAtOrDefault(2);
            var fontsizeMax = args.ElementAtOrDefault(3);

            int.TryParse(fontsizeMin, out var min);
            int.TryParse(fontsizeMax, out var max);

            var generator = new FontGenerator(contentFilePath, "Gabriela", min == default ? 8 : min, max == default ? 72 : max);
            generator.Generate();
        }
    }
}
