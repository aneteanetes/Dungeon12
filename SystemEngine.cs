using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Threading;
using System.Runtime.Serialization;

namespace Rogue
{
    public static class SystemEngine
    {
        public class ColorSettings
        {
            private bool _DeadEyesColors;

            private string __DeadEyesColors
            {
                get
                {
                    if (_DeadEyesColors)
                    {
                        return "Да";
                    }
                    else
                    {
                        return "Нет";
                    }
                }
            }

            public string DeadEyesColors
            {
                set { _DeadEyesColors = Boolean.TryParse(value, out _DeadEyesColors); }
                get { return __DeadEyesColors; }
            }
        }

        public class SoundSettings
        {
            public float Volume;
        }

        public class BugReport
        {
            public string Category, Title, Text, Author;
        }

        public class GraphicHeroSettings
        {
            public char Icon = '@';

            public ConsoleColor Color = ConsoleColor.Red;
        }

        public enum ArrowDirection
        {
            /// <summary>
            /// User pressed top arrow
            /// </summary>
            Top = 0,
            /// <summary>
            /// User pressed bottom arrow
            /// </summary>
            Bot = 1,
            /// <summary>
            /// User pressed left arrow
            /// </summary>
            Left = 2,
            /// <summary>
            /// User pressed right arrow
            /// </summary>
            Right = 3
        }

        public enum PopUpWindow
        {
            V1 = 0,
            VA = 1
        }
        /// <summary>
        /// This class can give you information (or some thing) about D12-object
        /// </summary>
        public static class Helper
        {
            private static Random r=new Random();
            public static class Randomer
            {
                public static MechEngine.Race Race
                {
                    get
                    {
                        Thread.Sleep(10);
                        switch (r.Next(10))
                        {
                            case 0: { return MechEngine.Race.DarkElf; }
                            case 1: { return MechEngine.Race.Dwarf; }
                            case 2: { return MechEngine.Race.Elf; }
                            case 3: { return MechEngine.Race.FallenAngel; }
                            case 4: { return MechEngine.Race.Gnome; }
                            case 5: { return MechEngine.Race.Human; }
                            case 6: { return MechEngine.Race.MoonElf; }
                            case 7: { return MechEngine.Race.Orc; }
                            case 8: { return MechEngine.Race.Troll; }
                            case 9: { return MechEngine.Race.Undead; }
                            default: { return MechEngine.Race.Human; }
                        }
                        
                    }
                }

                public static MechEngine.BattleClass Class
                {
                    get
                    {
                        Thread.Sleep(10);
                        switch (r.Next(11))
                        {
                            case 0: { return  MechEngine.BattleClass.Alchemist; }
                            case 1: { return MechEngine.BattleClass.Assassin; }
                            case 2: { return MechEngine.BattleClass.BloodMage; }
                            case 3: { return MechEngine.BattleClass.FireMage;}
                            case 4: { return MechEngine.BattleClass.Inquisitor;}
                            case 5: { return MechEngine.BattleClass.Monk;}
                            case 7: { return MechEngine.BattleClass.Necromant;}
                            case 8: { return MechEngine.BattleClass.Paladin;}
                            case 9: { return MechEngine.BattleClass.Shaman;}
                            case 10: { return MechEngine.BattleClass.Warrior; }
                            default: { return MechEngine.BattleClass.Monk; }
                        }

                    }
                }

                private static List<string> WarlockPowers = new List<string>()
                {
                    "Пламя ада",
                    "Огненный дождь",
                    "Ледяной купол",
                    "Примораживание",
                    "Испепеление",
                    "Вилку истины",
                    "Уничтожение тела",
                    "Превращение в нежить",
                    "Пожирание души",
                    "Разящую молнию",
                    "Электрический заряд смерти"
                };
                public static string WarlockSpell
                {
                    get
                    {
                        return WarlockPowers[r.Next(WarlockPowers.Count)];
                    }
                }

            }

