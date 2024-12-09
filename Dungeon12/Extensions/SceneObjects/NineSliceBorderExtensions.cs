using Dungeon.View.Interfaces;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12
{
    static internal class NineSliceBorderExtensions
    {
        /// <summary>
        /// border is 5px
        /// </summary>
        /// <param name="sceneObject"></param>
        /// <param name="opacity"></param>
        public static Border AddBorderBack(this ISceneObject sceneObject, double opacity=.95)
        {
            var b = new Border(sceneObject.Width, sceneObject.Height, opacity);
            sceneObject.AddChild(b);
            return b;
        }

        public static BorderMap AddBorderMapBack(this ISceneObject sceneObject, double opacity = .95, string bord = null)
        {
            var b = new BorderMap(sceneObject.Width, sceneObject.Height, opacity,bord);
            sceneObject.AddChild(b);
            return b;
        }

        public static BorderMap AddBorderMapBack(this ISceneObject sceneObject, BorderConfiguration cfg)
        {
            cfg.Width = sceneObject.Width;
            cfg.Height = sceneObject.Height;
            var b = new BorderMap(cfg);
            sceneObject.AddChild(b);
            return b;
        }

        /// <summary>
        /// border is 5px
        /// </summary>
        /// <param name="sceneObject"></param>
        /// <param name="opacity"></param>
        public static void AddBorder(this ISceneObject sceneObject)
        {
            sceneObject.AddChild(new Border(sceneObject.Width, sceneObject.Height, -1));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sceneObject"></param>
        /// <param name="opacity"></param>
        public static void AddBorder(this ISceneObject sceneObject, BorderConfiguration settings)
        {
            settings.BindDefaults(sceneObject);

            sceneObject.AddChild(new Border(settings));
        }
    }
}
