using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Dungeon.View.Interfaces;

namespace Dungeon
{
    public static class StringPathExtensions
    {
        /// <summary>
        /// <see cref="Global"/>.<see cref="IGameClient.CacheImage"/>
        /// </summary>
        /// <param name="imgPath"></param>
        /// <returns></returns>
        public static string Cache(this string imgPath)
        {
            DungeonGlobal.GameClient.CacheImage(imgPath);
            return imgPath;
        }

        private static Dictionary<string, string> cache = new Dictionary<string, string>();

        public static string Embedded(this string path)
        {
            if(!cache.ContainsKey(path))
            {
                cache.Add(path, path.Replace(@"\", ".").Replace(@"/", "."));
            }
            return cache[path];
        }

        public static string PathImage(this string path) => DungeonGlobal.GameAssemblyName + ".Resources.Images." + path.Embedded();
        
        public static string PathPng(this string path) => path + ".png";


        private static Dictionary<string, string> imgPathCache = new Dictionary<string, string>();

        public static string ImgPath(this string img,string callingAsmName=default)
        {
            if(!imgPathCache.TryGetValue(img,out var imgPath))
            {
                imgPath = $"{callingAsmName ?? Assembly.GetCallingAssembly().GetName().Name}.Images.{img.Embedded()}";
                imgPathCache.Add(img, imgPath);                
            }

            return imgPath;
        }

        /// <summary>
        /// Алиас <see cref="ImgPath(string)"/> т.к. название неудачное
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static string PathAsmImg(this string img) => ImgPath(img, Assembly.GetCallingAssembly().GetName().Name);


        /// <summary>
        /// Вернёт имя сборки + строка
        /// <returns></returns>
        public static string AsmName(this string img, string between = "") => Assembly.GetCallingAssembly().GetName().Name + between.Embedded() + img.Embedded();

        /// <summary>
        /// Вернёт имя сборки + Resources + строка
        /// <returns></returns>
        public static string AsmNameRes(this string img, string between = "") => Assembly.GetCallingAssembly().GetName().Name + ".Resources." + between.Embedded() + img.Embedded();


        /// <summary>
        /// Assembly.Resources.Images.Path
        /// <returns></returns>
        public static string AsmImg(this string img, string between = "") => Assembly.GetCallingAssembly().GetName().Name + ".Resources.Images." + between.Embedded() + img.Embedded();

        /// <summary>
        /// CallingAssembly.Resources.Images._between_IMG@_RESOLUTION.extension
        /// </summary>
        /// <param name="img"></param>
        /// <param name="between"></param>
        /// <returns></returns>
        public static string AsmImgResolution(this string img, string between = "") => Assembly.GetCallingAssembly().GetName().Name + ".Resources.Images." + between.Embedded() + Path.GetFileNameWithoutExtension(img.Embedded()) + "@" + DungeonGlobal.Resolution + Path.GetExtension(img.Embedded());

        public static string AsmRes(this string res)=> Assembly.GetCallingAssembly().GetName().Name + ".Resources." + res.Embedded();

        public static string AsmRes(this string res, Assembly assembly) => assembly.GetName().Name + ".Resources." + res.Embedded();

        public static string ImgRes(this string img) => ".Resources.Images." + img.Embedded();

        public static string AudioPathMusic(this string img, string between = "") => Assembly.GetCallingAssembly().GetName().Name + ".Resources.Audio.Music." + between.Embedded() + img.Embedded();

        public static string AudioPathSound(this string img, string between = "") => Assembly.GetCallingAssembly().GetName().Name + ".Resources.Audio.Sound." + between.Embedded() + img.Embedded();
    }
}