            /// <summary>
            /// Get specific information about D12Object
            /// </summary>
            public static class Information
            {
                /// <summary>
                /// Get Color of class
                /// </summary>
                public static ConsoleColor ClassC
                {
                    get
                    {
                        switch (Rogue.RAM.Player.Class)
                        {
                            case MechEngine.BattleClass.Alchemist: { return ConsoleColor.Magenta; }
                            case MechEngine.BattleClass.Assassin: { return ConsoleColor.Green; }
                            case MechEngine.BattleClass.BloodMage: { return ConsoleColor.DarkRed; }
                            case MechEngine.BattleClass.FireMage: { return ConsoleColor.Blue; }
                            case MechEngine.BattleClass.Inquisitor: { return ConsoleColor.Cyan; }
                            case MechEngine.BattleClass.Monk: { return ConsoleColor.Yellow; }
                            case MechEngine.BattleClass.Necromant: { return ConsoleColor.White; }
                            case MechEngine.BattleClass.Paladin: { return ConsoleColor.Blue; }
                            case MechEngine.BattleClass.Shaman: { return ConsoleColor.Blue; }
                            case MechEngine.BattleClass.Valkyrie: { return ConsoleColor.Cyan; }
                            case MechEngine.BattleClass.LightWarrior: { return ConsoleColor.Cyan; }
                            case MechEngine.BattleClass.Warlock: { return ConsoleColor.Blue; }
                            case MechEngine.BattleClass.Illusionist: { return ConsoleColor.DarkBlue; }
                            case MechEngine.BattleClass.Warrior: { return ConsoleColor.Red; }
                            default: { return ConsoleColor.Black; }
                        }
                    }
                }
                /// <summary>
                /// Get multiply of class
                /// </summary>
                public static double Class
                {
                    get
                    {
                        switch (Rogue.RAM.Player.Class)
                        {
                            case MechEngine.BattleClass.Alchemist: { return 3.7; }
                            case MechEngine.BattleClass.Assassin: { return 3.8; }
                            case MechEngine.BattleClass.BloodMage: { return 3.5; }
                            case MechEngine.BattleClass.FireMage: { return 3.6; }
                            case MechEngine.BattleClass.Inquisitor: { return 3.8; }
                            case MechEngine.BattleClass.Monk: { return 3.7; }
                            case MechEngine.BattleClass.Necromant: { return 3.6; }
                            case MechEngine.BattleClass.Paladin: { return 3.8; }
                            case MechEngine.BattleClass.Shaman: { return 3.8; }
                            default: { return 5; }
                        }
                    }
                }
                /// <summary>
                /// Get multiply of race
                /// </summary>
                public static double Race
                {
                    get
                    {
                        switch (Rogue.RAM.Player.Race)
                        {
                            case MechEngine.Race.DarkElf: { return 3.7; }
                            case MechEngine.Race.Dwarf: { return 3.7; }
                            case MechEngine.Race.Elf: { return 3.6; }
                            case MechEngine.Race.FallenAngel: { return 3.8; }
                            case MechEngine.Race.Gnome: { return 3.6; }
                            case MechEngine.Race.Human: { return 3.5; }
                            case MechEngine.Race.MoonElf: { return 3.6; }
                            case MechEngine.Race.Orc: { return 3.8; }
                            case MechEngine.Race.Troll: { return 3.7; }
                            case MechEngine.Race.Undead: { return 3.7; }
                            default: { return 5; }
                        }
                    }
                }

                

