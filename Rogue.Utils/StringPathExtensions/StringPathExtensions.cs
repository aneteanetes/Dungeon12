using System.Collections.Generic;
using System.Reflection;

namespace Rogue
{
    public static class StringPathExtensions
    {
        public static string PathResource(this string path)=> "Rogue.Resources." + path;

        public static string PathImage(this string path) => "".PathResource() + "Images." + path.Replace(@"\",".");

        public static string PathParticle(this string path) => "".PathResource() + path;

        public static string PathPng(this string path) => path + ".png";


        private static Dictionary<string, string> imgPathCache = new Dictionary<string, string>();

        public static string ImgPath(this string img,string callingAsmName=default)
        {
            if(!imgPathCache.TryGetValue(img,out var imgPath))
            {
                imgPath = $"{callingAsmName ?? Assembly.GetCallingAssembly().GetName().Name}.Images.{img.Replace(@"\", ".").Replace(@"/", ".")}";
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
        public static string AsmName(this string img,string between="") => Assembly.GetCallingAssembly().GetName().Name + between+ img;
    }
}
