using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.IO;
using IrrKlang;


namespace Rogue
{
    public static class SoundEngine
    {
        public static ISoundEngine Player = new ISoundEngine();
        private static string pathMusic = Directory.GetCurrentDirectory() + "/Music/";
        private static string pathSound = Directory.GetCurrentDirectory() + "/Sound/";

        public static class Settings
        {
            public static float Volume { set { Player.SoundVolume = -50; } }// = value; } }
        }

        public static class Music
        {
            public static void MainTheme() {}// Rogue.RAM.DungeonSound = false; Player.StopAllSounds(); Player.Play2D(pathMusic + "maintheme.ogg", true); }
            public static void TownTheme() {} //Rogue.RAM.DungeonSound = false; Player.StopAllSounds(); Player.Play2D(pathMusic + "towntheme.ogg", true); }
            public static void DungeonTheme() {}// if (!Rogue.RAM.DungeonSound) { Player.StopAllSounds(); Player.Play2D(pathMusic + "dungeontheme.ogg", true); Rogue.RAM.DungeonSound = true; } }
            public static void SilentTheme() { } //Rogue.RAM.DungeonSound = false; Player.StopAllSounds(); Player.Play2D(pathMusic + "silenttheme.ogg", true); }
        }

        public static class Sound
        {
            public static void NPC() { Player.Play2D(pathSound + "npc.ogg"); }
            public static void TakeItem() { Player.Play2D(pathSound + "take.ogg"); }
            public static void OpenDoor() { Player.Play2D(pathSound + "opendoor.ogg"); }
            public static void Attack() { Player.Play2D(pathSound + "attack.ogg"); }
            public static void Teleport() { Player.Play2D(pathSound + "teleport.ogg"); }
            public static void Strike() { Player.Play2D(pathSound + "strike.ogg"); }
            public static void MagicCast() { Player.Play2D(pathSound + "magic.ogg"); }
            public static void DropItem() { Player.Play2D(pathSound + "dropitem.ogg"); }
            public static void DestroyItem() { Player.Play2D(pathSound + "destroy.ogg"); }
            public static void OpenChest() { Player.Play2D(pathSound + "chest.ogg"); }
            public static void Drink() { Player.Play2D(pathSound + "potion.ogg"); }
            public static void Eqip() { Player.Play2D(pathSound + "equip.ogg"); }
            public static void Identify() { Player.Play2D(pathSound + "identify.ogg"); }
            public static void LevelUp() { Player.Play2D(pathSound + "level.ogg"); }
            public static void GameOver() { Player.StopAllSounds(); Player.Play2D(pathSound + "gameover.ogg"); }
        }
    }
}