                public static List<Mob> MobsHere()
                {
                    List<Mob> m = new List<Mob>();
                    switch (Rogue.RAM.Map.Biom)
                    {
                        case ConsoleColor.DarkGreen:
                            {
                                m.Add(new Mob() { Icon = DataBase.MobBase.AzraiWarrior.Icon, Color = DataBase.MobBase.AzraiWarrior.Chest, Name = DataBase.MobBase.AzraiWarrior.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.AzraiElite.Icon, Color = DataBase.MobBase.AzraiElite.Chest, Name = DataBase.MobBase.AzraiElite.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.AzraiTiger.Icon, Color = DataBase.MobBase.AzraiTiger.Chest, Name = DataBase.MobBase.AzraiTiger.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.Maiden.Icon, Color = DataBase.MobBase.Maiden.Chest, Name = DataBase.MobBase.Maiden.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.Wisp.Icon, Color = DataBase.MobBase.Wisp.Chest, Name = DataBase.MobBase.Wisp.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.AirSpirit.Icon, Color = DataBase.MobBase.AirSpirit.Chest, Name = DataBase.MobBase.AirSpirit.Name });
                                break;
                            }
                        case ConsoleColor.DarkYellow:
                            {
                                m.Add(new Mob() { Icon = DataBase.MobBase.Demon.Icon, Color = DataBase.MobBase.Demon.Chest, Name = DataBase.MobBase.Demon.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.Ifrit.Icon, Color = DataBase.MobBase.Ifrit.Chest, Name = DataBase.MobBase.Ifrit.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.Dragonid.Icon, Color = DataBase.MobBase.Dragonid.Chest, Name = DataBase.MobBase.Dragonid.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.BlackDragon.Icon, Color = DataBase.MobBase.BlackDragon.Chest, Name = DataBase.MobBase.BlackDragon.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.WhiteDragon.Icon, Color = DataBase.MobBase.WhiteDragon.Chest, Name = DataBase.MobBase.WhiteDragon.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.FireSpirit.Icon, Color = DataBase.MobBase.FireSpirit.Chest, Name = DataBase.MobBase.FireSpirit.Name });
                                break;
                            }
                        case ConsoleColor.DarkMagenta:
                            {
                                m.Add(new Mob() { Icon = DataBase.MobBase.DrowWarrior.Icon, Color = DataBase.MobBase.DrowWarrior.Chest, Name = DataBase.MobBase.DrowWarrior.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.DrowArcher.Icon, Color = DataBase.MobBase.DrowArcher.Chest, Name = DataBase.MobBase.DrowArcher.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.Shadow.Icon, Color = DataBase.MobBase.Shadow.Chest, Name = DataBase.MobBase.Shadow.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.Scorpio.Icon, Color = DataBase.MobBase.Scorpio.Chest, Name = DataBase.MobBase.Scorpio.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.DrowTitan.Icon, Color = DataBase.MobBase.DrowTitan.Chest, Name = DataBase.MobBase.DrowTitan.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.EarthSpirit.Icon, Color = DataBase.MobBase.EarthSpirit.Chest, Name = DataBase.MobBase.EarthSpirit.Name });
                                break;
                            }
                        case ConsoleColor.DarkGray:
                            {
                                m.Add(new Mob() { Icon = DataBase.MobBase.Sceleton.Icon, Color = DataBase.MobBase.Sceleton.Chest, Name = DataBase.MobBase.Sceleton.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.Zombie.Icon, Color = DataBase.MobBase.Zombie.Chest, Name = DataBase.MobBase.Zombie.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.Troll.Icon, Color = DataBase.MobBase.Troll.Chest, Name = DataBase.MobBase.Troll.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.Spectral.Icon, Color = DataBase.MobBase.Spectral.Chest, Name = DataBase.MobBase.Spectral.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.Lich.Icon, Color = DataBase.MobBase.Lich.Chest, Name = DataBase.MobBase.Lich.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.DeathSpirit.Icon, Color = DataBase.MobBase.DeathSpirit.Chest, Name = DataBase.MobBase.DeathSpirit.Name });
                                break;
                            }
                        case ConsoleColor.DarkCyan:
                            {
                                m.Add(new Mob() { Icon = DataBase.MobBase.Hrimthus.Icon, Color = DataBase.MobBase.Hrimthus.Chest, Name = DataBase.MobBase.Hrimthus.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.HighlandRogue.Icon, Color = DataBase.MobBase.HighlandRogue.Chest, Name = DataBase.MobBase.HighlandRogue.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.HighlandTiger.Icon, Color = DataBase.MobBase.HighlandTiger.Chest, Name = DataBase.MobBase.HighlandTiger.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.WaterPriest.Icon, Color = DataBase.MobBase.WaterPriest.Chest, Name = DataBase.MobBase.WaterPriest.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.WaterSpirit.Icon, Color = DataBase.MobBase.WaterSpirit.Chest, Name = DataBase.MobBase.WaterSpirit.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.FrostSpirit.Icon, Color = DataBase.MobBase.FrostSpirit.Chest, Name = DataBase.MobBase.FrostSpirit.Name });
                                break;
                            }
                        case ConsoleColor.Gray:
                            {
                                m.Add(new Mob() { Icon = DataBase.MobBase.GnomeRioter.Icon, Color = DataBase.MobBase.GnomeRioter.Chest, Name = DataBase.MobBase.GnomeRioter.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.Dune.Icon, Color = DataBase.MobBase.Dune.Chest, Name = DataBase.MobBase.Dune.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.DwarfFortress.Icon, Color = DataBase.MobBase.DwarfFortress.Chest, Name = DataBase.MobBase.DwarfFortress.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.DwarfMerch.Icon, Color = DataBase.MobBase.DwarfMerch.Chest, Name = DataBase.MobBase.DwarfMerch.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.DwarfPriest.Icon, Color = DataBase.MobBase.DwarfPriest.Chest, Name = DataBase.MobBase.DwarfPriest.Name });
                                m.Add(new Mob() { Icon = DataBase.MobBase.DwarfSpirit.Icon, Color = DataBase.MobBase.DwarfSpirit.Chest, Name = DataBase.MobBase.DwarfSpirit.Name });
                                break;
                            }
                    }
                    return m;
                }

