using Dungeon;
using Dungeon.View.Interfaces;

namespace Dungeon12
{
    internal static class Debugger
    {
        private static ISceneObject sceneObject;

        public static void Bind(ISceneObject _sceneObject)
        {
            sceneObject= _sceneObject;
            Console.WriteLine(sceneObject.ToString());
        }

        public static void Set(string property, string value)
        {
            if (sceneObject == null)
            {
                Console.WriteLine("sceneObject is null!");
                return;
            }

            try
            {
                Console.WriteLine($"{sceneObject.Uid}.{property} was: {sceneObject.GetPropertyExprRaw(property)}");
                sceneObject.SetPropertyExprConverted(property, value);
                Console.WriteLine($"{sceneObject.Uid}.{property} now: {sceneObject.GetPropertyExprRaw(property)}");
            }
            catch
            {
                Console.WriteLine("fault");
            }
        }

        public static void Get(string property)
        {
            if (sceneObject==null)
            {
                Console.WriteLine("sceneObject is null!");
                return;
            }

            try
            {
                Console.WriteLine($"{sceneObject.Uid}.{property} now: {sceneObject.GetPropertyExprRaw(property)}");
            }
            catch
            {
                Console.WriteLine("fault");
            }
        }
    }
}
