namespace Rogue
{
    public static class StringPathExtensions
    {
        public static string PathResource(this string path)=> "Rogue.Resources." + path;

        public static string PathImage(this string path) => "".PathResource() + "Images." + path;

        public static string PathParticle(this string path) => "".PathResource() + path;

        public static string PathPng(this string path) => path + ".png";
    }
}
