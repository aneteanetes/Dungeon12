using Dungeon;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Dungeon12
{
    public static class ResourcesExtensions
    {
        /// <summary>
        /// Assembly.Resources.Audio.Music.
        /// <returns></returns>
        public static string AsmMusicRes(this string img, string between = "") => Assembly.GetCallingAssembly().GetName().Name + ".Resources.Audio.Music." + between.Embedded() + img.Embedded();

        /// <summary>
        /// Assembly.Resources.Audio.Sounds.
        /// <returns></returns>
        public static string AsmSoundRes(this string img, string between = "") => Assembly.GetCallingAssembly().GetName().Name + ".Resources.Audio.Sounds." + between.Embedded() + img.Embedded();
    }
}