                public class Mob
                { public char Icon; public ConsoleColor Color; public string Name;}

                public class Point 
                { public int x; public int y;}
                public static Point FindObject(MechEngine.ActiveObject Object)
                {
                    MechEngine.Labirinth.Cell[][] map = Rogue.RAM.Map.Map;
                    for (int y = 0; y < 23; y++)
                    {
                        for (int x = 0; x < 71; x++)
                        {
                            if (map[x][y].Object == Object)
                            {
                                return new Point() { x = x, y = y };
                            }
                        }

                    }
                    return new Point();
                }
                public static Point FindEnemy(MechEngine.Monster Object)
                {
                    MechEngine.Labirinth.Cell[][] map = Rogue.RAM.Map.Map;
                    for (int y = 0; y < 23; y++)
                    {
                        for (int x = 0; x < 71; x++)
                        {
                            if (map[x][y].Enemy == Object)
                            {
                                return new Point() { x = x, y = y };
                            }
                        }

                    }
                    return new Point();
                }
                public static Point FindPlayer(MechEngine.Character Object)
                {
                    MechEngine.Labirinth.Cell[][] map = Rogue.RAM.Map.Map;
                    for (int y = 0; y < 23; y++)
                    {
                        for (int x = 0; x < 71; x++)
                        {
                            if (map[x][y].Player == Object)
                            {
                                return new Point() { x = x, y = y };
                            }
                        }

                    }
                    return new Point();
                }
            }
            /// <summary>
            /// Return special string which associate with object
            /// </summary>
            public static class String
            {
                /// <summary>
                /// Return one of the most appropriate string
                /// </summary>
                /// <param name="Object">Object to string</param>
                /// <returns></returns>
                public static string ToString(MechEngine.AbilityStats Object)
                { switch (Object) { case MechEngine.AbilityStats.AD: { return "Сила атаки"; } case MechEngine.AbilityStats.AP: { return "Сила магии"; } case MechEngine.AbilityStats.ARM: { return "Защита"; } case MechEngine.AbilityStats.DMG: { return "Урон"; } case MechEngine.AbilityStats.MHP: { return "Жизнь"; } case MechEngine.AbilityStats.MMP: { return "Мана"; } case MechEngine.AbilityStats.MRS: { return "Маг. Защита"; } } return ""; }
                /// <summary>
                /// Return one of the most appropriate string
                /// </summary>
                /// <param name="Object">Object to string</param>
                /// <returns></returns>
                public static string ToString(MechEngine.AbilityElement Object)
                { switch (Object) { case MechEngine.AbilityElement.Physical: { return "Материя"; } case MechEngine.AbilityElement.ElementalMagic: { return "Магия"; } case MechEngine.AbilityElement.FireMagic: { return "Огонь"; } case MechEngine.AbilityElement.NatureMagic: { return "Природа"; } } return ""; }
                /// <summary>
                /// Return one of the most appropriate string
                /// </summary>
                /// <param name="Object">Object to string</param>
                /// <returns></returns>
                public static string ToString(MechEngine.AbilityType Object)
                { switch (Object) { case MechEngine.AbilityType.Active: { return "Активный"; } case MechEngine.AbilityType.Passive: { return "Пассивный"; } default: { return ""; } } }
                /// <summary>
                /// Return one of the most appropriate string
                /// </summary>
                /// <param name="Object">Object to string</param>
                /// <returns></returns>
                public static string ToString(MechEngine.AbilityLocation Object)
                { switch (Object) { case MechEngine.AbilityLocation.Alltime: { return "Везде"; } case MechEngine.AbilityLocation.Combat: { return "В бою"; } case MechEngine.AbilityLocation.WorldMap: { return "Карта"; } default: { return ""; } } }
                /// <summary>
                /// Return one of the most appropriate string
                /// </summary>
                /// <param name="Object">Object to string</param>
                /// <returns></returns>
                public static string ToString(MechEngine.MonsterRace Object)
                {
                    switch (Object) { case MechEngine.MonsterRace.Animal: { return "Животные"; } case MechEngine.MonsterRace.Avariel: { return "Спириталы"; } case MechEngine.MonsterRace.Dragon: { return "Драконы"; } case MechEngine.MonsterRace.Drow: { return "Темные эльфы"; } case MechEngine.MonsterRace.Human: { return "Гуманойды"; } case MechEngine.MonsterRace.Undead: { return "Нежить"; } default: { return ""; } }
                }
                /// <summary>
                /// Return one of the most appropriate string
                /// </summary>
                /// <param name="Object">Object to string</param>
                /// <returns></returns>
                public static string ToString(MechEngine.AbilityRate Object, double d, double p)
                { switch (Object) { case MechEngine.AbilityRate.AttackDamage: { return "Сила атаки: " + d.ToString(); } case MechEngine.AbilityRate.AbilityPower: { return "Сила магии: " + p.ToString(); } default: { return ""; } } }
            }
            /// <summary>
            /// Dispose special D12 object
            /// </summary>
            public static class Disposing
            {
                /// <summary>
                /// Dispose timer
                /// </summary>
                public static void Dispose(System.Timers.Timer Object)
                {
                    ///Object.Enabled = false; Object.Dispose();
                }

