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

        public static string ImgPath(this string img)
        {
            if(!imgPathCache.TryGetValue(img,out var imgPath))
            {
                imgPath = $"{Assembly.GetCallingAssembly().GetName().Name}.Images.{img.Replace(@"\", ".")}";
                imgPathCache.Add(img, imgPath);                
            }

            return imgPath;
        }
    }
}
