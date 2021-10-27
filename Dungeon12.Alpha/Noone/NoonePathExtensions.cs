using Dungeon;

namespace Dungeon12.Noone
{
    public static class NoonePathExtensions
    {
        public static string NoonePath(this string path) => "Dungeon12.Noone.Resources." + path.Embedded();

        public static string NooneSoundPath(this string path) => "Dungeon12.Noone.Resources.Audio.Sound" + path.Embedded();
    }
}