                public static bool DisposeTimers
                {
                    set
                    {
                        if (value)
                        {
                            //MechEngine.Ability.SummonTimer.Close();
                            //MechEngine.Ability.DebuffTimer.Close();
                            //MechEngine.Ability.DoTtimer.Close();
                            //MechEngine.Ability.HoTtimer.Close();
                            //MechEngine.Ability.ImproveTimer.Close();
                            //MechEngine.Ability.SummonTimer.Close();
                            Rogue.RAM.Judge.JudgeEye.Close();
                            try
                            {
                                if (Rogue.RAM.Player.Buffs != null)
                                {
                                    for (int i = 0; i < Rogue.RAM.Player.Buffs.Count; i++)
                                    {
                                        Rogue.RAM.Player.Buffs.Remove(Rogue.RAM.Player.Buffs[i]);
                                    }
                                }
                            }
                            catch { }
                            //MechEngine.Ability.SummonTimer.Enabled = false;
                            //MechEngine.Ability.DebuffTimer.Enabled = false;
                            //MechEngine.Ability.DoTtimer.Enabled = false;
                            //MechEngine.Ability.HoTtimer.Enabled = false;
                            //MechEngine.Ability.ImproveTimer.Enabled = false;
                            //MechEngine.Summoned.SummonedTimer.Enabled = false;
                            Rogue.RAM.Judge.JudgeEye.Enabled = false;
                        }
                    }
                }
            }
            /// <summary>
            /// Activate special D12 objects
            /// </summary>
            public static class Activation
            {
                /// <summary>
                /// Activate summoned monsters
                /// </summary>
                public static void StartBattle()
                {
                    foreach (MechEngine.Summoned Ally in Rogue.RAM.SummonedList)
                    {
                        Ally.Enabled = true;
                    }
                    Rogue.RAM.Judge.Enabled = true;
                }
            }
            /// <summary>
            /// Deactivate special D12 objects
            /// </summary>
            public static class Deactivation
            {
                /// <summary>
                /// Deactivate summoned monsters
                /// </summary>
                public static void EndBattle()
                {
                    foreach (MechEngine.Summoned Ally in Rogue.RAM.SummonedList)
                    {
                        Ally.Enabled = false;
                    }
                    Rogue.RAM.Judge.Enabled = false;
                    if (Rogue.RAM.Player.Class == MechEngine.BattleClass.Inquisitor)
                    {
                        Rogue.RAM.Player.CMP = Rogue.RAM.Player.MMP;
                        DrawEngine.GUIDraw.ReDrawCharStat();
                        DrawEngine.InfoWindow.Custom("Жажда битвы удовлетворена, сила кары восстановлена!");
                    }
                }
            }
            /// <summary>
            /// Return string stat
            /// </summary>
            public static class Stat
            {                
                /// <summary>
                /// get
                /// </summary>
                public static MechEngine.AbilityStats Gets
                {
                    get
                    {                        
                        switch (r.Next(6))
                        {
                            case 0: { return MechEngine.AbilityStats.AD; }
                            case 1: { return MechEngine.AbilityStats.AP; }
                            case 2: { return MechEngine.AbilityStats.ARM; }
                            case 3: { return MechEngine.AbilityStats.DMG; }
                            case 4: { return MechEngine.AbilityStats.MHP; }
                            case 5: { return MechEngine.AbilityStats.MRS; }
                            default: { return MechEngine.AbilityStats.MHP; }
                        }
                    }
                }
            }
            /// <summary>
            /// Net helper
            /// </summary>
            public static class Net
            {
                /// <summary>
                /// Send e-mail
                /// </summary>
                /// <param name="Bug">Bug for send</param>
                public static void SendBugReport(BugReport Bug)
                {
                    if (Internet)
                    {
                        var fromAddress = new MailAddress("dungeon12sup@gmail.com", Bug.Author);
                        var toAddress = new MailAddress("dungeon12sup@gmail.com", "Fix");
                        const string fromPassword = "dungeon1217041993";
                        string subject = Bug.Category;
                        StringBuilder body = new System.Text.StringBuilder();
                        body.AppendLine("From: \t\t" + Bug.Author);
                        body.AppendLine("Category: \t\t" + Bug.Category);
                        body.AppendLine("Title: \t\t" + Bug.Title);
                        body.AppendLine("Text: " + Bug.Text);
                        var smtp = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                        };
                        using (var message = new MailMessage(fromAddress, toAddress)
                        {
                            Subject = subject,
                            Body = body.ToString()
                        })
                        {
                            smtp.Send(message);
                        }
                    }
                    else
                    {
                        DrawEngine.ConsoleDraw.WriteTitle("Для отправки баг репорта требуется подключение к интернету!");
                    }
                }
                /// <summary>
                /// return internet connection status
                /// </summary>
                public static bool Internet
                {
                    get 
                    {
                        try { using (var c = new WebClient()) using (var s = c.OpenRead("http://ya.ru")) { return true; } }
                        catch { return false; }
                    }
                }
            }
            /// <summary>
            /// Actions with files
            /// </summary>
            public static class File
            {
                public static List<SaveLoadFile> LoadFiles
                {
                    get
                    {
                        List<SaveLoadFile> lf = new List<SaveLoadFile>();
                        Directory.CreateDirectory("Save");
                        string[] fl = Directory.GetFiles("Save");
                        foreach (string s in fl)
                        {
                            //D12ML.D12MLDocument Doc = new D12ML.D12MLDocument(s);
                            //Doc.Load(s);
                            //SaveLoadFile f = new SaveLoadFile();
                            //f = Doc.ExtractD12MLInfo;
                            //f.Path = s;
                            //lf.Add(f);
                        }                       
                        Rogue.RAM.LoadFiles = new List<SaveLoadFile>();
                        Rogue.RAM.LoadFiles = lf;
                        return lf;
                    }
                }

                public static string DeleteFile
                {
                    set
                    {
                        Directory.CreateDirectory("Save");
                        string[] fl = Directory.GetFiles("Save");
                        foreach (string s in fl)
                        {
                            if (s == ("Save\\" + value + ".d12ml"))
                            { System.IO.File.Delete("Save\\" + value + ".d12ml"); }
                        }
                    }
                }

                public static class ExportDisplay
                {
                    public static bool ExportKeyMap
                    {
                        set
                        {
                            var f = System.IO.File.Open("KeyMap.html", FileMode.OpenOrCreate);
                            string b = @"<table style='border: 1px goldenrod;text-decoration-color: #ffffff;' rules='all'>
                                    <tr>
                                        <td style='color: gold'><b>Action</b></td><td style='color: gold'><b>Key</b></td><td style='color: gold'><b>Modifier</b></td><td colspan='1' style='color: gold'><b>Item window</b></td><td style='color: gold'><b>Key</b></td><td style='color: gold'><b>Next Key / Mod</b></td>
                                    </tr>
                                    <tr>
                                        <td>Opening the doors</td><td>O</td><td>-</td><td>Wear armor</td><td>A</td><td>Numpad 1-6</td>
                                    </tr>
                                    <tr>
                                        <td>Talk to the merchant</td><td>O</td><td>-</td><td>Wear helm</td><td>H</td><td>Numpad 1-6</td>
                                    </tr>
                                <tr>
                                    <td>Talking with quest giver</td><td>O</td><td>-</td><td>Wear weapon</td><td>W</td><td>Numpad 1-6</td>
                                </tr>
                                <tr>
                                    <td>Take an item from the ground</td><td>T</td><td>-</td><td>Wear offhand</td><td>O</td><td>Numpad 1-6</td>
                                </tr>
                                <tr>
                                    <td>Attack the enemy</td><td>A</td><td>-</td><td>Wear boots</td><td>B</td><td>Numpad 1-6</td>
                                </tr>
                                <tr>
                                    <td>Destroy the item</td><td>D</td><td>-</td><td colspan='3' style='color: gold'><b>Skill window</b></td>
                                </tr>
                                <tr>
                                    <td>Character info window</td><td>C</td><td>-</td><td>Navigation</td><td>Arrows</td><td>-</td>
                                </tr>
                                <tr>
                                    <td>Use inventory item slot 1-6</td><td>Numpad 1-6</td><td>-</td><td>Improve Skill</td><td>Enter</td><td>-</td>
                                </tr>
                                <tr>
                                    <td>Use a class skill 1</td><td>Q</td><td>-</td><td colspan='3' style='color: gold'><b>Fight</b></td>
                                </tr>
                                <tr>
                                    <td>Use a class skill 2</td><td>W</td><td>-</td><td>Use a class skill 1</td><td>Q</td><td>-</td>
                                </tr>
                                <tr>
                                    <td>Use a class skill 3</td><td>E</td><td>-</td><td>Use a class skill 2</td><td>W</td><td>-</td>
                                </tr>
                                <tr>
                                    <td>Use a class skill 4</td><td>R</td><td>-</td><td>Use a class skill 3</td><td>E</td><td>-</td>
                                </tr>
                                <tr>
                                    <td>Use a main skill 1</td><td>Q</td><td>Shift</td><td>Use a class skill 4</td><td>R</td><td>-</td>
                                </tr>
                                <tr>
                                    <td>Use a main skill 2</td><td>W</td><td>Shift</td><td>Weapon attack</td><td>A</td><td>-</td>
                                </tr>
                                <tr>
                                    <td>Use a main skill 3</td><td>E</td><td>Shift</td><td>Defend</td><td>D</td><td>-</td>
                                </tr>
                                <tr>
                                    <td>Use a main skill 4</td><td>R</td><td>Shift</td><td>Try to escape</td><td>S</td><td>-</td>
                                </tr>
                                <tr>
                                    <td colspan='3' style='color: gold'><b>Character info window</b></td><td colspan='3' style='color: gold'><b>Perk window turning pages</b></td>
                                </tr>
                                <tr>
                                    <td>Perk window</td><td>P</td><td>-</td><td>Previous page</td><td>Left Arrow</td><td>-</td>
                                </tr>
                                <tr>
                                    <td>Quest window</td><td>Q</td><td>-</td><td>Next page</td><td>Right Arrow</td><td>-</td>
                                </tr>
                                <tr>
                                    <td>Skill window</td><td>S</td><td>-</td><td colspan='3' style='color: gold'><b>Quest window turning pages</b></td>
                                </tr>
                                <tr>
                                    <td>Item window</td><td>I</td><td>-</td><td>Previous page</td><td>Left Arrow</td><td>-</td>
                                </tr>
                                <tr>
                                    <td>Close info window</td><td>Escape</td><td>-</td><td>Next page</td><td>Right Arrow</td><td>-</td>
                                </tr>
								</table>";
                            byte[] buf = System.Text.Encoding.UTF8.GetBytes(b);                            
                            f.Write(buf, 0, buf.Length);

                        }
                    }
                }
            }
        }
        /// <summary>
        /// Judge! Was created for special AI--object who will see battle
        /// <example><b>Not did yet</b></example>
        /// </summary>
        public class Judge
        {
            /// <summary>
            /// Enable or disable Judge
            /// </summary>
            public bool Enabled
            {
                set
                { _Enabled = value; if (_Enabled) { Watch(); } else { Stop(); } }
            }
            /// <summary>
            /// Real field
            /// </summary>
            private bool _Enabled;
            /// <summary>
            /// Judge start watch
            /// </summary>
            private void Watch()
            {
                JudgeEye.Interval = 45000;
                JudgeEye.Elapsed += Judgement;
                JudgeEye.Enabled = true;
            }
            /// <summary>
            /// Judge eyes
            /// </summary>
            public System.Timers.Timer JudgeEye = new System.Timers.Timer();
            /// <summary>
            /// Judgement
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void Judgement(object sender, EventArgs e)
            {
                if (Rogue.RAM.Enemy != null)
                {
                    if (Rogue.RAM.Enemy.CHP > 0)
                    {
                        if (Rogue.RAM.Enemy.CHP == LastHp) { PlayEngine.GamePlay.EnemyAttack(); }
                        LastHp = Rogue.RAM.Enemy.CHP;
                    }
                }
            }
            /// <summary>
            /// Last enemy hit points at snapshot
            /// </summary>
            private int LastHp;
            /// <summary>
            /// stop judgement
            /// </summary>
            private void Stop() 
            { JudgeEye.Enabled = false; }
        }
        /// <summary>
        /// For my timers
        /// </summary>
        public class Timers
        { public bool Activated; public TimerType Type;  }
        /// <summary>
        /// Type of my timer
        /// </summary>
        public enum TimerType
        {
            DoT=0,
            HoT=1,
            Buf=2,
            Deb=3,
            Sum=4,
            Smd=5,
            Que=6,
            Pro=7
        }
        /// <summary>
        /// Save or Load files
        /// </summary>
        public class SaveLoadFile
        {
            public string Path;
            public string Title;
            public string Status
            {
                get
                {
                    if (this.Path == null || this.Title == null)
                    { return "Состояние: файл поврежден"; }
                    else
                    { return "Состояние: файл целый"; }
                }
            }
            public char Icon;
            public ConsoleColor Color;
            public ConsoleColor FileStatus
            {
                get
                {
                    if (this.Path == null || this.Title == null)
                    { return ConsoleColor.Red; }
                    else
                    { return ConsoleColor.Green; }
                }
            }
            public ConsoleColor FileType
            {
                get
                {
                    if (this.Title == "Файл сохранения") { return ConsoleColor.DarkCyan; }
                    if (this.Title == "Бонус контент") { return ConsoleColor.DarkYellow; }
                    if (this.Title == "Фанатский модуль") { return ConsoleColor.DarkMagenta; }
                    return ConsoleColor.Black;
                }
            }
        }

        public class ArmorSet
        { public string Name; public bool Active;}

        public class AbilityShadow
        {
            public List<MechEngine.Ability> Abilityes = new List<MechEngine.Ability>(); public int Level = 0;
        }

        public class QuestFlag
        {
            public bool Gregory = false;
            public bool Valery = true;
            public bool Stephan = true;
            public bool Klara = false;
            public bool KlaraHelp = false;
            public bool Sara = false;
            public bool SaraHelp = false;
            public int UndeadRising = 0;
            public bool Retronslate = false;
            public bool Scrijal = false;
            public bool TransformAltar = false;
            public bool AllTransform = false;
            public bool VirgilEnd = false;
        }

        public static int score
        {
            get
            {
                return 0;//return Convert.ToInt32(Rogue.RAM.Player.Level * 0.1535);
            }
        }
    }
}

