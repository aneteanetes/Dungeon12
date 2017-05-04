using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;

namespace Rogue
{
    public static class PlayEngine
    {
        private static Random r = new Random();
        public static bool Enemy
        {
            set
            {
                if (value)
                {
                    if (!EnemyMoves.CheckMove) { Console.BackgroundColor = ConsoleColor.Black; EnemyMoves.Move(true); }
                }
                else
                {
                    if (EnemyMoves.CheckMove) { Console.BackgroundColor = ConsoleColor.Black; EnemyMoves.Move(false); }
                }
            }
        }

        public static class Menu
        {
            public static bool MainMenu
            {
                get
                {
                    Console.Clear();
                    DrawEngine.SplashScreen.MainScreen();
                    DrawMenu();
                    bool suka = false;
                    while (!suka)
                    {
                        ConsoleKey push = Console.ReadKey(true).Key;
                        switch (push)
                        {
                            case ConsoleKey.UpArrow: { if (Position != 0) { Position--; DrawMenu(); } break; }
                            case ConsoleKey.DownArrow: { if (Position != 3) { Position++; DrawMenu(); } break; }
                            case ConsoleKey.Enter:
                                {
                                    switch (Position)
                                    {
                                        case 0:
                                            {
                                                DrawEngine.ConsoleDraw.WriteTitle("Начало новой игры...\n \n Нажмите любую клавишу для продолжения...");
                                                PlayEngine.GamePlay.NewGame.CharacterCreation();
                                                suka = true;
                                                return true;
                                            }
                                        case 1:
                                            {
                                                PlayEngine.GamePlay.NewGame.CharacterCreation(true);
                                                return true;
                                            }
                                        case 2: { break; }
                                        case 3:
                                            {
                                                DrawEngine.ConsoleDraw.WriteTitle("Досвидания!");
                                                Thread.Sleep(100);
                                                Environment.Exit(0);
                                                return false;
                                            }
                                    }
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                    return false;
                }
            }
            private static int Position = 0;
            private static void DrawMenu()
            {
                //Console.Clear();
                DrawEngine.StringMenu m = new DrawEngine.StringMenu();
                m.Logo = new DrawEngine.ColoredWord() { Word = "Dungeon 12", Color = ConsoleColor.DarkCyan };
                m.Additional = new DrawEngine.ColoredWord() { Word = "Альфа", Color = ConsoleColor.Red };
                m.Addon = new DrawEngine.ColoredWord() { Word = "[You are not alone]", Color = ConsoleColor.Magenta };
                m.Options = new List<string>() { "Новая игра", "Быстрая игра", "Создатели", "Выход" };
                m.OptionsColor = ConsoleColor.Gray;
                m.OptionsColorBackground = ConsoleColor.Black;
                m.OptionsColorSelected = ConsoleColor.White;
                m.OptionsColorSelectedBackground = ConsoleColor.DarkGray;
                m.Index = Position;
                DrawEngine.StringMenus.DrawMenu = m;                
            }

            public static bool InGameMenu
            {
                get
                {
                    Console.Clear();
                    DrawEngine.SplashScreen.MainScreen();
                    DrawInGameMenu();
                    bool suka = false;
                    while (!suka)
                    {
                        ConsoleKey push = Console.ReadKey(true).Key;
                        switch (push)
                        {
                            case ConsoleKey.UpArrow: { if (Position != 0) { Position--; DrawInGameMenu(); } break; }
                            case ConsoleKey.DownArrow: { if (Position != 2) { Position++; DrawInGameMenu(); } break; }
                            case ConsoleKey.Enter:
                                {
                                    switch (Position)
                                    {
                                        case 0:
                                            {
                                                Rogue.Main(new string[0]);
                                                return true;
                                            }
                                        case 1:
                                            {
                                                InGameSettings.Keymap(); return false;
                                            }
                                        case 2:
                                            {
                                                DrawEngine.ConsoleDraw.WriteTitle("Досвидания!");
                                                Thread.Sleep(100);
                                                Environment.Exit(0);
                                                return false;
                                            }
                                    }
                                    break;
                                }
                            case ConsoleKey.Escape:
                                {
                                    if (Rogue.RAM.Map.Name.IndexOf("Мраумир") != -1) { SoundEngine.Music.TownTheme(); }
                                    else
                                    { SoundEngine.Music.DungeonTheme(); }
                                    Console.Clear();
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    DrawEngine.GUIDraw.DrawGUI();
                                    DrawEngine.GUIDraw.drawstat();
                                    return true;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                    return false;
                }
            }
            private static void DrawInGameMenu()
            {
                if (Position > 2) { Position = 0; }
                DrawEngine.StringMenu m = new DrawEngine.StringMenu();
                m.Logo = new DrawEngine.ColoredWord() { Word = "Dungeon 12", Color = ConsoleColor.DarkCyan };
                m.Additional = new DrawEngine.ColoredWord() { Word = "Альфа", Color = ConsoleColor.Red };
                m.Addon = new DrawEngine.ColoredWord() { Word = "[You are not alone]", Color = ConsoleColor.Magenta };
                m.Options = new List<string>() { "Главное меню", "Глобальный лог", "Выход" };
                m.OptionsColor = ConsoleColor.Gray;
                m.OptionsColorBackground = ConsoleColor.Black;
                m.OptionsColorSelected = ConsoleColor.White;
                m.OptionsColorSelectedBackground = ConsoleColor.DarkGray;
                m.Index = Position;
                DrawEngine.StringMenus.DrawMenu = m;
            }

            public static void LoadM()
            {
                //bool lost = false;
                //DrawEngine.SaveLoadWindows.DrawLoad = true;
                //ConsoleKey push = Console.ReadKey(true).Key;

                //try
                //{
                //    switch (push)
                //    {
                //        case ConsoleKey.D1:
                //        case ConsoleKey.NumPad1: { string f = Rogue.RAM.LoadFiles[0].Path; lost = true; API.SaveLoad.Load = f;  break; }
                //        case ConsoleKey.D2:
                //        case ConsoleKey.NumPad2: { string f = Rogue.RAM.LoadFiles[1].Path; lost = true; API.SaveLoad.Load = f; break; }
                //        case ConsoleKey.D3:
                //        case ConsoleKey.NumPad3: { string f = Rogue.RAM.LoadFiles[2].Path; lost = true; API.SaveLoad.Load = f; break; }
                //        case ConsoleKey.D4:
                //        case ConsoleKey.NumPad4: { string f = Rogue.RAM.LoadFiles[3].Path; lost = true; API.SaveLoad.Load = f; break; }
                //        case ConsoleKey.D5:
                //        case ConsoleKey.NumPad5: { string f = Rogue.RAM.LoadFiles[4].Path; lost = true; API.SaveLoad.Load = f; break; }
                //        case ConsoleKey.D6:
                //        case ConsoleKey.NumPad6: { string f = Rogue.RAM.LoadFiles[5].Path; lost = true; API.SaveLoad.Load = f; break; }
                //        //case ConsoleKey.Escape: { MainMenu(); break; }
                //        default: { LoadM(); break; }
                //    }
                //}
                //catch { }
                //if (!lost) { LoadM(); }
                //else { return; }
            }

            private static void GoAfterLoad()
            {
                
                //DrawEngine.GUIDraw.DrawGUI();
                //LabirinthEngine.Create(1);
                //GUI.EnemyMoves.En = null;
                //GUI.EnemyMoves.Move(true);
                //GUI.GamePlay.Play();
            }

            public static void SettingsMenu()
            {
                DrawEngine.ConsoleDraw.WriteMenu("Убийственные цвета миров {Z} - " + Rogue.RAM.ColorSet.DeadEyesColors + "\nМузыка и звук {M} - " + Rogue.RAM.SoundSet.Volume.ToString() + "\n\nВернуться {ESC}");
                ConsoleKey push = Console.ReadKey(true).Key;
                switch (push)
                {
                    case ConsoleKey.Z:
                        {
                            DrawEngine.ConsoleDraw.WriteTitle("Y/N?");
                            ConsoleKey pushpush = Console.ReadKey(true).Key;
                            switch (pushpush)
                            {
                                case ConsoleKey.Y:
                                    { Rogue.RAM.ColorSet.DeadEyesColors = "true"; SettingsMenu(); break; }
                                case ConsoleKey.N:
                                    { Rogue.RAM.ColorSet.DeadEyesColors = "0"; SettingsMenu(); break; }
                                default:
                                    { SettingsMenu(); break; }
                            }
                            break;
                        }
                    case ConsoleKey.M:
                        {
                            DrawEngine.ConsoleDraw.WriteTitle("Введите уровень звука от 0.0 до 10.0:");
                            string volume = DrawEngine.ConsoleDraw.ReadCenter();
                            Rogue.RAM.SoundSet.Volume = (float)Convert.ToDouble(volume);
                            SoundEngine.Settings.Volume = Rogue.RAM.SoundSet.Volume;
                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            bool wtf = MainMenu;
                            Console.ReadKey(true);
                            break;
                        }
                    default:
                        {
                            SettingsMenu();
                            break;
                        }
                }
            }

            public static bool History
            {
                set
                {
                    if (value)
                    {
                        DrawEngine.SplashScreen.History_part1();
                    }
                }
            }

            public static void ChooseClass()
            {
                Console.Clear();
                Position = 0;
                DrawChooseClass();
                bool pick = false;
                while (!pick)
                {
                    ConsoleKey push = Console.ReadKey(true).Key;
                    switch (push)
                    {
                        case ConsoleKey.UpArrow: { if (Position != 0) { Position--; DrawChooseClass(); } break; }
                        case ConsoleKey.DownArrow: { if (Position != 9) { Position++; DrawChooseClass(); } break; }
                        case ConsoleKey.Enter:
                            {
                                switch (Position)
                                {
                                    case 0: { Rogue.RAM.Player.Class = MechEngine.BattleClass.BloodMage; break; }
                                    case 1: { Rogue.RAM.Player.Class = MechEngine.BattleClass.Paladin; break; }
                                    case 2: { Rogue.RAM.Player.Class = MechEngine.BattleClass.Alchemist; break; }
                                    case 3: { Rogue.RAM.Player.Class = MechEngine.BattleClass.Assassin; break; }
                                    case 4: { Rogue.RAM.Player.Class = MechEngine.BattleClass.FireMage; break; }
                                    case 5: { Rogue.RAM.Player.Class = MechEngine.BattleClass.Inquisitor; break; }
                                    case 6: { Rogue.RAM.Player.Class = MechEngine.BattleClass.Monk; break; }
                                    case 7: { Rogue.RAM.Player.Class = MechEngine.BattleClass.Necromant; break; }
                                    case 8: { Rogue.RAM.Player.Class = MechEngine.BattleClass.Shaman; break; }
                                    case 9: { Rogue.RAM.Player.Class = MechEngine.BattleClass.Warrior; break; }
                                }
                                Rogue.RAM.Player.GetClassRacePerk(2);
                                pick = true;
                                break;
                            }
                    }
                }
            }
            private static void DrawChooseClass()
            {
                DrawEngine.StringMenu m = new DrawEngine.StringMenu();
                m.Logo = new DrawEngine.ColoredWord() { Word = "Dungeon 12", Color = ConsoleColor.DarkCyan };
                m.Additional = new DrawEngine.ColoredWord() { Word = "Альфа", Color = ConsoleColor.Red };
                m.Addon = new DrawEngine.ColoredWord() { Word = "Выберите класс персонажа:", Color = ConsoleColor.Gray };
                m.Options = new List<string>() { "Маг Крови", "Паладин", "Алхимик", "Разбойник", "Маг Огня", "Экзорцист", "Монах", "Некромант", "Шаман", "Воин" };
                m.OptionsColor = ConsoleColor.Gray;
                m.OptionsColorBackground = ConsoleColor.Black;
                m.OptionsColorSelected = ConsoleColor.White;
                m.OptionsColorSelectedBackground = ConsoleColor.DarkGray;
                m.Index = Position;
                DrawEngine.StringMenus.DrawMenu = m;
            }

            public static void ChooseRace()
            {
                Console.Clear();
                Position = 0;
                DrawChooseRace();
                bool pick = false;
                while (!pick)
                {
                    ConsoleKey push = Console.ReadKey(true).Key;
                    switch (push)
                    {
                        case ConsoleKey.UpArrow: { if (Position != 0) { Position--; DrawChooseRace(); } break; }
                        case ConsoleKey.DownArrow: { if (Position != 9) { Position++; DrawChooseRace(); } break; }
                        case ConsoleKey.Enter:
                            {
                                switch (Position)
                                {
                                    case 0: { Rogue.RAM.Player.Race = MechEngine.Race.Human; break; }
                                    case 1: { Rogue.RAM.Player.Race = MechEngine.Race.Elf; break; }
                                    case 2: { Rogue.RAM.Player.Race = MechEngine.Race.DarkElf; break; }
                                    case 3: { Rogue.RAM.Player.Race = MechEngine.Race.Dwarf; break; }
                                    case 4: { Rogue.RAM.Player.Race = MechEngine.Race.FallenAngel; break; }
                                    case 5: { Rogue.RAM.Player.Race = MechEngine.Race.Gnome; break; }
                                    case 6: { Rogue.RAM.Player.Race = MechEngine.Race.MoonElf; break; }
                                    case 7: { Rogue.RAM.Player.Race = MechEngine.Race.Orc; break; }
                                    case 8: { Rogue.RAM.Player.Race = MechEngine.Race.Troll; break; }
                                    case 9: { Rogue.RAM.Player.Race = MechEngine.Race.Undead; break; }
                                }
                                Rogue.RAM.Player.GetClassRacePerk(1);
                                pick = true;
                                break;
                            }
                    }
                }
            }
            private static void DrawChooseRace()
            {
                DrawEngine.StringMenu m = new DrawEngine.StringMenu();
                m.Logo = new DrawEngine.ColoredWord() { Word = "Dungeon 12", Color = ConsoleColor.DarkCyan };
                m.Additional = new DrawEngine.ColoredWord() { Word = "Альфа", Color = ConsoleColor.Red };
                m.Addon = new DrawEngine.ColoredWord() { Word = "Выберите расу персонажа:", Color = ConsoleColor.Gray };
                m.Options = new List<string>() { "Человек","Азрай","Дроу","Дварф","Падший","Гном","Калдорай","Орк","Тролль","Нежить" };
                m.OptionsColor = ConsoleColor.Gray;
                m.OptionsColorBackground = ConsoleColor.Black;
                m.OptionsColorSelected = ConsoleColor.White;
                m.OptionsColorSelectedBackground = ConsoleColor.DarkGray;
                m.Index = Position;
                DrawEngine.StringMenus.DrawMenu = m;
            }
            
            /// <summary>
            /// Uses for draw menu from ability
            /// </summary>
            /// <param name="Name">Name of draw ability</param>
            public static void AbilityMenu(string Name) 
            {
                if (Name == "Banish") { GamePlay.GUIa.Banish(); }
                if (Name == "Trap") { GamePlay.GUIa.Trap(); }            
            }

            public static void Bug()
            {
                DrawEngine.ConsoleDraw.WriteTitle("Ваше имя для баг репорта:");
                SystemEngine.BugReport Bug = new SystemEngine.BugReport();
                Bug.Author = "";
                while (Bug.Author == "")
                {
                    Bug.Author = DrawEngine.ConsoleDraw.ReadCenter();
                }
                BugReportMenu(Bug);
            }

            public static void BugReportMenu(SystemEngine.BugReport Bug)
            {
                DrawEngine.ConsoleDraw.WriteMenu("Выберите категорию:\n\n\n\nОшибка: Способности [A]\n\nОшибка: Отрисовка [D]\n\nОшибка: Предметы [I]\n\nОшибка: Другое [O]\n\nБаланс: Способности [Q]\n\nБаланс: Враги [W]\n\nБаланс: Предметы [E]\n\nДругое: Опечатка [P]\n\n Другое: Своё [N]");
                ConsoleKey push = Console.ReadKey(true).Key;                
                switch (push)
                {
                    case ConsoleKey.A: { Bug.Category = "Bug: Ability"; BugReportTitle(Bug); break; }
                    case ConsoleKey.D: { Bug.Category = "Bug: Drawning"; BugReportTitle(Bug); break; }
                    case ConsoleKey.I: { Bug.Category = "Bug: Item"; BugReportTitle(Bug); break; }
                    case ConsoleKey.O: { Bug.Category = "Bug: Other"; BugReportTitle(Bug); break; }
                    case ConsoleKey.Q: { Bug.Category = "Balance: Ability"; BugReportTitle(Bug); break; }
                    case ConsoleKey.W: { Bug.Category = "Balance: Enemy"; BugReportTitle(Bug); break; }
                    case ConsoleKey.R: { Bug.Category = "Balance: Item"; BugReportTitle(Bug); break; }
                    case ConsoleKey.P: { Bug.Category = "Other: Text"; BugReportTitle(Bug); break; }
                    case ConsoleKey.N: { Bug.Category = "Other: My"; BugReportTitle(Bug); break; }
                    default: { BugReportMenu(Bug); break; }
                }
            }

            public static void BugReportTitle(SystemEngine.BugReport Bug)
            {
                DrawEngine.ConsoleDraw.WriteTitle("Название проблемы: опишите суть ошибки.\n\nПример: <<Стрела огня: Высокий урон>>\n\n\n\nНажмите любую клавишу для продолжения...");
                Console.ReadKey(true);
                DrawEngine.ConsoleDraw.WriteTitle("Проблема:");
                Bug.Title = "";
                while (Bug.Title == "")
                {
                    Bug.Title = DrawEngine.ConsoleDraw.ReadCenter();
                }
                BugReportText(Bug);
            }

            public static void BugReportText(SystemEngine.BugReport Bug)
            {
                DrawEngine.ConsoleDraw.WriteTitle("Описание проблемы: опишите суть проблемы.\n\nПример: <<При использовании способности 'Стрела огня' врагу наносится слишком большой урон, \n\n800 повреждений на 1 уровне. Проблема видна только если персонаж уже умерал.>>\n\n\n\nНажмите любую клавишу для продолжения...");
                Console.ReadKey(true);
                DrawEngine.ConsoleDraw.WriteTitle("Описание проблемы:\n\n(5 строк [5 Enter'ов])");
                Bug.Text +="\n"+Console.ReadLine();
                Bug.Text += "\n" + Console.ReadLine();
                Bug.Text += "\n" + Console.ReadLine();
                Bug.Text += "\n" + Console.ReadLine();
                Bug.Text += "\n" + Console.ReadLine();
                Console.ReadKey(false);
                BugReportSend(Bug);
            }

            public static void BugReportSend(SystemEngine.BugReport Bug)
            {
                DrawEngine.ConsoleDraw.WriteTitle("Составление баг-репорта завершено, нажмите Y для окончания, и N для отмены.");
                ConsoleKey push = Console.ReadKey(true).Key;
                switch (push)
                {
                    case ConsoleKey.Y: { SystemEngine.Helper.Net.SendBugReport(Bug); DrawEngine.ConsoleDraw.WriteTitle("Спасибо за помощь, авторы самых полезных репортов получат уникальные вещи и способности."); Thread.Sleep(1500); DrawEngine.ConsoleDraw.WriteTitle("Игра перезапускается..."); Thread.Sleep(200); Environment.Exit(0); break; }
                    case ConsoleKey.N: { SystemEngine.Helper.Net.SendBugReport(Bug); DrawEngine.ConsoleDraw.WriteTitle("Вы - редиска."); Thread.Sleep(1000); Environment.Exit(0); break; }
                }
            }
        }

        public static class EnemyMoves
        {
            public static Thread En;       

            public static void Move(bool m,bool ReDrawMap=true,bool ReDrawOther=true)
            {
                if (m)
                {
                    En = null;
                    if (Rogue.RAM.Map._Name != "Мраумир" && Rogue.RAM.Map._Name!="Святилище") { En = new Thread(PlayEngine.GamePlay.EnemyMove); }
                    else { En = new Thread(PlayEngine.GamePlay.NpcMove); }
                    if (ReDrawMap) { DrawEngine.GUIDraw.DrawLab(); }
                    if (ReDrawOther)
                    {
                        DrawEngine.GUIDraw.ReDrawCharStat();
                        DrawEngine.CharMap.DrawCMap(new List<string>() 
                                { 
                                    "[O] - Действие",
                                    "[M] - Карта ", 
                                    "[T] - Взять вещь ",
                                    "[A] - Атаковать ",
                                    "[С] - Персонаж ",
                                    "[I] - Идентификация ",
                                    "[1-6] - Инвентарь ",
                                    "[Q,W,E,R],+[Shift] - Навыки",
                                });
                    }
                    Rogue.RAM.EnemyBool = false;
                    System.Object lockThis = new System.Object();
                    lock (lockThis)
                    {
                        En.Start();
                    }
                    //DrawEngine.GUIDraw.ReDrawMapWindow();
                }
                else
                {
                    if (Rogue.RAM.EnemyBool)
                    { GamePlay.EnemyAttack(Rogue.RAM.EnemyX, Rogue.RAM.EnemyY); }
                    try
                    {
                        En.Abort();
                        En = null;
                    }
                    catch (ThreadAbortException)
                    {
                        Thread.Sleep(10);
                    }
                    catch (NullReferenceException) { }
                    
                }
            }
            /// <summary>
            /// If moves = true, if they stoped = false
            /// </summary>
            /// <returns></returns>
            public static bool CheckMove
            { get { if (En != null) { return true; } else { return false; } } }
        }

        public static class InGameSettings
        {
            public static void Enter()
            {
                if (PlayEngine.EnemyMoves.CheckMove) { PlayEngine.EnemyMoves.Move(false); }
                DrawEngine.ConsoleDraw.WriteMenu("Главное меню {M}\n\nСохранить игру {S}\n\nЗагрузить игру {L}\n\nГлобальный лог {K}\n\nВыйти {E}");
                DrawEngine.ConsoleDraw.WriteAdditionalInformation("Альфа", ConsoleColor.Red);
                DrawEngine.ConsoleDraw.WriteAdditionalLogo("Dungeon 12", ConsoleColor.DarkCyan);
                //DrawEngine.ConsoleDraw.WriteMenu("Сохранить игру {S}\n\nЗагрузить игру {L}\n\nКарта клавиш {K}\n\nВыйти {E}\n\n\n\nБаг репорт {B}");
                ConsoleKey push = Console.ReadKey(true).Key;
                switch (push)
                {
                    case ConsoleKey.M:
                        {
                            SystemEngine.Helper.Disposing.DisposeTimers = true;
                            //Environment.Exit(0);
                            Rogue.Main(new string[0]);
                            break;
                        }
                    case ConsoleKey.S:
                        {
                            Save();
                            break;
                        }
                    case ConsoleKey.L:
                        {
                            Load();
                            //bool wtf = false; while (!wtf) { wtf = Menu.InGameMenu; }
                            
                            break;
                        }
                    case ConsoleKey.K: { Keymap(); break; }
                    case ConsoleKey.E:
                        {
                            DrawEngine.ConsoleDraw.WriteTitle("Досвидания!");
                            Thread.Sleep(1000);
                            Environment.Exit(0);
                            break;
                        }
                    //case ConsoleKey.B:
                    //    {
                    //        DrawEngine.ConsoleDraw.WriteTitle("Текущая игра не будет сохранена, продолжить?\n\n\n Y/N?");
                    //        push = Console.ReadKey(true).Key;
                    //        if (push == ConsoleKey.Y) { PlayEngine.Menu.Bug(); }
                    //        else { Enter(); }
                    //        return;
                    //    }
                    case ConsoleKey.Escape:
                        {
                            DrawEngine.GUIDraw.DrawGUI();
                            PlayEngine.GamePlay.Play();
                            break;
                        }
                    default:
                        {
                            bool wtf = false; while (!wtf) { wtf = Menu.InGameMenu; }
                            break;
                        }
                }
                Enter();
                
            }

            public static void Keymap()
            {
                DrawEngine.FightDraw.DrawGlobalLog();
                bool w = false;
                while (!w)
                {
                    ConsoleKey push = Console.ReadKey(true).Key;
                    switch (push)
                    {
                        default:
                            {
                                w = true;
                                if (Rogue.RAM.Map.Name.IndexOf("Мраумир") != -1) { SoundEngine.Music.TownTheme(); }
                                else
                                { SoundEngine.Music.DungeonTheme(); }
                                Console.Clear();
                                Console.BackgroundColor = ConsoleColor.Black;
                                DrawEngine.GUIDraw.DrawGUI();
                                DrawEngine.GUIDraw.drawstat();
                                break;
                            }
                    }
                }
            }

            public static void Save()
            {
                //DrawEngine.SaveLoadWindows.DrawSave = true;
                //ConsoleKey push = Console.ReadKey(true).Key;
                //try
                //{
                //    switch (push)
                //    {
                //        case ConsoleKey.D1:
                //        case ConsoleKey.NumPad1: { DrawEngine.ConsoleDraw.WriteTitle("Введите имя файла."); string filena = DrawEngine.ConsoleDraw.ReadCenter(false,true); SystemEngine.Helper.File.DeleteFile = filena; API.SaveLoad.Save = filena; DrawEngine.ConsoleDraw.WriteTitle("Игра сохранена в файл: Save\\" + filena + ".d12ml"); Thread.Sleep(2000); break; }
                //        case ConsoleKey.D2:
                //        case ConsoleKey.NumPad2: { DrawEngine.ConsoleDraw.WriteTitle("Введите имя файла."); string filena = DrawEngine.ConsoleDraw.ReadCenter(false, true); SystemEngine.Helper.File.DeleteFile = filena; API.SaveLoad.Save = filena; DrawEngine.ConsoleDraw.WriteTitle("Игра сохранена в файл: Save\\" + filena + ".d12ml"); Thread.Sleep(2000); break; }
                //        case ConsoleKey.D3:
                //        case ConsoleKey.NumPad3: { DrawEngine.ConsoleDraw.WriteTitle("Введите имя файла."); string filena = DrawEngine.ConsoleDraw.ReadCenter(false, true); SystemEngine.Helper.File.DeleteFile = filena; API.SaveLoad.Save = filena; DrawEngine.ConsoleDraw.WriteTitle("Игра сохранена в файл: Save\\" + filena + ".d12ml"); Thread.Sleep(2000); break; }
                //        case ConsoleKey.D4:
                //        case ConsoleKey.NumPad4: { DrawEngine.ConsoleDraw.WriteTitle("Введите имя файла."); string filena = DrawEngine.ConsoleDraw.ReadCenter(false, true); SystemEngine.Helper.File.DeleteFile = filena; API.SaveLoad.Save = filena; DrawEngine.ConsoleDraw.WriteTitle("Игра сохранена в файл: Save\\" + filena + ".d12ml"); Thread.Sleep(2000); break; }
                //        case ConsoleKey.D5:
                //        case ConsoleKey.NumPad5: { DrawEngine.ConsoleDraw.WriteTitle("Введите имя файла."); string filena = DrawEngine.ConsoleDraw.ReadCenter(false, true); SystemEngine.Helper.File.DeleteFile = filena; API.SaveLoad.Save = filena; DrawEngine.ConsoleDraw.WriteTitle("Игра сохранена в файл: Save\\" + filena + ".d12ml"); Thread.Sleep(2000); break; }
                //        case ConsoleKey.D6:
                //        case ConsoleKey.NumPad6: { DrawEngine.ConsoleDraw.WriteTitle("Введите имя файла."); string filena = DrawEngine.ConsoleDraw.ReadCenter(false, true); SystemEngine.Helper.File.DeleteFile = filena; API.SaveLoad.Save = filena; DrawEngine.ConsoleDraw.WriteTitle("Игра сохранена в файл: Save\\" + filena + ".d12ml"); Thread.Sleep(2000); break; }
                //        case ConsoleKey.Escape: { Enter(); break; }
                //        default: { Save(); break; }
                //    }
                //}
                //catch { }
                //Save();
            }

            public static void Load()
            {
                //bool lost = false;
                //DrawEngine.SaveLoadWindows.DrawLoad = true;
                //ConsoleKey push = Console.ReadKey(true).Key;

                //try
                //{
                //    switch (push)
                //    {
                //        case ConsoleKey.D1:
                //        case ConsoleKey.NumPad1: { string f = Rogue.RAM.LoadFiles[0].Path; lost = true; API.SaveLoad.Load = f;  break; }
                //        case ConsoleKey.D2:
                //        case ConsoleKey.NumPad2: { string f = Rogue.RAM.LoadFiles[1].Path; lost = true; API.SaveLoad.Load = f; break; }
                //        case ConsoleKey.D3:
                //        case ConsoleKey.NumPad3: { string f = Rogue.RAM.LoadFiles[2].Path;lost = true; API.SaveLoad.Load = f;  break; }
                //        case ConsoleKey.D4:
                //        case ConsoleKey.NumPad4: { string f = Rogue.RAM.LoadFiles[3].Path; lost = true; API.SaveLoad.Load = f; break; }
                //        case ConsoleKey.D5:
                //        case ConsoleKey.NumPad5: { string f = Rogue.RAM.LoadFiles[4].Path; lost = true; API.SaveLoad.Load = f;  break; }
                //        case ConsoleKey.D6:
                //        case ConsoleKey.NumPad6: { string f = Rogue.RAM.LoadFiles[5].Path;  lost = true; API.SaveLoad.Load = f; break; }
                //        case ConsoleKey.Escape: { Enter(); break; }
                //        default: { Load(); break; }
                //    }
                //}
                //catch { }
                //if (!lost) { Load(); }
                //else { return; }
            }
        }

        public static class GamePlay
        {
            public static class NewGame
            {
                public static void CharacterCreation(bool Auto = false)
                {

                    #region Name

                    MechEngine.InventoryTab tab = new MechEngine.InventoryTab();
                    tab.Type = "Perk";
                    tab.NowTab = 1;
                    tab.MaxTab = 1;
                    Rogue.RAM.iTab = tab;
                    Rogue.RAM.PopUpTab = new MechEngine.InventoryTab();
                    Rogue.RAM.Player = new MechEngine.Character();
                    Rogue.RAM.Player.Perks = new List<MechEngine.Perk>();
                    Rogue.RAM.Player.CraftAbility = DataBase.OtherAbilityBase.BaseSet();
                    MechEngine.Character My = Rogue.RAM.Player;
                    if (!Auto)
                    {
                        //Console.Clear();
                        //DrawEngine.StringMenu m = new DrawEngine.StringMenu();
                        //m.Logo = new DrawEngine.ColoredWord() { Word = "Dungeon 12", Color = ConsoleColor.DarkCyan };
                        //m.Additional = new DrawEngine.ColoredWord() { Word = "Альфа", Color = ConsoleColor.Red };
                        //m.Addon = new DrawEngine.ColoredWord() { Word = "Введите имя персонажа:", Color = ConsoleColor.Gray };
                        //m.Options = new List<string>();
                        //m.OptionsColor = ConsoleColor.Gray;
                        //m.OptionsColorBackground = ConsoleColor.Black;
                        //m.OptionsColorSelected = ConsoleColor.White;
                        //m.OptionsColorSelectedBackground = ConsoleColor.DarkGray;
                        //m.Index = 0;
                        //DrawEngine.StringMenus.DrawMenu = m;

                        ////DrawEngine.ConsoleDraw.WriteTitle("Введите имя персонажа.");
                        //DrawEngine.ConsoleDraw.CursorCenter();
                        //while (My.Name == "" || My.Name == null || My.Name == string.Empty)
                        //{
                        //    My.Name = DrawEngine.ConsoleDraw.ReadCenter();
                        //}
                        while (Rogue.RAM.Player.Name == "" || Rogue.RAM.Player.Name == null || Rogue.RAM.Player.Name == string.Empty)
                        {
                            MenuEngine.EnterName.Draw(); try { if (Rogue.RAM.Player.Name.Length > 15) { Rogue.RAM.Player.Name = ""; } }
                            catch { }
                        }
                    }
                    else { My.Name = "Странник"; }

                    //My.Name = Console.ReadLine();

                    #endregion

                    #region New Graphic Settings

                    Rogue.RAM.Player.Icon = Rogue.RAM.GraphHeroSet.Icon;
                    Rogue.RAM.Player.Color = Rogue.RAM.GraphHeroSet.Color;

                    #endregion

                    #region Race
                    if (!Auto)
                    {
                        //DrawEngine.ConsoleDraw.WriteMenu("Выберите расу персонажа:\n\nЧеловек{H}\n\nАзрай{E}\n\nДроу{R}\n\nДварф{D}\n\nПадший{F}\n\nГном{G}\n\nКалдорай{M}\n\nОрк{O}\n\nТролль{T}\n\nОтрекшийся{U}");
                        //PlayEngine.Menu.ChooseRace();
                        MenuEngine.EnterRace.Draw();
                    }
                    else
                    { Rogue.RAM.Player.Race = SystemEngine.Helper.Randomer.Race;  }
                    Rogue.RAM.Player.GetClassRacePerk(1);
                    #endregion

                    #region BattleClass
                    if (!Auto)
                    {
                        //DrawEngine.ConsoleDraw.WriteMenu("Выберите класс персонажа:\n\nМаг крови{B}\n\nПаладин{P}\n\nАлхимик{L}\n\nРазбойник{A}\n\nВоин{W}\n\nМаг огня{F}\n\nЭкзорцист{I}\n\nМонах{M}\n\nНекромант{N}\n\nШаман{S}");
                        //PlayEngine.Menu.ChooseClass();
                        MenuEngine.EnterClass.Draw();
                    }
                    else { Rogue.RAM.Player.Class = SystemEngine.Helper.Randomer.Class;  }
                    Rogue.RAM.Player.GetClassRacePerk(2);
                    #endregion

                    #region Stats

                    My.Level = 1;
                    My.mEXP = My.UpExp;
                    My.EXP = 0;
                    My.MHP = 100;
                    My.CHP = 100;
                    switch (Rogue.RAM.Player.Class)
                    {
                        case MechEngine.BattleClass.BloodMage:
                            {
                                My.CMP = 100; My.MMP = 100; My.MIDMG = 2;
                                My.MADMG = 4; break;
                            }
                        case MechEngine.BattleClass.Inquisitor:
                            {
                                My.CMP = 1; My.MMP = 1; My.MIDMG = 4;
                                My.MADMG = 6; break;
                            }
                        case MechEngine.BattleClass.Necromant:
                            {
                                My.CMP = 2; My.MMP = 3; My.MIDMG = 2;
                                My.MADMG = 5; break;
                            }
                        case MechEngine.BattleClass.Assassin:
                            {
                                My.CMP = 2; My.MMP = 2; My.MIDMG = 3;
                                My.MADMG = 7; break;
                            }
                        case MechEngine.BattleClass.Alchemist:
                            {
                                My.CMP = 100; My.MMP = 100; My.MIDMG = 2;
                                My.MADMG = 4; break;
                            }
                        case MechEngine.BattleClass.Warrior:
                            {
                                My.CMP = 0; My.MMP = 0; My.MIDMG = 4;
                                My.MADMG = 8; break;
                            }
                        default:
                            {
                                My.CMP = 100; My.MMP = 100; My.MIDMG = 3;
                                My.MADMG = 6; break;
                            }
                    }

                    My.MIDMG += 3;
                    My.MADMG += 3;
                    My.CHP += 10;
                    My.MHP += 10;

                    My.RecountPerks();

                    My.Gold = 100;

                    #endregion

                    #region Inventory

                    MechEngine.Equipment Eq = new MechEngine.Equipment();
                    My.Equipment = Eq;

                    //lists etc
                    List<MechEngine.Item> I = new List<MechEngine.Item>();
                    I.Add(DataBase.ItemBase.HealthPotion);
                    I.Add(DataBase.ItemBase.ManaPotion);
                    I.Add(DataBase.ItemBase.IronKey);
                    I.Add(DataBase.ItemBase.MagicKey);
                    I.Add(DataBase.ResourseBase.CatFood);
                    I.Add(DataBase.ItemBase.AmuletOfMraumir);

                    My.Inventory = new List<MechEngine.Item>(I);
                    #endregion

                    #region Title

                    //DrawEngine.ConsoleDraw.WriteTitle("Ваш персонаж:\n\nИмя: " + My.Name + "\nРаса:" + My.GetClassRace(1) + "\nКласс: " + My.GetClassRace(2) + "\nУровень: " + My.Level.ToString());
                    //                    Thread.Sleep(1000);
                    MenuEngine.InfoWindow.Draw();

                    #endregion

                    #region Skills

                    switch (Rogue.RAM.Player.Class)
                    {
                        case MechEngine.BattleClass.Alchemist: { Rogue.RAM.Player.Ability = DataBase.BattleAbilityBase.Alchemist(); break; }
                        case MechEngine.BattleClass.Assassin: { Rogue.RAM.Player.Ability = DataBase.BattleAbilityBase.Assassin(); break; }
                        case MechEngine.BattleClass.BloodMage: { Rogue.RAM.Player.Ability = DataBase.BattleAbilityBase.Vampire(); break; }
                        case MechEngine.BattleClass.FireMage: { Rogue.RAM.Player.Ability = DataBase.BattleAbilityBase.FireMage(); break; }
                        case MechEngine.BattleClass.Inquisitor: { Rogue.RAM.Player.Ability = DataBase.BattleAbilityBase.Inquisitor(); break; }
                        case MechEngine.BattleClass.Monk: { Rogue.RAM.Player.Ability = DataBase.BattleAbilityBase.Monk(); break; }
                        case MechEngine.BattleClass.Necromant: { Rogue.RAM.Player.Ability = DataBase.BattleAbilityBase.Necromant(); break; }
                        case MechEngine.BattleClass.Paladin: { Rogue.RAM.Player.Ability = DataBase.BattleAbilityBase.Paladin(); break; }
                        case MechEngine.BattleClass.Shaman: { Rogue.RAM.Player.Ability = DataBase.BattleAbilityBase.Shaman(); break; }
                        case MechEngine.BattleClass.Warrior: { Rogue.RAM.Player.Ability = DataBase.BattleAbilityBase.Warrior; break; }
                    }
                    Rogue.RAM.Player.Buffs = new List<MechEngine.Ability>();
                    Rogue.RAM.Player.MonkSpec = MechEngine.AbilityRate.AttackDamage;

                    #endregion

                    #region QuestBook

                    Rogue.RAM.Player.QuestBook = new List<MechEngine.Quest>();
                    Rogue.RAM.Player.QuestBook.Add(DataBase.QuestBase.__MAIN_QUEST);

                    #endregion

                    #region Another
                    Rogue.RAM.Player.Repute.Add(new MechEngine.Reputation() { biom = ConsoleColor.DarkGray, name = "Мертвые", min = 0, max = 10000, race = MechEngine.MonsterRace.Undead });
                    Rogue.RAM.Player.Repute.Add(new MechEngine.Reputation() { biom = ConsoleColor.Gray, name = "Дварфы", min = 0, max = 3000, race = MechEngine.MonsterRace.Human });
                    Rogue.RAM.Player.Repute.Add(new MechEngine.Reputation() { biom = ConsoleColor.DarkMagenta, name = "Рай", min = 0, max = 80000, race = MechEngine.MonsterRace.Drow });
                    #endregion

                    if (!Auto) { Rogue.RAM.History = true; }
                }
            }

            public static void Play()
            {                
                while (Rogue.RAM.Bone) { RealPlay(); }
            }

            public static void RealPlay()
            {
                #region Help
                if (!Rogue.RAM.WasHelp) { Rogue.RAM.WasHelp = true; DrawEngine.DialogueDraw.Helpers(); }
                if (Rogue.RAM.Map._Name == "Мраумир" && !Rogue.RAM.WasHelpAgain) { Rogue.RAM.WasHelpAgain = true; DataBase.NpcBase.VergiliyDialogue.Use(); }
                #endregion
                if (!EnemyMoves.CheckMove) { EnemyMoves.Move(true); }
                ConsoleKeyInfo qpush = Console.ReadKey(true);
                ConsoleKey push = qpush.Key;
                ConsoleModifiers mod = qpush.Modifiers;
                if (Rest && push != ConsoleKey.C)
                { DrawEngine.InfoWindow.Message = "Вы боитесь создать шум поэтому ничего не можете делать..."; }
                else if (Rest && push == ConsoleKey.C)
                {
                    EnemyMoves.Move(false);
                    DrawEngine.GUIDraw.ReDrawCharStat();
                    CharInfo();
                }
                if (!Rest)
                {
                    switch (push)
                    {
                        case ConsoleKey.UpArrow:
                            {
                                MoveCharacter(1);
                                Rogue.RAM.Step.Step();
                                break;
                            }
                        case ConsoleKey.DownArrow:
                            {
                                Rogue.RAM.Step.Step();
                                MoveCharacter(2);
                                break;
                            }
                        case ConsoleKey.LeftArrow:
                            {
                                Rogue.RAM.Step.Step();
                                MoveCharacter(3);
                                break;
                            }
                        case ConsoleKey.RightArrow:
                            {
                                Rogue.RAM.Step.Step();
                                MoveCharacter(4);
                                break;
                            }
                        case ConsoleKey.O:
                            {
                                Rogue.RAM.Step.Step();
                                OpenDoor();
                                break;
                            }
                        case ConsoleKey.T:
                            {
                                Rogue.RAM.Step.Step();
                                TakeItem();
                                break;
                            }
                        case ConsoleKey.A:
                            {
                                Rogue.RAM.Step.Step();
                                Attack();
                                break;
                            }
                        case ConsoleKey.D:
                            {
                                Rogue.RAM.Step.Step();
                                Destroy();
                                break;
                            }
                        case ConsoleKey.I:
                            {
                                Rogue.RAM.Step.Step();
                                GetInfo.GetInfoFromMap();
                                break;
                            }
                        case ConsoleKey.M:
                            {
                                if (Rogue.RAM.Map._Name != "Мраумир" && Rogue.RAM.Map._Name!="Святилище")
                                {
                                    EnemyMoves.Move(false);
                                    DrawEngine.WorldMapDraw.DrawWorldMap = true;
                                    Console.ReadKey(true);
                                }
                                RealPlay();
                                break;
                            }
                        case ConsoleKey.C:
                            {
                                EnemyMoves.Move(false);
                                //DrawEngine.GUIDraw.ReDrawCharStat();
                                Character();
                                break;
                            }
                        case ConsoleKey.D1:
                        case ConsoleKey.NumPad1:
                            {
                                Rogue.RAM.Step.Step();
                                PlayMenuItem(3);
                                break;
                            }
                        case ConsoleKey.D2:
                        case ConsoleKey.NumPad2:
                            {
                                Rogue.RAM.Step.Step();
                                PlayMenuItem(4);
                                break;
                            }
                        case ConsoleKey.D3:
                        case ConsoleKey.NumPad3:
                            {
                                Rogue.RAM.Step.Step();
                                PlayMenuItem(5);
                                break;
                            }
                        case ConsoleKey.D4:
                        case ConsoleKey.NumPad4:
                            {
                                Rogue.RAM.Step.Step();
                                PlayMenuItem(0);
                                break;
                            }
                        case ConsoleKey.D5:
                        case ConsoleKey.NumPad5:
                            {
                                Rogue.RAM.Step.Step();
                                PlayMenuItem(1);
                                break;
                            }
                        case ConsoleKey.D6:
                        case ConsoleKey.NumPad6:
                            {
                                Rogue.RAM.Step.Step();
                                PlayMenuItem(2);
                                break;
                            }
                        case ConsoleKey.Q: { Rogue.RAM.Step.Step(); if (mod == ConsoleModifiers.Shift) { Rogue.RAM.Player.CraftAbility[0].Activate(); } else { Rogue.RAM.Player.Ability[0].Activate(); } break; }
                        case ConsoleKey.W: { Rogue.RAM.Step.Step(); if (mod == ConsoleModifiers.Shift) { Rogue.RAM.Player.CraftAbility[1].Activate(); } else { Rogue.RAM.Player.Ability[1].Activate(); } break; }
                        case ConsoleKey.E: { Rogue.RAM.Step.Step(); if (mod == ConsoleModifiers.Shift) { Rogue.RAM.Player.CraftAbility[2].Activate(); } else { Rogue.RAM.Player.Ability[2].Activate(); } break; }
                        case ConsoleKey.R: { Rogue.RAM.Step.Step(); if (mod == ConsoleModifiers.Shift) { Rogue.RAM.Player.CraftAbility[3].Activate(); } else { Rogue.RAM.Player.Ability[3].Activate(); } break; }
                        case ConsoleKey.Escape: { bool wtf = false; Enemy = false; MenuEngine.EscapeMenu.Draw(); Enemy = true; break; }//while (!wtf) { wtf = Menu.InGameMenu; } Enemy = true; break; }
                    }
                }
            }

            private static bool Rest
            {
                get
                {
                    if (Rogue.RAM.Player.Buffs != null)
                    {
                        for (int i = 0; i < Rogue.RAM.Player.Buffs.Count; i++)
                        {
                            MechEngine.Ability At = Rogue.RAM.Player.Buffs[i];
                            { if (At.Name == "Спрятаться") { return true; } }
                        }
                    }
                    return false;
                }
            }

            public static void Fight(bool Attack = false,bool Education=false)
            {
                if (Education && Rogue.RAM.Enemy.CHP <= 0) { return; }
                EnemyMoves.Move(false);
                ConsoleKey push = Console.ReadKey(true).Key;
                switch (push)
                {
                    case ConsoleKey.Q:
                        {
                            Rogue.RAM.Step.Step();
                            Rogue.RAM.Player.Ability[0].Activate();
                            SoundEngine.Sound.MagicCast();
                            break;
                        }
                    case ConsoleKey.W:
                        {
                            Rogue.RAM.Step.Step();
                            Rogue.RAM.Player.Ability[1].Activate();
                            SoundEngine.Sound.MagicCast();
                            break;
                        }
                    case ConsoleKey.E:
                        {
                            Rogue.RAM.Step.Step();
                            Rogue.RAM.Player.Ability[2].Activate();
                            SoundEngine.Sound.MagicCast();
                            break;
                        }
                    case ConsoleKey.R:
                        {
                            Rogue.RAM.Step.Step();
                            Rogue.RAM.Player.Ability[3].Activate();
                            SoundEngine.Sound.MagicCast();
                            break;
                        }
                    case ConsoleKey.A:
                        {
                            Rogue.RAM.Step.Step();
                            Strike();
                            break;
                        }
                    case ConsoleKey.D:
                        {
                            Rogue.RAM.Step.Step();
                            Defense();
                            break;
                        }
                    case ConsoleKey.S:
                        {
                            Rogue.RAM.Step.Step();
                            Stealth();
                            break;
                        }
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        {
                            Rogue.RAM.Step.Step();
                            UseItem(3);
                            break;
                        }
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        {
                            Rogue.RAM.Step.Step();
                            UseItem(4);
                            break;
                        }
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        {
                            Rogue.RAM.Step.Step();
                            UseItem(5);
                            break;
                        }
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        {
                            Rogue.RAM.Step.Step();
                            UseItem(0);
                            break;
                        }
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        {
                            Rogue.RAM.Step.Step();
                            UseItem(1);
                            break;
                        }
                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        {
                            Rogue.RAM.Step.Step();
                            UseItem(2);
                            break;
                        }
                }
                if (Education) { Fight(true, true); } else { Fight(); }
            }

            //CHARACTER

            public static void Character()
            {
                privateindex = 0;
                DrawEngine.CharacterDraw.DrawInterface();
                string tabt = Rogue.RAM.iTab.Type;
                switch (tabt)
                {
                    case "Perk":
                        {
                            DrawEngine.CharacterDraw.DrawPerks();
                            DrawEngine.CharMap.DrawCMap(new List<string>() 
                                { 
                                    "[P] - Особенности ",
                                    "[S] - Навыки ", 
                                    "[I] - Экипировка ",
                                    "[Q] - Квесты ",
                                    "[G] - Эффекты ",
                                    "[1-6] - Инвентарь ",
                                    "[→] - Листать →",
                                    "[←] - Листать ←",
                                });
                            Perk();
                            break;
                        }
                    case "Item":
                        {
                            DrawEngine.CharacterDraw.DrawItems(0,privateindex);
                            DrawEngine.CharMap.DrawCMap(new List<string>() 
                                { 
                                    "[P] - Особенности ",
                                    "[S] - Навыки ", 
                                    "[I] - Экипировка ",
                                    "[Q] - Квесты ",
                                    "[G] - Эффекты ",
                                    "[1-6] - Инвентарь ",
                                    "[H],[M],[A],[W],[O],[B]",
                                    "↑+Enter - Снять вещь",
                                });
                            Item();
                            break;
                        }
                    case "Skill":
                        {
                            DrawEngine.CharacterDraw.DrawAbility(true);
                            DrawEngine.CharMap.DrawCMap(new List<string>() 
                                { 
                                    "[P] - Особенности ",
                                    "[S] - Навыки ", 
                                    "[I] - Экипировка ",
                                    "[Q] - Квесты ",
                                    "[G] - Эффекты ",
                                    "[1-6] - Инвентарь ",
                                    "[Enter] - Улучшить",
                                    "[N] - Информация",
                                });
                            Abil();
                            break;
                        }
                    case "Quest":
                        {
                            DrawEngine.CharacterDraw.DrawQuests();
                            DrawEngine.CharMap.DrawCMap(new List<string>() 
                                { 
                                    "[P] - Особенности ",
                                    "[S] - Навыки ", 
                                    "[I] - Экипировка ",
                                    "[Q] - Квесты ",
                                    "[G] - Эффекты ",
                                    "[1-6] - Инвентарь ",
                                    "[→] - Листать →",
                                    "[←] - Листать ←",
                                });
                            QuestB();
                            break;
                        }
                    case "Buffs":
                        {
                            DrawEngine.CharacterDraw.DrawBuffs();
                            DrawEngine.CharMap.DrawCMap(new List<string>() 
                                { 
                                    "[P] - Особенности ",
                                    "[S] - Навыки ", 
                                    "[I] - Экипировка ",
                                    "[Q] - Квесты ",
                                    "[G] - Эффекты ",
                                    "[1-6] - Инвентарь ",
                                    "[→] - Листать →",
                                    "[←] - Листать ←",
                                });
                            BuffsB();
                            break;
                        }
                }
            }
            /// <summary>
            /// GUI Perk window
            /// </summary>
            private static void Perk()
            {
                int tab = Rogue.RAM.iTab.NowTab;
                ConsoleKey push = Console.ReadKey(true).Key;
                switch (push)
                {
                    case ConsoleKey.RightArrow:
                        {
                            if (Rogue.RAM.iTab.NowTab < Rogue.RAM.iTab.MaxTab)
                            {
                                DrawEngine.CharacterDraw.DrawPerks(tab + 1);
                            }
                            Perk();
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            if (Rogue.RAM.iTab.NowTab >= Rogue.RAM.iTab.MaxTab && Rogue.RAM.iTab.NowTab != 1)
                            {
                                DrawEngine.CharacterDraw.DrawPerks(tab - 1);
                            }
                            Perk();
                            break;
                        }
                    case ConsoleKey.S:
                        {
                            Rogue.RAM.iTab.Type = "Skill";
                            Rogue.RAM.iTab.NowTab = 1;
                            Character();
                            break;
                        }
                    case ConsoleKey.G:
                        {
                            Rogue.RAM.iTab.Type = "Buffs";
                            Rogue.RAM.iTab.NowTab = 1;
                            Character();
                            break;
                        }
                    case ConsoleKey.I:
                        {
                            Rogue.RAM.iTab.Type = "Item";
                            Rogue.RAM.iTab.NowTab = 1;
                            Character();
                            break;
                        }
                    case ConsoleKey.Q:
                        {
                            Rogue.RAM.iTab.Type = "Quest";
                            Rogue.RAM.iTab.NowTab = 1;
                            Character();
                            break;
                        }
                    case ConsoleKey.Escape:
                        {                            
                            break;
                        }
                    default:
                        {
                            Perk();
                            break;
                        }
                }
            }
            private static void BuffsB()
            {
                int tab = Rogue.RAM.iTab.NowTab;
                ConsoleKey push = Console.ReadKey(true).Key;
                switch (push)
                {
                    case ConsoleKey.RightArrow:
                        {
                            if (Rogue.RAM.iTab.NowTab < Rogue.RAM.iTab.MaxTab)
                            {
                                DrawEngine.CharacterDraw.DrawBuffs(tab + 1);
                            }
                            BuffsB();
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            if (Rogue.RAM.iTab.NowTab >= Rogue.RAM.iTab.MaxTab && Rogue.RAM.iTab.NowTab != 1)
                            {
                                DrawEngine.CharacterDraw.DrawBuffs(tab - 1);
                            }
                            BuffsB();
                            break;
                        }
                    case ConsoleKey.S:
                        {
                            Rogue.RAM.iTab.Type = "Skill";
                            Rogue.RAM.iTab.NowTab = 1;
                            Character();
                            break;
                        }
                    case ConsoleKey.I:
                        {
                            Rogue.RAM.iTab.Type = "Item";
                            Rogue.RAM.iTab.NowTab = 1;
                            Character();
                            break;
                        }
                    case ConsoleKey.Q:
                        {
                            Rogue.RAM.iTab.Type = "Quest";
                            Rogue.RAM.iTab.NowTab = 1;
                            Character();
                            break;
                        }
                    case ConsoleKey.P:
                        {
                            Rogue.RAM.iTab.Type = "Perk";
                            Rogue.RAM.iTab.NowTab = 1;
                            Character();
                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            break;
                        }
                    default:
                        {
                            BuffsB();
                            break;
                        }
                }
            }
            /// <summary>
            /// GUI Quest window
            /// </summary>
            private static void QuestB()
            {
                int tab = Rogue.RAM.Qtab.NowTab;
                ConsoleKey push = Console.ReadKey(true).Key;
                switch (push)
                {
                    case ConsoleKey.RightArrow:
                        {
                            if (Rogue.RAM.Qtab.NowTab < Rogue.RAM.Qtab.MaxTab)
                            {
                                DrawEngine.CharacterDraw.DrawQuests(tab + 1);
                            }
                            QuestB();
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            if (Rogue.RAM.Qtab.NowTab >= Rogue.RAM.Qtab.MaxTab && Rogue.RAM.Qtab.NowTab != 1)
                            {
                                DrawEngine.CharacterDraw.DrawQuests(tab - 1);
                            }
                            QuestB();
                            break;
                        }
                    case ConsoleKey.S:
                        {
                            Rogue.RAM.iTab.Type = "Skill";
                            Rogue.RAM.iTab.NowTab = 1;
                            Character();
                            break;
                        }
                    case ConsoleKey.G:
                        {
                            Rogue.RAM.iTab.Type = "Buffs";
                            Rogue.RAM.iTab.NowTab = 1;
                            Character();
                            break;
                        }
                    case ConsoleKey.I:
                        {
                            Rogue.RAM.iTab.Type = "Item";
                            Rogue.RAM.iTab.NowTab = 1;
                            Character();
                            break;
                        }
                    case ConsoleKey.P:
                        {
                            Rogue.RAM.iTab.Type = "Perk";
                            Rogue.RAM.iTab.NowTab = 1;
                            Character();
                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            break;
                        }
                    default:
                        {
                            QuestB();
                            break;
                        }
                }
            }
            private static int privateindex;
            /// <summary>
            /// GUI Item window
            /// </summary>
            private static void Item()
            {
                ConsoleKey push = Console.ReadKey(true).Key;
                switch (push)
                {
                    case ConsoleKey.UpArrow: { if (privateindex != 0) { privateindex--; } DrawEngine.CharacterDraw.DrawItems(0, privateindex); Item(); break; }
                    case ConsoleKey.DownArrow: { if (privateindex != 4) { privateindex++; } DrawEngine.CharacterDraw.DrawItems(0, privateindex); Item(); break; }
                    case ConsoleKey.Enter:
                        {
                            switch (privateindex)
                            {
                                case 0: { TryUndress(Rogue.RAM.Player.Equipment.Weapon); break; }
                                case 1: { TryUndress(Rogue.RAM.Player.Equipment.OffHand); break; }
                                case 2: { TryUndress(Rogue.RAM.Player.Equipment.Helm); break; }
                                case 3: { TryUndress(Rogue.RAM.Player.Equipment.Armor); break; }
                                case 4: { TryUndress(Rogue.RAM.Player.Equipment.Boots); break; }
                            }
                            Item();
                            break;
                        }
                    case ConsoleKey.P:
                        {
                            Rogue.RAM.iTab.Type = "Perk";
                            Rogue.RAM.iTab.NowTab = 1;
                            Character();
                            break;
                        }
                    case ConsoleKey.G:
                        {
                            Rogue.RAM.iTab.Type = "Buffs";
                            Rogue.RAM.iTab.NowTab = 1;
                            Character();
                            break;
                        }
                    case ConsoleKey.S:
                        {
                            Rogue.RAM.iTab.Type = "Skill";
                            Rogue.RAM.iTab.NowTab = 1;
                            Character();
                            break;
                        }
                    case ConsoleKey.Q:
                        {
                            Rogue.RAM.iTab.Type = "Quest";
                            Rogue.RAM.iTab.NowTab = 1;
                            Character();
                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            break;
                        }
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        {
                            try
                            {
                                useEquip(Rogue.RAM.Player.Inventory[3], 3);
                                DrawEngine.CharacterDraw.DrawItems(0,privateindex);
                                DrawEngine.CharacterDraw.DrawItemsWindow();
                                DrawEngine.GUIDraw.ReDrawCharStat();
                            }
                            catch { }
                            Item();
                            break;
                        }
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        {
                            try
                            {
                                useEquip(Rogue.RAM.Player.Inventory[4], 4);
                                DrawEngine.CharacterDraw.DrawItems(0, privateindex);
                                DrawEngine.CharacterDraw.DrawItemsWindow();
                                DrawEngine.GUIDraw.ReDrawCharStat();
                            }
                            catch { }
                            Item();
                            break;
                        }
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        {
                            try
                            {
                                useEquip(Rogue.RAM.Player.Inventory[5], 5);
                                DrawEngine.CharacterDraw.DrawItems(0, privateindex);
                                DrawEngine.CharacterDraw.DrawItemsWindow();
                                DrawEngine.GUIDraw.ReDrawCharStat();
                            }
                            catch { }
                            Item();
                            break;
                        }
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        {
                            useEquip(Rogue.RAM.Player.Inventory[0], 0);
                            DrawEngine.CharacterDraw.DrawItems(0, privateindex);
                            DrawEngine.CharacterDraw.DrawItemsWindow();
                            DrawEngine.GUIDraw.ReDrawCharStat();
                            Item();
                            break;
                        }
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        {
                            try
                            {
                                useEquip(Rogue.RAM.Player.Inventory[1], 1);
                                DrawEngine.CharacterDraw.DrawItems(0, privateindex);
                                DrawEngine.CharacterDraw.DrawItemsWindow();
                                DrawEngine.GUIDraw.ReDrawCharStat();
                            }
                            catch { }
                            Item();
                            break;
                        }
                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        {
                            try
                            {
                                useEquip(Rogue.RAM.Player.Inventory[2], 2);
                                DrawEngine.CharacterDraw.DrawItems(0, privateindex);
                                DrawEngine.CharacterDraw.DrawItemsWindow();
                                DrawEngine.GUIDraw.ReDrawCharStat();
                            }
                            catch { }
                            Item();
                            break;
                        }
                    default:
                        {
                            Item();
                            break;
                        }
                }
            }
            /// <summary>
            /// Undress item
            /// </summary>
            private static void TryUndress(MechEngine.Item i)
            {
                if (i != null)
                {
                    if (Rogue.RAM.Player.InventorySlots)
                    {
                        //MechEngine.Item temp = new MechEngine.Item();
                        switch (i.Kind)
                        {
                            case MechEngine.Kind.Armor:
                                {
                                    Rogue.RAM.Player.Equipment.Armor.UnDress();
                                    Rogue.RAM.Player.Inventory.Add(Rogue.RAM.Player.Equipment.Armor);
                                    Rogue.RAM.Player.Equipment.Armor = null;
                                    DrawEngine.CharacterDraw.DrawItems(0,privateindex);
                                    DrawEngine.CharacterDraw.DrawItemsWindow();
                                    DrawEngine.GUIDraw.ReDrawCharStat();
                                    break;
                                }
                            case MechEngine.Kind.Boots:
                                {
                                    Rogue.RAM.Player.Equipment.Boots.UnDress();
                                    Rogue.RAM.Player.Inventory.Add(Rogue.RAM.Player.Equipment.Boots);
                                    Rogue.RAM.Player.Equipment.Boots = null;
                                    DrawEngine.CharacterDraw.DrawItems(0, privateindex);
                                    DrawEngine.CharacterDraw.DrawItemsWindow();
                                    DrawEngine.GUIDraw.ReDrawCharStat();
                                    break;
                                }
                            case MechEngine.Kind.Helm:
                                {
                                    Rogue.RAM.Player.Equipment.Helm.UnDress();
                                    Rogue.RAM.Player.Inventory.Add(Rogue.RAM.Player.Equipment.Helm);
                                    Rogue.RAM.Player.Equipment.Helm = null;
                                    DrawEngine.CharacterDraw.DrawItems(0, privateindex);
                                    DrawEngine.CharacterDraw.DrawItemsWindow();
                                    DrawEngine.GUIDraw.ReDrawCharStat();
                                    break;
                                }
                            case MechEngine.Kind.OffHand:
                                {
                                    Rogue.RAM.Player.Equipment.OffHand.UnDress();
                                    Rogue.RAM.Player.Inventory.Add(Rogue.RAM.Player.Equipment.OffHand);
                                    Rogue.RAM.Player.Equipment.OffHand = null;
                                    DrawEngine.CharacterDraw.DrawItems(0, privateindex);
                                    DrawEngine.CharacterDraw.DrawItemsWindow();
                                    DrawEngine.GUIDraw.ReDrawCharStat();
                                    break;
                                }
                            case MechEngine.Kind.Weapon:
                                {
                                    Rogue.RAM.Player.Equipment.Weapon.UnDress();
                                    Rogue.RAM.Player.Inventory.Add(Rogue.RAM.Player.Equipment.Weapon);
                                    Rogue.RAM.Player.Equipment.Weapon = null;
                                    DrawEngine.CharacterDraw.DrawItems(0, privateindex);
                                    DrawEngine.CharacterDraw.DrawItemsWindow();
                                    DrawEngine.GUIDraw.ReDrawCharStat();
                                    break;
                                }
                        }
                        DrawEngine.InfoWindow.Message = "Вы сняли '" + i.Name + "' !";
                    }
                    else
                    { DrawEngine.InfoWindow.Message = "Инвентарь переполнен!"; }
                }
                else { DrawEngine.InfoWindow.Warning = "Нельзя снять пустоту!"; }
            }
            /// <summary>
            /// For identify char dress slot and inventory slot
            /// </summary>
            /// <param name="Slot">Kind of Slot</param>
            private static void ItemWear(MechEngine.Kind Slot)
            {
                ConsoleKey push = Console.ReadKey(true).Key;
                switch (push)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        {
                            Dress(0, Slot);
                            break;
                        }
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        {
                            Dress(1, Slot);
                            break;
                        }
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        {
                            Dress(2, Slot);
                            break;
                        }
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        {
                            Dress(3, Slot);
                            break;
                        }
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        {
                            Dress(4, Slot);
                            break;
                        }
                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        {
                            Dress(5, Slot);
                            break;
                        }
                    default:
                        {
                            DrawEngine.InfoWindow.Custom("Отмена.");
                            Character();
                            break;
                        }
                }
            }
            /// <summary>
            /// For dress some item in character items slot
            /// </summary>
            /// <param name="Index">Index of inventory slot</param>
            /// <param name="Slot">Kind of character inventory slot</param>
            private static void Dress(int Index, MechEngine.Kind Slot)
            {
                try
                {
                    if (Rogue.RAM.Player.Inventory[Index].Kind == Slot)
                    {
                        #region Dress

                        switch (Slot)
                        {
                            case MechEngine.Kind.Helm:
                                {
                                    MechEngine.Item.Helm temp = Rogue.RAM.Player.Inventory[Index] as MechEngine.Item.Helm;
                                    if (Rogue.RAM.Player.Equipment.Helm != null)
                                    {
                                        Rogue.RAM.Player.Inventory[Index] = Rogue.RAM.Player.Equipment.Helm;
                                        Rogue.RAM.Player.Equipment.Helm.UnDress();
                                    }
                                    else
                                    {
                                        Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[Index]);
                                    }
                                    Rogue.RAM.Player.Equipment.Helm = temp;
                                    Rogue.RAM.Player.Equipment.Helm.Dress();
                                    DrawEngine.InfoWindow.Custom("Вы экипировали: " + Rogue.RAM.Player.Equipment.Helm.Name);
                                    break;
                                }
                            case MechEngine.Kind.Armor:
                                {
                                    MechEngine.Item.Armor temp = Rogue.RAM.Player.Inventory[Index] as MechEngine.Item.Armor;
                                    if (Rogue.RAM.Player.Equipment.Armor != null)
                                    {
                                        Rogue.RAM.Player.Inventory[Index] = Rogue.RAM.Player.Equipment.Armor;
                                        Rogue.RAM.Player.Equipment.Armor.UnDress();
                                    }
                                    else
                                    {
                                        Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[Index]);
                                    }
                                    Rogue.RAM.Player.Equipment.Armor = temp;
                                    Rogue.RAM.Player.Equipment.Armor.Dress();
                                    DrawEngine.InfoWindow.Custom("Вы экипировали: " + Rogue.RAM.Player.Equipment.Armor.Name);
                                    break;
                                }
                            case MechEngine.Kind.Boots:
                                {
                                    MechEngine.Item.Boots temp = Rogue.RAM.Player.Inventory[Index] as MechEngine.Item.Boots;
                                    if (Rogue.RAM.Player.Equipment.Boots != null)
                                    {
                                        Rogue.RAM.Player.Inventory[Index] = Rogue.RAM.Player.Equipment.Boots;
                                        Rogue.RAM.Player.Equipment.Boots.UnDress();
                                    }
                                    else
                                    {
                                        Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[Index]);
                                    }
                                    Rogue.RAM.Player.Equipment.Boots = temp;
                                    Rogue.RAM.Player.Equipment.Boots.Dress();
                                    DrawEngine.InfoWindow.Custom("Вы экипировали: " + Rogue.RAM.Player.Equipment.Boots.Name);
                                    break;
                                }
                            case MechEngine.Kind.OffHand:
                                {
                                    MechEngine.Item.OffHand temp = Rogue.RAM.Player.Inventory[Index] as MechEngine.Item.OffHand;
                                    if (Rogue.RAM.Player.Equipment.OffHand != null)
                                    {
                                        Rogue.RAM.Player.Inventory[Index] = Rogue.RAM.Player.Equipment.OffHand;
                                        Rogue.RAM.Player.Equipment.OffHand.UnDress();
                                    }
                                    else
                                    {
                                        Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[Index]);
                                    }
                                    Rogue.RAM.Player.Equipment.OffHand = temp;
                                    Rogue.RAM.Player.Equipment.OffHand.Dress();
                                    DrawEngine.InfoWindow.Custom("Вы экипировали: " + Rogue.RAM.Player.Equipment.OffHand.Name);
                                    break;
                                }
                            case MechEngine.Kind.Weapon:
                                {
                                    MechEngine.Item.Weapon temp = Rogue.RAM.Player.Inventory[Index] as MechEngine.Item.Weapon;
                                    if (Rogue.RAM.Player.Equipment.Weapon != null)
                                    {
                                        Rogue.RAM.Player.Inventory[Index] = Rogue.RAM.Player.Equipment.Weapon;
                                        Rogue.RAM.Player.Equipment.Weapon.UnDress();
                                    }
                                    else
                                    {
                                        Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[Index]);
                                    }
                                    Rogue.RAM.Player.Equipment.Weapon = temp;
                                    Rogue.RAM.Player.Equipment.Weapon.Dress();
                                    DrawEngine.InfoWindow.Custom("Вы экипировали: " + Rogue.RAM.Player.Equipment.Weapon.Name);
                                    break;
                                }
                        }

                        #endregion

                        DrawEngine.GUIDraw.ReDrawCharStat();
                        DrawEngine.CharacterDraw.DrawItems();
                        DrawEngine.CharacterDraw.DrawItemsWindow();
                    }
                    else
                    {
                        DrawEngine.InfoWindow.Custom(Rogue.RAM.Player.Inventory[Index].Name + " не подходит для этого слота!");                        
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    DrawEngine.InfoWindow.Custom("Эта ячейка инвентаря пустая!");
                }
                Item();
            }
            /// <summary>
            /// GUI Skill window
            /// </summary>
            private static void Abil()
            {
                int tab = Rogue.RAM.iTab.NowTab;
                ConsoleKey push = Console.ReadKey(true).Key;
                switch (push)
                {
                    case ConsoleKey.RightArrow:
                        {
                            DrawEngine.CharacterDraw.DrawAbilitySelect(SystemEngine.ArrowDirection.Right);
                            Abil();
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            DrawEngine.CharacterDraw.DrawAbilitySelect(SystemEngine.ArrowDirection.Left);
                            Abil();
                            break;
                        }
                    case ConsoleKey.UpArrow:
                        {
                            DrawEngine.CharacterDraw.DrawAbilitySelect(SystemEngine.ArrowDirection.Top);
                            Abil();
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            DrawEngine.CharacterDraw.DrawAbilitySelect(SystemEngine.ArrowDirection.Bot);
                            Abil();
                            break;
                        }
                    case ConsoleKey.Enter:
                        {
                            UpSkill();
                            Abil();
                            break;
                        }
                    case ConsoleKey.N:
                        {
                            InfoSkill();
                            Abil();
                            break;
                        }
                    case ConsoleKey.P:
                        {
                            Rogue.RAM.iTab.Type = "Perk";
                            Rogue.RAM.iTab.NowTab = 1;
                            Character();
                            break;
                        }
                    case ConsoleKey.I:
                        {
                            Rogue.RAM.iTab.Type = "Item";
                            Rogue.RAM.iTab.NowTab = 1;
                            Character();
                            break;
                        }
                    case ConsoleKey.Q:
                        {
                            Rogue.RAM.iTab.Type = "Quest";
                            Rogue.RAM.iTab.NowTab = 1;
                            Character();
                            break;
                        }
                    case ConsoleKey.G:
                        {
                            Rogue.RAM.iTab.Type = "Buffs";
                            Rogue.RAM.iTab.NowTab = 1;
                            Character();
                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            break;
                        }
                    default:
                        {
                            Abil();
                            break;
                        }
                }
            }
            /// <summary>
            /// Up current chosen ability, if character have skill points
            /// </summary>
            private static void UpSkill()
            {
                if (
                    Rogue.RAM.iTab.NowTab != 5 &&
                    Rogue.RAM.iTab.NowTab != 6 &&
                    Rogue.RAM.iTab.NowTab != 7 &&
                    Rogue.RAM.iTab.NowTab != 8
                    )
                {
                    if (Rogue.RAM.Player.AbPoint <= 0) { DrawEngine.InfoWindow.Custom("У вас нет свободных очков навыков!"); }
                    else
                    {
                        switch (Rogue.RAM.iTab.NowTab)
                        {
                            case 1: { Rogue.RAM.Player.Ability[0].UP(); DrawEngine.InfoWindow.Custom("Вы увеличиваете " + Rogue.RAM.Player.Ability[0].Name + " на еденицу!"); break; }
                            case 2: { Rogue.RAM.Player.Ability[1].UP(); DrawEngine.InfoWindow.Custom("Вы увеличиваете " + Rogue.RAM.Player.Ability[1].Name + " на еденицу!"); break; }
                            case 3: { Rogue.RAM.Player.Ability[2].UP(); DrawEngine.InfoWindow.Custom("Вы увеличиваете " + Rogue.RAM.Player.Ability[2].Name + " на еденицу!"); break; }
                            case 4: { Rogue.RAM.Player.Ability[3].UP(); DrawEngine.InfoWindow.Custom("Вы увеличиваете " + Rogue.RAM.Player.Ability[3].Name + " на еденицу!"); break; }
                        }
                        Rogue.RAM.Player.AbPoint -= 1;
                        DrawEngine.CharacterDraw.DrawAbility(true);
                        DrawEngine.CharacterDraw.DrawItemsWindow();
                    }
                }
                else
                {
                    if (Rogue.RAM.Player.CrPoint <= 0) { DrawEngine.InfoWindow.Custom("У вас нет свободных очков способностей!"); }
                    else
                    {
                        switch (Rogue.RAM.iTab.NowTab)
                        {
                            case 5: { Rogue.RAM.Player.CraftAbility[0].UP(); DrawEngine.InfoWindow.Custom("Вы увеличиваете " + Rogue.RAM.Player.CraftAbility[0].Name + " на еденицу!"); break; }
                            case 6: { Rogue.RAM.Player.CraftAbility[1].UP(); DrawEngine.InfoWindow.Custom("Вы увеличиваете " + Rogue.RAM.Player.CraftAbility[1].Name + " на еденицу!"); break; }
                            case 7: { Rogue.RAM.Player.CraftAbility[2].UP(); DrawEngine.InfoWindow.Custom("Вы увеличиваете " + Rogue.RAM.Player.CraftAbility[2].Name + " на еденицу!"); break; }
                            case 8: { Rogue.RAM.Player.CraftAbility[3].UP(); DrawEngine.InfoWindow.Custom("Вы увеличиваете " + Rogue.RAM.Player.CraftAbility[3].Name + " на еденицу!"); break; }
                        }
                        Rogue.RAM.Player.CrPoint -= 1;
                        DrawEngine.CharacterDraw.DrawAbility(true);
                        DrawEngine.CharacterDraw.DrawItemsWindow();
                    }
                }
            }
            /// <summary>
            /// See info about skill
            /// </summary>
            private static void InfoSkill()
            {
                string info = string.Empty;
                switch (Rogue.RAM.iTab.NowTab)
                {
                    case 1: { Rogue.RAM.Player.Ability[0].DrawInfo(); break; }
                    case 2: { Rogue.RAM.Player.Ability[1].DrawInfo(); break; }
                    case 3: { Rogue.RAM.Player.Ability[2].DrawInfo(); break; }
                    case 4: { Rogue.RAM.Player.Ability[3].DrawInfo(); break; }
                    case 5: { Rogue.RAM.Player.CraftAbility[0].DrawInfo(); break; }
                    case 6: { Rogue.RAM.Player.CraftAbility[1].DrawInfo(); break; }
                    case 7: { Rogue.RAM.Player.CraftAbility[2].DrawInfo(); break; }
                    case 8: { Rogue.RAM.Player.CraftAbility[3].DrawInfo(); break; }
                }
                DrawEngine.CharacterDraw.DrawAbility(true);
            }
            /// <summary>
            /// GUI Merchant window
            /// </summary>
            public static void Merch()
            {
                DrawEngine.CharMap.DrawCMap(new List<string>() 
                                { 
                                    "[↑] - Навигация ",
                                    "[↓] - Навигация ", 
                                    "[→] - Навигация ",
                                    "[←] - Навиация ",
                                    "[I] - Инфо о вещи",
                                    "[1-6] - Продать ",
                                    "[Enter] - Купить",
                                    "[Escape] - Отмена",
                                });
                ConsoleKey push = Console.ReadKey(true).Key;
                switch (push)
                {
                    case ConsoleKey.RightArrow:
                        {
                            DrawEngine.MerchantDraw.DrawAbilitySelect(SystemEngine.ArrowDirection.Right);
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            DrawEngine.MerchantDraw.DrawAbilitySelect(SystemEngine.ArrowDirection.Left);
                            break;
                        }
                    case ConsoleKey.UpArrow:
                        {
                            DrawEngine.MerchantDraw.DrawAbilitySelect(SystemEngine.ArrowDirection.Top);
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            DrawEngine.MerchantDraw.DrawAbilitySelect(SystemEngine.ArrowDirection.Bot);
                            break;
                        }
                    case ConsoleKey.I:
                        {
                            try { GetInfo.Item = (Rogue.RAM.Merch as MechEngine.Merchant).Goods[Rogue.RAM.MerchTab.NowTab - 1]; }
                            catch { }
                            break;
                        }
                    case ConsoleKey.Enter:
                        {
                            if (Rogue.RAM.Player.InventorySlots)
                            {
                                try
                                {
                                    if ((Rogue.RAM.Player.Gold - (Rogue.RAM.Merch as MechEngine.Merchant).Goods[Rogue.RAM.MerchTab.NowTab - 1].Sell) >= 0)
                                    {
                                        if (Rogue.RAM.Merch.GetType() == typeof(MechEngine.Member))
                                        {
                                            if ((Rogue.RAM.Merch as MechEngine.Merchant).Goods[Rogue.RAM.MerchTab.NowTab - 1].CheckReputation)
                                            {
                                                Rogue.RAM.Player.Gold -= (Rogue.RAM.Merch as MechEngine.Merchant).Goods[Rogue.RAM.MerchTab.NowTab - 1].Sell;
                                                (Rogue.RAM.Merch as MechEngine.Merchant).CurGold += (Rogue.RAM.Merch as MechEngine.Merchant).Goods[Rogue.RAM.MerchTab.NowTab - 1].Sell;

                                                Rogue.RAM.Player.Inventory.Add((Rogue.RAM.Merch as MechEngine.Merchant).Goods[Rogue.RAM.MerchTab.NowTab - 1]);
                                                Rogue.RAM.Player.QuestItem = (Rogue.RAM.Merch as MechEngine.Merchant).Goods[Rogue.RAM.MerchTab.NowTab - 1].Name;
                                                DrawEngine.InfoWindow.Buy((Rogue.RAM.Merch as MechEngine.Merchant).Goods[Rogue.RAM.MerchTab.NowTab - 1].Sell, (Rogue.RAM.Merch as MechEngine.Merchant).Goods[Rogue.RAM.MerchTab.NowTab - 1].Name);
                                                DrawEngine.GUIDraw.ReDrawCharStat();

                                                (Rogue.RAM.Merch as MechEngine.Merchant).Goods.Remove((Rogue.RAM.Merch as MechEngine.Merchant).Goods[Rogue.RAM.MerchTab.NowTab - 1]);
                                                DrawEngine.MerchantDraw.ReDrawMoney();
                                                DrawEngine.MerchantDraw.DrawGoodsWindow(true);
                                            }
                                            else
                                            { DrawEngine.InfoWindow.Message = "У вас нехватает репутации для покупки этого предмета!"; }
                                        }
                                        else
                                        {
                                            Rogue.RAM.Player.Gold -= (Rogue.RAM.Merch as MechEngine.Merchant).Goods[Rogue.RAM.MerchTab.NowTab - 1].Sell;
                                            (Rogue.RAM.Merch as MechEngine.Merchant).CurGold += (Rogue.RAM.Merch as MechEngine.Merchant).Goods[Rogue.RAM.MerchTab.NowTab - 1].Sell;

                                            Rogue.RAM.Player.Inventory.Add((Rogue.RAM.Merch as MechEngine.Merchant).Goods[Rogue.RAM.MerchTab.NowTab - 1]);
                                            Rogue.RAM.Player.QuestItem = (Rogue.RAM.Merch as MechEngine.Merchant).Goods[Rogue.RAM.MerchTab.NowTab - 1].Name;
                                            DrawEngine.InfoWindow.Buy((Rogue.RAM.Merch as MechEngine.Merchant).Goods[Rogue.RAM.MerchTab.NowTab - 1].Sell, (Rogue.RAM.Merch as MechEngine.Merchant).Goods[Rogue.RAM.MerchTab.NowTab - 1].Name);
                                            DrawEngine.GUIDraw.ReDrawCharStat();

                                            (Rogue.RAM.Merch as MechEngine.Merchant).Goods.Remove((Rogue.RAM.Merch as MechEngine.Merchant).Goods[Rogue.RAM.MerchTab.NowTab - 1]);
                                            DrawEngine.MerchantDraw.ReDrawMoney();
                                            DrawEngine.MerchantDraw.DrawGoodsWindow(true);
                                        }
                                    }
                                    else { DrawEngine.InfoWindow.Buy(0, "null"); }
                                }
                                catch { }
                            }
                            else { DrawEngine.InfoWindow.Loot(false, null); }
                            break;
                        }
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        {
                            try
                            {
                                if (!(Rogue.RAM.Merch.GetType() == typeof(MechEngine.Member)))
                                {
                                    if (Rogue.RAM.Player.Inventory[3] != null)
                                    {
                                        DrawEngine.InfoWindow.Warning = "Вы хотите продать " + Rogue.RAM.Player.Inventory[3].Name + " за " + Rogue.RAM.Player.Inventory[3].Sell.ToString() + " золотых?  Y/N?";
                                        ConsoleKey sellkey = Console.ReadKey(true).Key;
                                        if (sellkey == ConsoleKey.Y)
                                        {
                                            if (((Rogue.RAM.Merch as MechEngine.Merchant).CurGold + Rogue.RAM.Player.Inventory[3].Sell) <= (Rogue.RAM.Merch as MechEngine.Merchant).MaxGold)
                                            {
                                                Rogue.RAM.Player.Gold += Rogue.RAM.Player.Inventory[3].Sell;
                                                (Rogue.RAM.Merch as MechEngine.Merchant).CurGold -= Rogue.RAM.Player.Inventory[3].Sell;
                                                DrawEngine.InfoWindow.Sell(Rogue.RAM.Player.Inventory[3].Sell, Rogue.RAM.Player.Inventory[3].Name);
                                                if ((Rogue.RAM.Merch as MechEngine.Merchant).Goods.Count < 12)
                                                {
                                                    (Rogue.RAM.Merch as MechEngine.Merchant).Goods.Add(Rogue.RAM.Player.Inventory[3]);
                                                }
                                                DrawEngine.MerchantDraw.ReDrawMoney();
                                                DrawEngine.MerchantDraw.DrawGoodsWindow(true);
                                                Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[3]);
                                                DrawEngine.GUIDraw.ReDrawCharStat();
                                            }
                                            else
                                            { DrawEngine.InfoWindow.Message = "Торговец не может нести столько золота!"; }
                                        }
                                        else
                                        { DrawEngine.InfoWindow.Warning = "Продажа предмета отменена!"; }
                                    }
                                    else { DrawEngine.InfoWindow.Custom("В этой ячейке ничего нет на продажу!"); }
                                }
                            }
                            catch { DrawEngine.InfoWindow.Custom("В этой ячейке ничего нет на продажу!"); }
                            break;
                        }
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        {
                            try
                            {
                                if (!(Rogue.RAM.Merch.GetType() == typeof(MechEngine.Member)))
                                {
                                    if (Rogue.RAM.Player.Inventory[4] != null)
                                    {
                                        DrawEngine.InfoWindow.Warning = "Вы хотите продать " + Rogue.RAM.Player.Inventory[4].Name + " за " + Rogue.RAM.Player.Inventory[4].Sell.ToString() + " золотых?  Y/N?";
                                        ConsoleKey sellkey = Console.ReadKey(true).Key;
                                        if (sellkey == ConsoleKey.Y)
                                        {
                                            if (((Rogue.RAM.Merch as MechEngine.Merchant).CurGold + Rogue.RAM.Player.Inventory[4].Sell) <= (Rogue.RAM.Merch as MechEngine.Merchant).MaxGold)
                                            {
                                                Rogue.RAM.Player.Gold += Rogue.RAM.Player.Inventory[4].Sell;
                                                (Rogue.RAM.Merch as MechEngine.Merchant).CurGold -= Rogue.RAM.Player.Inventory[4].Sell;
                                                DrawEngine.InfoWindow.Sell(Rogue.RAM.Player.Inventory[4].Sell, Rogue.RAM.Player.Inventory[4].Name);
                                                if ((Rogue.RAM.Merch as MechEngine.Merchant).Goods.Count < 12)
                                                {
                                                    (Rogue.RAM.Merch as MechEngine.Merchant).Goods.Add(Rogue.RAM.Player.Inventory[4]);
                                                }
                                                DrawEngine.MerchantDraw.ReDrawMoney();
                                                DrawEngine.MerchantDraw.DrawGoodsWindow(true);
                                                Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[4]);
                                                DrawEngine.GUIDraw.ReDrawCharStat();
                                                DrawEngine.MerchantDraw.ReDrawMoney();
                                            }
                                            else
                                            { DrawEngine.InfoWindow.Message = "Торговец не может нести столько золота!"; }
                                        }
                                        else
                                        { DrawEngine.InfoWindow.Warning = "Продажа предмета отменена!"; }
                                    }
                                    else { DrawEngine.InfoWindow.Custom("В этой ячейке ничего нет на продажу!"); }
                                }
                            }
                            catch { DrawEngine.InfoWindow.Custom("В этой ячейке ничего нет на продажу!"); }
                            break;
                        }
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        {
                            try
                            {
                                if (!(Rogue.RAM.Merch.GetType() == typeof(MechEngine.Member)))
                                {
                                    if (Rogue.RAM.Player.Inventory[5] != null)
                                    {
                                        DrawEngine.InfoWindow.Warning = "Вы хотите продать " + Rogue.RAM.Player.Inventory[5].Name + " за " + Rogue.RAM.Player.Inventory[5].Sell.ToString() + " золотых?  Y/N?";
                                        ConsoleKey sellkey = Console.ReadKey(true).Key;
                                        if (sellkey == ConsoleKey.Y)
                                        {
                                            if (((Rogue.RAM.Merch as MechEngine.Merchant).CurGold + Rogue.RAM.Player.Inventory[5].Sell) <= (Rogue.RAM.Merch as MechEngine.Merchant).MaxGold)
                                            {
                                                Rogue.RAM.Player.Gold += Rogue.RAM.Player.Inventory[5].Sell;
                                                (Rogue.RAM.Merch as MechEngine.Merchant).CurGold -= Rogue.RAM.Player.Inventory[5].Sell;
                                                DrawEngine.InfoWindow.Sell(Rogue.RAM.Player.Inventory[5].Sell, Rogue.RAM.Player.Inventory[5].Name);
                                                if ((Rogue.RAM.Merch as MechEngine.Merchant).Goods.Count < 12)
                                                {
                                                    (Rogue.RAM.Merch as MechEngine.Merchant).Goods.Add(Rogue.RAM.Player.Inventory[5]);
                                                }
                                                DrawEngine.MerchantDraw.ReDrawMoney();
                                                DrawEngine.MerchantDraw.DrawGoodsWindow(true);
                                                Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[5]);
                                                DrawEngine.GUIDraw.ReDrawCharStat();
                                                DrawEngine.MerchantDraw.ReDrawMoney();
                                            }
                                            else
                                            { DrawEngine.InfoWindow.Message = "Торговец не может нести столько золота!"; }
                                        }
                                        else
                                        { DrawEngine.InfoWindow.Warning = "Продажа предмета отменена!"; }
                                    }
                                    else { DrawEngine.InfoWindow.Custom("В этой ячейке ничего нет на продажу!"); }
                                }
                            }
                            catch { DrawEngine.InfoWindow.Custom("В этой ячейке ничего нет на продажу!"); }
                            break;
                        }
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        {
                            try
                            {
                                if (!(Rogue.RAM.Merch.GetType() == typeof(MechEngine.Member)))
                                {
                                    if (Rogue.RAM.Player.Inventory[0] != null)
                                    {
                                        DrawEngine.InfoWindow.Warning = "Вы хотите продать " + Rogue.RAM.Player.Inventory[0].Name + " за " + Rogue.RAM.Player.Inventory[0].Sell.ToString() + " золотых?  Y/N?";
                                        ConsoleKey sellkey = Console.ReadKey(true).Key;
                                        if (sellkey == ConsoleKey.Y)
                                        {
                                            if (((Rogue.RAM.Merch as MechEngine.Merchant).CurGold + Rogue.RAM.Player.Inventory[0].Sell) <= (Rogue.RAM.Merch as MechEngine.Merchant).MaxGold)
                                            {
                                                Rogue.RAM.Player.Gold += Rogue.RAM.Player.Inventory[0].Sell;
                                                (Rogue.RAM.Merch as MechEngine.Merchant).CurGold -= Rogue.RAM.Player.Inventory[0].Sell;
                                                DrawEngine.InfoWindow.Sell(Rogue.RAM.Player.Inventory[0].Sell, Rogue.RAM.Player.Inventory[0].Name);
                                                if ((Rogue.RAM.Merch as MechEngine.Merchant).Goods.Count < 12)
                                                {
                                                    (Rogue.RAM.Merch as MechEngine.Merchant).Goods.Add(Rogue.RAM.Player.Inventory[0]);
                                                }
                                                DrawEngine.MerchantDraw.ReDrawMoney();
                                                DrawEngine.MerchantDraw.DrawGoodsWindow(true);
                                                Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[0]);
                                                DrawEngine.GUIDraw.ReDrawCharStat();
                                                DrawEngine.MerchantDraw.ReDrawMoney();
                                            }
                                            else
                                            { DrawEngine.InfoWindow.Message = "Торговец не может нести столько золота!"; }
                                        }
                                        else
                                        { DrawEngine.InfoWindow.Warning = "Продажа предмета отменена!"; }
                                    }
                                    else { DrawEngine.InfoWindow.Custom("В этой ячейке ничего нет на продажу!"); }
                                }
                            }
                            catch { DrawEngine.InfoWindow.Custom("В этой ячейке ничего нет на продажу!"); }
                            break;
                        }
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        {
                            try
                            {
                                if (!(Rogue.RAM.Merch.GetType() == typeof(MechEngine.Member)))
                                {
                                    if (Rogue.RAM.Player.Inventory[1] != null)
                                    {
                                        DrawEngine.InfoWindow.Warning = "Вы хотите продать " + Rogue.RAM.Player.Inventory[1].Name + " за " + Rogue.RAM.Player.Inventory[1].Sell.ToString() + " золотых?  Y/N?";
                                        ConsoleKey sellkey = Console.ReadKey(true).Key;
                                        if (sellkey == ConsoleKey.Y)
                                        {
                                            if (((Rogue.RAM.Merch as MechEngine.Merchant).CurGold + Rogue.RAM.Player.Inventory[1].Sell) <= (Rogue.RAM.Merch as MechEngine.Merchant).MaxGold)
                                            {
                                                Rogue.RAM.Player.Gold += Rogue.RAM.Player.Inventory[1].Sell;
                                                (Rogue.RAM.Merch as MechEngine.Merchant).CurGold -= Rogue.RAM.Player.Inventory[1].Sell;
                                                DrawEngine.InfoWindow.Sell(Rogue.RAM.Player.Inventory[1].Sell, Rogue.RAM.Player.Inventory[1].Name);
                                                if ((Rogue.RAM.Merch as MechEngine.Merchant).Goods.Count < 12)
                                                {
                                                    (Rogue.RAM.Merch as MechEngine.Merchant).Goods.Add(Rogue.RAM.Player.Inventory[1]);
                                                }
                                                DrawEngine.MerchantDraw.ReDrawMoney();
                                                DrawEngine.MerchantDraw.DrawGoodsWindow(true);
                                                Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[1]);
                                                DrawEngine.GUIDraw.ReDrawCharStat();
                                                DrawEngine.MerchantDraw.ReDrawMoney();
                                            }
                                            else
                                            { DrawEngine.InfoWindow.Message = "Торговец не может нести столько золота!"; }
                                        }
                                        else
                                        { DrawEngine.InfoWindow.Warning = "Продажа предмета отменена!"; }
                                    }
                                    else { DrawEngine.InfoWindow.Custom("В этой ячейке ничего нет на продажу!"); }
                                }
                            }
                            catch { DrawEngine.InfoWindow.Custom("В этой ячейке ничего нет на продажу!"); }
                            break;
                        }
                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        {
                            try
                            {
                                if (!(Rogue.RAM.Merch.GetType() == typeof(MechEngine.Member)))
                                {
                                    if (Rogue.RAM.Player.Inventory[2] != null)
                                    {
                                        DrawEngine.InfoWindow.Warning = "Вы хотите продать " + Rogue.RAM.Player.Inventory[2].Name + " за " + Rogue.RAM.Player.Inventory[2].Sell.ToString() + " золотых?  Y/N?";
                                        ConsoleKey sellkey = Console.ReadKey(true).Key;
                                        if (sellkey == ConsoleKey.Y)
                                        {
                                            if (((Rogue.RAM.Merch as MechEngine.Merchant).CurGold + Rogue.RAM.Player.Inventory[2].Sell) <= (Rogue.RAM.Merch as MechEngine.Merchant).MaxGold)
                                            {
                                                Rogue.RAM.Player.Gold += Rogue.RAM.Player.Inventory[2].Sell;
                                                (Rogue.RAM.Merch as MechEngine.Merchant).CurGold -= Rogue.RAM.Player.Inventory[2].Sell;
                                                DrawEngine.InfoWindow.Sell(Rogue.RAM.Player.Inventory[2].Sell, Rogue.RAM.Player.Inventory[2].Name);
                                                if ((Rogue.RAM.Merch as MechEngine.Merchant).Goods.Count < 12)
                                                {
                                                    (Rogue.RAM.Merch as MechEngine.Merchant).Goods.Add(Rogue.RAM.Player.Inventory[2]);
                                                }
                                                DrawEngine.MerchantDraw.ReDrawMoney();
                                                DrawEngine.MerchantDraw.DrawGoodsWindow(true);
                                                Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[2]);
                                                DrawEngine.GUIDraw.ReDrawCharStat();
                                                DrawEngine.MerchantDraw.ReDrawMoney();
                                            }
                                            else
                                            { DrawEngine.InfoWindow.Message = "Торговец не может нести столько золота!"; }
                                        }
                                        else
                                        { DrawEngine.InfoWindow.Warning = "Продажа предмета отменена!"; }
                                    }
                                    else { DrawEngine.InfoWindow.Custom("В этой ячейке ничего нет на продажу!"); }
                                }
                            }
                            catch { DrawEngine.InfoWindow.Custom("В этой ячейке ничего нет на продажу!"); }
                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            return;
                        }
                    default:
                        {
                            Merch();
                            break;
                        }
                }
                Merch();
            }
            /// <summary>
            /// Gui quest giver
            /// </summary>
            public static void qGiver()
            {
                ConsoleKey push = Console.ReadKey(true).Key;
                switch (push)
                {
                    case ConsoleKey.Y:
                        {
                            foreach (MechEngine.Quest Q in Rogue.RAM.Player.QuestBook) { if (Q.Name == Rogue.RAM.qGiver.Quest.Name) { DrawEngine.InfoWindow.Custom("У вас уже есть это задание!"); return; } }
                            Rogue.RAM.Player.QuestBook.Add(Rogue.RAM.qGiver.Quest);
                            //Rogue.RAM.qGiver.Quest.Active = true;
                            DrawEngine.InfoWindow.Custom("Вы принимаете задание " + Rogue.RAM.qGiver.Quest.Name + " ! У вас есть 15 минут на выполнение!");
                            Thread.Sleep(500);
                            break;
                        }
                    case ConsoleKey.N:
                        {
                            DrawEngine.InfoWindow.Custom("Вы отказываетесь от задания " + Rogue.RAM.qGiver.Quest.Name + "...");
                            return;
                        }
                    default:
                        {
                            qGiver();
                            break;
                        }
                }
            }
            /// <summary>
            /// Static class for get information
            /// </summary>
            public static class GetInfo
            {
                /// <summary>
                /// see info about anything
                /// </summary>
                public static void GetInfoFromMap()
                {
                    var Lab = Rogue.RAM.Map;

                    int x = 0, y = 0;

                    for (int ys = 0; ys < 23; ys++)
                    {
                        for (int xs = 0; xs < 71; xs++)
                        { if (Lab.Map[xs][ys].Player != null) { x = xs; y = ys; } }
                    }

                    #region NPC
                    if (Lab.Map[x][y].Object != null)
                    {
                        if (Lab.Map[x][y].Object.GetType() == typeof(MechEngine.NPC))
                        { Npc = Lab.Map[x][y].Object as MechEngine.NPC; StopPlay(); return; }
                    }
                    if (Lab.Map[x + 1][y].Object != null)
                    {
                        if (Lab.Map[x + 1][y].Object.GetType() == typeof(MechEngine.NPC))
                        { Npc = Lab.Map[x + 1][y].Object as MechEngine.NPC; StopPlay(); return; }
                    }
                    if (Lab.Map[x - 1][y].Object != null)
                    {
                        if (Lab.Map[x - 1][y].Object.GetType() == typeof(MechEngine.NPC))
                        { Npc = Lab.Map[x - 1][y].Object as MechEngine.NPC; StopPlay(); return; }
                    }
                    if (Lab.Map[x][y + 1].Object != null)
                    {
                        if (Lab.Map[x][y + 1].Object.GetType() == typeof(MechEngine.NPC))
                        { Npc = Lab.Map[x][y + 1].Object as MechEngine.NPC; StopPlay(); return; }
                    }
                    if (Lab.Map[x][y - 1].Object != null)
                    {
                        if (Lab.Map[x][y - 1].Object.GetType() == typeof(MechEngine.NPC))
                        { Npc = Lab.Map[x][y - 1].Object as MechEngine.NPC; StopPlay(); return; }
                    }
                    #endregion

                    #region Member
                    if (Lab.Map[x][y].Object != null)
                    {
                        if (Lab.Map[x][y].Object.GetType() == typeof(MechEngine.Member))
                        { Member = Lab.Map[x][y].Object as MechEngine.Member; StopPlay(); return; }
                    }
                    if (Lab.Map[x + 1][y].Object != null)
                    {
                        if (Lab.Map[x + 1][y].Object.GetType() == typeof(MechEngine.Member))
                        { Member = Lab.Map[x+1][y].Object as MechEngine.Member; StopPlay(); return; }
                    }
                    if (Lab.Map[x - 1][y].Object != null)
                    {
                        if (Lab.Map[x - 1][y].Object.GetType() == typeof(MechEngine.Member))
                        { Member = Lab.Map[x-1][y].Object as MechEngine.Member; StopPlay(); return; }
                    }
                    if (Lab.Map[x][y + 1].Object != null)
                    {
                        if (Lab.Map[x][y + 1].Object.GetType() == typeof(MechEngine.Member))
                        { Member = Lab.Map[x][y+1].Object as MechEngine.Member; StopPlay(); return; }
                    }
                    if (Lab.Map[x][y - 1].Object != null)
                    {
                        if (Lab.Map[x][y - 1].Object.GetType() == typeof(MechEngine.Member))
                        { Member = Lab.Map[x][y-1].Object as MechEngine.Member; StopPlay(); return; }
                    }
                    #endregion

                    #region ActivePbject
                    if (Lab.Map[x][y].Object != null)
                    {
                        if (Lab.Map[x][y].Object.GetType() == typeof(MechEngine.ActiveObject))
                        { ActiveObject = Lab.Map[x][y].Object; StopPlay(); return; }
                    }
                    if (Lab.Map[x + 1][y].Object != null)
                    {
                        if (Lab.Map[x + 1][y].Object.GetType() == typeof(MechEngine.ActiveObject))
                        { ActiveObject = Lab.Map[x + 1][y].Object; StopPlay(); return; }
                    }
                    if (Lab.Map[x - 1][y].Object != null)
                    {
                        if (Lab.Map[x - 1][y].Object.GetType() == typeof(MechEngine.ActiveObject))
                        { ActiveObject = Lab.Map[x - 1][y].Object; StopPlay(); return; }
                    }
                    if (Lab.Map[x][y + 1].Object != null)
                    {
                        if (Lab.Map[x][y + 1].Object.GetType() == typeof(MechEngine.ActiveObject))
                        { ActiveObject = Lab.Map[x][y + 1].Object; StopPlay(); return; }
                    }
                    if (Lab.Map[x][y - 1].Object != null)
                    {
                        if (Lab.Map[x][y - 1].Object.GetType() == typeof(MechEngine.ActiveObject))
                        { ActiveObject = Lab.Map[x][y - 1].Object; StopPlay(); return; }
                    }
                    #endregion

                    #region Fountain, Altar, etc
                    if (Lab.Map[x][y].Object != null)
                    {
                        if (Lab.Map[x][y].Object.GetType() == typeof(MechEngine.Fountain))
                        { ActiveObject = Lab.Map[x][y].Object; StopPlay(); return; }
                    }
                    if (Lab.Map[x + 1][y].Object != null)
                    {
                        if (Lab.Map[x + 1][y].Object.GetType() == typeof(MechEngine.Fountain))
                        { ActiveObject = Lab.Map[x + 1][y].Object; StopPlay(); return; }
                    }
                    if (Lab.Map[x - 1][y].Object != null)
                    {
                        if (Lab.Map[x - 1][y].Object.GetType() == typeof(MechEngine.Fountain))
                        { ActiveObject = Lab.Map[x - 1][y].Object; StopPlay(); return; }
                    }
                    if (Lab.Map[x][y + 1].Object != null)
                    {
                        if (Lab.Map[x][y + 1].Object.GetType() == typeof(MechEngine.Fountain))
                        { ActiveObject = Lab.Map[x][y + 1].Object; StopPlay(); return; }
                    }
                    if (Lab.Map[x][y - 1].Object != null)
                    {
                        if (Lab.Map[x][y - 1].Object.GetType() == typeof(MechEngine.Fountain))
                        { ActiveObject = Lab.Map[x][y - 1].Object; StopPlay(); return; }
                    }
                    #endregion

                    #region Trade
                    if (Lab.Map[x][y].Object != null)
                    {
                        if (Lab.Map[x][y].Object.GetType() == typeof(MechEngine.Merchant))
                        { Merch = Lab.Map[x][y].Object as MechEngine.Merchant; StopPlay(); return; }
                    }
                    if (Lab.Map[x + 1][y].Object != null)
                    {
                        if (Lab.Map[x + 1][y].Object.GetType() == typeof(MechEngine.Merchant))
                        { Merch = Lab.Map[x + 1][y].Object as MechEngine.Merchant; StopPlay(); return; }
                    }
                    if (Lab.Map[x - 1][y].Object != null)
                    {
                        if (Lab.Map[x - 1][y].Object.GetType() == typeof(MechEngine.Merchant))
                        { Merch = Lab.Map[x - 1][y].Object as MechEngine.Merchant; StopPlay(); return; }
                    }
                    if (Lab.Map[x][y + 1].Object != null)
                    {
                        if (Lab.Map[x][y + 1].Object.GetType() == typeof(MechEngine.Merchant))
                        { Merch = Lab.Map[x][y + 1].Object as MechEngine.Merchant; StopPlay(); return; }
                    }
                    if (Lab.Map[x][y - 1].Object != null)
                    {
                        if (Lab.Map[x][y - 1].Object.GetType() == typeof(MechEngine.Merchant))
                        { Merch = Lab.Map[x][y - 1].Object as MechEngine.Merchant; StopPlay(); return; }
                    }
                    #endregion

                    #region Enemy
                    if (Lab.Map[x + 1][y].Enemy != null)
                    { Enemy = Lab.Map[x + 1][y].Enemy; StopPlay(); return; }
                    if (Lab.Map[x - 1][y].Enemy != null)
                    { Enemy = Lab.Map[x - 1][y].Enemy; StopPlay(); return; }
                    if (Lab.Map[x][y + 1].Enemy != null)
                    { Enemy = Lab.Map[x][y + 1].Enemy; StopPlay(); return; }
                    if (Lab.Map[x][y - 1].Enemy != null)
                    { Enemy = Lab.Map[x][y - 1].Enemy; StopPlay(); return; }
                    #endregion

                    #region Item
                    if (Lab.Map[x][y].Item != null)
                    { Item = Lab.Map[x][y].Item; ; StopPlay(); return; }
                    if (Lab.Map[x + 1][y].Item != null)
                    { Item = Lab.Map[x + 1][y].Item; StopPlay(); return; }
                    if (Lab.Map[x - 1][y].Item != null)
                    { Item = Lab.Map[x - 1][y].Item; StopPlay(); return; }
                    if (Lab.Map[x][y + 1].Item != null)
                    { Item = Lab.Map[x][y + 1].Item; StopPlay(); return; }
                    if (Lab.Map[x][y - 1].Item != null)
                    { Item = Lab.Map[x][y - 1].Item; StopPlay(); return; }
                    #endregion
                }
                /// <summary>
                /// Info about npc
                /// </summary>
                public static MechEngine.NPC Npc
                {
                    set
                    {
                        DrawEngine.InfoDraw.NPC = value; 
                    }
                }

                public static MechEngine.Member Member
                { set { DrawEngine.InfoDraw.Member = value; } }
                /// <summary>
                /// Info about merchant
                /// </summary>
                public static MechEngine.Merchant Merch
                {
                    set
                    {
                        DrawEngine.InfoDraw.Trade = value; 
                    }
                }
                /// <summary>
                /// Info about enemy
                /// </summary>
                public static MechEngine.Monster Enemy
                {
                    set { DrawEngine.InfoDraw.Enemy = value;  }
                }
                /// <summary>
                /// Info about item
                /// </summary>
                public static MechEngine.Item Item
                { set { DrawEngine.InfoDraw.Item = value; } }
                /// <summary>
                /// Info about other object
                /// </summary>
                public static MechEngine.ActiveObject ActiveObject
                { set { DrawEngine.InfoDraw.ActiveObject = value; } }
                /// <summary>
                /// StopPlay
                /// </summary>
                private static void StopPlay()
                { PlayEngine.EnemyMoves.Move(false); Console.ReadKey(true); PlayEngine.EnemyMoves.Move(true); }
            }

            //MAP

            public static void EnemyMove()
            {
                while (Rogue.RAM.Bone)
                { RealEnemyMove(); }
            }

            public static void NpcMove()
            {
                while (Rogue.RAM.Bone)
                { RealNpcMove(); }
            }

            private static void RealEnemyMove()
            {
                MechEngine.Labirinth Lab = new MechEngine.Labirinth();
                Lab = Rogue.RAM.Map;
                bool once = false;
                int Coord = 0;

                List<MechEngine.Monster> ml = new List<MechEngine.Monster>();

                for (int y = 0; y < 23; y++)
                {
                    for (int x = 0; x < 71; x++)
                    {
                        if (Lab.Map[x][y].Enemy != null)
                        {
                            ml.Add(Lab.Map[x][y].Enemy);
                        }
                    }
                }

                int q = 0;
                q = ml.Count;

                foreach (MechEngine.Monster m in ml)
                {

                    once = false;
                    
                    Coord = r.Next(4);

                    for (int y = 0; y < 23; y++)
                    {
                        for (int x = 0; x < 71; x++)
                        {
                            if (once == false)
                            {
                                if (Lab.Map[x][y].Enemy == m)
                                {
                                    MechEngine.Monster temp = new MechEngine.Monster();
                                    temp = Lab.Map[x][y].Enemy;
                                    Coord = r.Next(4);
                                    Thread.Sleep(20);
                                    switch (Coord)
                                    {
                                        case 0:
                                            {
                                                if (Lab.Map[x][y - 1].Wall == null && Lab.Map[x][y - 1].Item == null && Lab.Map[x][y - 1].Player == null && Lab.Map[x][y - 1].Object == null && Lab.Map[x][y - 1].Enemy == null)
                                                {
                                                    Lab.Map[x][y].Enemy = null;
                                                    Lab.Map[x][y].Vision = ' ';
                                                    Lab.Map[x][y - 1].Enemy = temp;
                                                    Lab.Map[x][y - 1].Vision = temp.Icon;
                                                    //DrawEngine.LabDraw.ReDrawObject(x, y, x, y - 1, temp);
                                                    DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(Convert.ToInt16(x), Convert.ToInt16(y), Convert.ToInt16(x), Convert.ToInt16(y - 1), ' ', Lab.Map[x][y - 1].Enemy.Icon, 0, Convert.ToInt16(Lab.Map[x][y - 1].Enemy.Chest));
                                                    if (Lab.Map[x][y - 1].Trap != null) { temp.TakeTrap(Lab.Map[x][y - 1].Trap); Lab.Map[x][y - 1].Trap = null; }
                                                    once = true;

                                                }
                                                break;
                                            }
                                        case 1:
                                            {
                                                if (Lab.Map[x][y + 1].Wall == null && Lab.Map[x][y + 1].Item == null && Lab.Map[x][y + 1].Player == null && Lab.Map[x][y + 1].Object == null && Lab.Map[x][y + 1].Enemy == null)
                                                {
                                                    Lab.Map[x][y].Enemy = null;
                                                    Lab.Map[x][y].Vision = ' ';
                                                    Lab.Map[x][y + 1].Enemy = temp;
                                                    Lab.Map[x][y + 1].Vision = temp.Icon;
                                                    //DrawEngine.LabDraw.ReDrawObject(x, y, x, y + 1, temp);
                                                    DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(Convert.ToInt16(x), Convert.ToInt16(y), Convert.ToInt16(x), Convert.ToInt16(y + 1), ' ', Lab.Map[x][y + 1].Enemy.Icon, 0, Convert.ToInt16(Lab.Map[x][y + 1].Enemy.Chest));
                                                    if (Lab.Map[x][y + 1].Trap != null) { temp.TakeTrap(Lab.Map[x][y + 1].Trap); Lab.Map[x][y + 1].Trap = null; }                                                    
                                                    once = true;
                                                }
                                                break;
                                            }
                                        case 2:
                                            {
                                                if (Lab.Map[x - 1][y].Wall == null && Lab.Map[x - 1][y].Item == null && Lab.Map[x - 1][y].Player == null && Lab.Map[x - 1][y].Object == null && Lab.Map[x - 1][y].Enemy == null)
                                                {
                                                    Lab.Map[x][y].Enemy = null;
                                                    Lab.Map[x][y].Vision = ' ';
                                                    Lab.Map[x - 1][y].Enemy = temp;
                                                    Lab.Map[x - 1][y].Vision = temp.Icon;
                                                    //DrawEngine.LabDraw.ReDrawObject(x, y, x - 1, y, temp);
                                                    DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(Convert.ToInt16(x), Convert.ToInt16(y), Convert.ToInt16(x - 1), Convert.ToInt16(y), ' ', Lab.Map[x - 1][y].Enemy.Icon, 0, Convert.ToInt16(Lab.Map[x - 1][y].Enemy.Chest));
                                                    if (Lab.Map[x - 1][y].Trap != null) { temp.TakeTrap(Lab.Map[x - 1][y].Trap); Lab.Map[x - 1][y].Trap = null; }                                                    
                                                    once = true;
                                                }
                                                break;
                                            }
                                        case 3:
                                            {
                                                if (Lab.Map[x + 1][y].Wall == null && Lab.Map[x + 1][y].Item == null && Lab.Map[x + 1][y].Player == null && Lab.Map[x + 1][y].Object == null && Lab.Map[x + 1][y].Enemy == null)
                                                {
                                                    Lab.Map[x][y].Enemy = null;
                                                    Lab.Map[x][y].Vision = ' ';
                                                    Lab.Map[x + 1][y].Enemy = temp;
                                                    Lab.Map[x + 1][y].Vision = temp.Icon;
                                                    //DrawEngine.LabDraw.ReDrawObject(x, y, x + 1, y, temp);
                                                    DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(Convert.ToInt16(x), Convert.ToInt16(y), Convert.ToInt16(x + 1), Convert.ToInt16(y), ' ', Lab.Map[x + 1][y].Enemy.Icon, 0, Convert.ToInt16(Lab.Map[x + 1][y].Enemy.Chest));
                                                    if (Lab.Map[x + 1][y].Trap != null) { temp.TakeTrap(Lab.Map[x + 1][y].Trap); Lab.Map[x + 1][y].Trap = null; }                                                    
                                                    once = true;
                                                }
                                                break;
                                            }
                                    }
                                }
                            }
                            else { break; }
                        }
                    }
                }
                try { EnemyMoves.En.Join(475); }
                catch { }
            }

            private static void RealNpcMove()
            {
                MechEngine.Labirinth Lab = new MechEngine.Labirinth();
                Lab = Rogue.RAM.Map;
                bool once = false;
                int Coord = 0;

                List<MechEngine.ActiveObject> ml = new List<MechEngine.ActiveObject>();

                for (int y = 0; y < 23; y++)
                {
                    for (int x = 0; x < 71; x++)
                    {
                        if (Lab.Map[x][y].Object != null && Lab.Map[x][y].Object.Move)
                        {
                            ml.Add(Lab.Map[x][y].Object);
                        }
                    }
                }

                int q = 0;
                q = ml.Count;

                foreach (MechEngine.ActiveObject m in ml)
                {

                    once = false;
                    
                    Coord = r.Next(4);

                    for (int y = 0; y < 23; y++)
                    {
                        for (int x = 0; x < 71; x++)
                        {
                            if (once == false)
                            {
                                if (Lab.Map[x][y].Object == m && m.Name!="Exit")
                                {                                    
                                    MechEngine.ActiveObject temp = new MechEngine.ActiveObject();
                                    temp = Lab.Map[x][y].Object;
                                    Coord = r.Next(4);
                                    Thread.Sleep(20);
                                    switch (Coord)
                                    {
                                        case 0:
                                            {
                                                if (Lab.Map[x][y - 1].Wall == null && Lab.Map[x][y - 1].Item == null && Lab.Map[x][y - 1].Player == null && Lab.Map[x][y - 1].Object == null && Lab.Map[x][y - 1].Enemy == null)
                                                {
                                                    Lab.Map[x][y].Object = null;
                                                    Lab.Map[x][y].Vision = ' ';
                                                    Lab.Map[x][y - 1].Object = temp;
                                                    Lab.Map[x][y - 1].Vision = temp.Icon;
                                                    //DrawEngine.LabDraw.ReDrawObject(x, y, x, y - 1, temp);                                                    
                                                    DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(Convert.ToInt16(x), Convert.ToInt16(y), Convert.ToInt16(x), Convert.ToInt16(y - 1), ' ', Lab.Map[x][y - 1].Object.Icon, 0, Convert.ToInt16(Lab.Map[x][y - 1].Object.Color));
                                                    once = true;

                                                }
                                                break;
                                            }
                                        case 1:
                                            {
                                                if (Lab.Map[x][y + 1].Wall == null && Lab.Map[x][y + 1].Item == null && Lab.Map[x][y + 1].Player == null && Lab.Map[x][y + 1].Object == null && Lab.Map[x][y + 1].Enemy == null)
                                                {
                                                    Lab.Map[x][y].Object = null;
                                                    Lab.Map[x][y].Vision = ' ';
                                                    Lab.Map[x][y + 1].Object = temp;
                                                    Lab.Map[x][y + 1].Vision = temp.Icon;
                                                    //DrawEngine.LabDraw.ReDrawObject(x, y, x, y + 1, temp);                                                    
                                                    DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(Convert.ToInt16(x), Convert.ToInt16(y), Convert.ToInt16(x), Convert.ToInt16(y + 1), ' ', Lab.Map[x][y + 1].Object.Icon, 0, Convert.ToInt16(Lab.Map[x][y + 1].Object.Color));
                                                    once = true;
                                                }
                                                break;
                                            }
                                        case 2:
                                            {
                                                if (Lab.Map[x - 1][y].Wall == null && Lab.Map[x - 1][y].Item == null && Lab.Map[x - 1][y].Player == null && Lab.Map[x - 1][y].Object == null && Lab.Map[x - 1][y].Enemy == null)
                                                {
                                                    Lab.Map[x][y].Object = null;
                                                    Lab.Map[x][y].Vision = ' ';
                                                    Lab.Map[x - 1][y].Object = temp;
                                                    Lab.Map[x - 1][y].Vision = temp.Icon;
                                                    //DrawEngine.LabDraw.ReDrawObject(x, y, x - 1, y, temp);
                                                    DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(Convert.ToInt16(x), Convert.ToInt16(y), Convert.ToInt16(x-1), Convert.ToInt16(y), ' ', Lab.Map[x-1][y].Object.Icon, 0, Convert.ToInt16(Lab.Map[x-1][y].Object.Color));
                                                    once = true;
                                                }
                                                break;
                                            }
                                        case 3:
                                            {
                                                if (Lab.Map[x + 1][y].Wall == null && Lab.Map[x + 1][y].Item == null && Lab.Map[x + 1][y].Player == null && Lab.Map[x + 1][y].Object == null && Lab.Map[x + 1][y].Enemy == null)
                                                {
                                                    Lab.Map[x][y].Object = null;
                                                    Lab.Map[x][y].Vision = ' ';
                                                    Lab.Map[x + 1][y].Object = temp;
                                                    Lab.Map[x + 1][y].Vision = temp.Icon;
                                                    //DrawEngine.LabDraw.ReDrawObject(x, y, x + 1, y, temp);
                                                    DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(Convert.ToInt16(x), Convert.ToInt16(y), Convert.ToInt16(x + 1), Convert.ToInt16(y), ' ', Lab.Map[x + 1][y].Object.Icon, 0, Convert.ToInt16(Lab.Map[x + 1][y].Object.Color));
                                                    once = true;
                                                }
                                                break;
                                            }
                                    }
                                }
                            }
                        }
                    }
                }
                try { EnemyMoves.En.Join(475); }
                catch { }
            }

            public static void MoveCharacter(int Coord)
            {
                var Lab = Rogue.RAM.Map;

                #region Player

                bool once = false;
                for (int y = 0; y < 23; y++)
                {
                    for (int x = 0; x < 71; x++)
                    {
                        if (once == false)
                        {
                            if (Lab.Map[x][y].Player != null)
                            {
                                switch (Coord)
                                {
                                    case 1:
                                        {
                                            if (Lab.Map[x][y - 1].Wall == null && Lab.Map[x][y - 1].Enemy == null && (Lab.Map[x][y - 1].Object == null || Lab.Map[x][y-1].Object.Icon=='$'))
                                            {
                                                Lab.Map[x][y].Player = null;
                                                Lab.Map[x][y].Vision = ' ';
                                                Lab.Map[x][y - 1].Player = Rogue.RAM.Player;
                                                Lab.Map[x][y - 1].Vision = '@';
                                                DrawEngine.LabDraw.ReDrawObject(x, y, x, y - 1);
                                                once = true;
                                            }
                                            break;
                                        }
                                    case 2:
                                        {
                                            if (Lab.Map[x][y + 1].Wall == null && Lab.Map[x][y + 1].Enemy == null && (Lab.Map[x][y + 1].Object == null || Lab.Map[x][y + 1].Object.Icon == '$'))
                                            {
                                                Lab.Map[x][y].Player = null;
                                                Lab.Map[x][y].Vision = ' ';
                                                Lab.Map[x][y + 1].Player = Rogue.RAM.Player;
                                                Lab.Map[x][y + 1].Vision = '@';
                                                DrawEngine.LabDraw.ReDrawObject(x, y, x, y + 1);
                                                once = true;
                                            }
                                            break;
                                        }
                                    case 3:
                                        {
                                            if (Lab.Map[x - 1][y].Wall == null && Lab.Map[x - 1][y].Enemy == null && (Lab.Map[x-1][y].Object == null || Lab.Map[x-1][y].Object.Icon == '$'))
                                            {
                                                Lab.Map[x][y].Player = null;
                                                Lab.Map[x][y].Vision = ' ';
                                                Lab.Map[x - 1][y].Player = Rogue.RAM.Player;
                                                Lab.Map[x - 1][y].Vision = '@';
                                                DrawEngine.LabDraw.ReDrawObject(x, y, x - 1, y);
                                                once = true;
                                            }
                                            break;
                                        }
                                    case 4:
                                        {
                                            if (Lab.Map[x + 1][y].Wall == null && Lab.Map[x + 1][y].Enemy == null && (Lab.Map[x+1][y].Object == null || Lab.Map[x+1][y].Object.Icon == '$'))
                                            {
                                                Lab.Map[x][y].Player = null;
                                                Lab.Map[x][y].Vision = ' ';
                                                Lab.Map[x + 1][y].Player = Rogue.RAM.Player;
                                                Lab.Map[x + 1][y].Vision = '@';
                                                DrawEngine.LabDraw.ReDrawObject(x, y, x + 1, y);
                                                once = true;
                                            }
                                            break;
                                        }
                                }
                            }
                        }
                    }
                }

                #endregion                

                Rogue.RAM.Map = Lab;

                if (Rogue.RAM.Player.Class == MechEngine.BattleClass.Assassin && (Rogue.RAM.Player.CMP > Rogue.RAM.Player.MMP))
                { Thread.Sleep((Rogue.RAM.Player.CMP - Rogue.RAM.Player.MMP) * 100); }
                
            }

            private static void PlayMenuItem(int Slot)
            {
                try
                {
                    if (Rogue.RAM.Player.Inventory[Slot] != null)
                    {
                        string str = Rogue.RAM.Player.Inventory[Slot].Name;
                        if (str.Length > 16) { str = str.Substring(0, 16); }
                        DrawEngine.InfoWindow.Message = str + ": [D] - Выбросить | [U] - Использовать | [I] - Информация | [Esc] - Отмена.";
                        ConsoleKey push = Console.ReadKey(true).Key;
                        switch (push)
                        {
                            case ConsoleKey.D:
                                {
                                    DropItem(Slot);
                                    break;
                                }
                            case ConsoleKey.U:
                                {
                                    UseItem(Slot);
                                    break;
                                }
                            case ConsoleKey.I:
                                {
                                    Enemy = false;
                                    GetInfo.Item = Rogue.RAM.Player.Inventory[Slot];
                                    Console.ReadKey(true);
                                    break;
                                }
                            case ConsoleKey.Escape:
                                {
                                    DrawEngine.InfoWindow.Clear();
                                    Play();
                                    break;
                                }
                            default:
                                {
                                    PlayMenuItem(Slot);
                                    break;
                                }
                        }
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    //ignore
                    Play();
                }
            }

            private static void UseItem(int Slot)
            {
                try
                {
                    MechEngine.Item I = Rogue.RAM.Player.Inventory[Slot];
                    if (I.Icon() == '⌂')
                    { I.Use(); }
                    else if (I.Icon() != '*' && I.Icon() != '■')
                    {
                        useEquip(I,Slot);
                        DrawEngine.GUIDraw.ReDrawCharStat();
                    }                    
                    else
                    {
                        Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[Slot]);
                        if (I.Icon() == '*') { usePotion((I as MechEngine.Item.Potion)); DrawEngine.InfoWindow.UseItem(I); }
                        if (I.Icon() == '■') { useScroll((I as MechEngine.Item.Scroll)); DrawEngine.GUIDraw.ReDrawCharStat(); }

                    }
                }
                catch { DrawEngine.InfoWindow.Message = "Эта ячейка инвентаря пустая!"; }
            }

            private static void usePotion(MechEngine.Item.Potion i)
            {
                SoundEngine.Sound.Drink();
                if (i.HP != 0)
                {
                    i.WasCHP = Rogue.RAM.Player.CHP;
                    Rogue.RAM.Player.CHP += i.HP;
                    if (Rogue.RAM.Player.CHP > Rogue.RAM.Player.MHP)
                    {
                        Rogue.RAM.Player.CHP = Rogue.RAM.Player.MHP;
                    }
                }
                if (i.MP != 0)
                {
                    i.WasCMP = Rogue.RAM.Player.CMP;
                    Rogue.RAM.Player.CMP += i.MP;
                    if (Rogue.RAM.Player.CMP > Rogue.RAM.Player.MMP)
                    {
                        Rogue.RAM.Player.CMP = Rogue.RAM.Player.MMP;
                    }
                }
            }

            private static void useEquip(MechEngine.Item i, int index)
            {
                if (Rogue.RAM.Enemy == null)
                {
                    MechEngine.Item old = new MechEngine.Item();
                    MechEngine.Item New = new MechEngine.Item();
                    switch (i.Kind)
                    {
                        case MechEngine.Kind.Armor:
                            {
                                SoundEngine.Sound.Eqip();
                                old = Rogue.RAM.Player.Equipment.Armor;
                                New = i;
                                Rogue.RAM.Player.Equipment.Armor = (i as MechEngine.Item.Armor); 
                                break;
                            }
                        case MechEngine.Kind.Boots:
                            {
                                SoundEngine.Sound.Eqip();
                                old = Rogue.RAM.Player.Equipment.Boots;
                                New = i;
                                Rogue.RAM.Player.Equipment.Boots = (i as MechEngine.Item.Boots);
                                break;
                            }
                        case MechEngine.Kind.Helm:
                            {
                                SoundEngine.Sound.Eqip();
                                old = Rogue.RAM.Player.Equipment.Helm;
                                New = i;
                                Rogue.RAM.Player.Equipment.Helm = (i as MechEngine.Item.Helm);
                                break;
                            }
                        case MechEngine.Kind.OffHand:
                            {
                                SoundEngine.Sound.Eqip();
                                old = Rogue.RAM.Player.Equipment.OffHand;
                                New = i;
                                Rogue.RAM.Player.Equipment.OffHand = (i as MechEngine.Item.OffHand);
                                break;
                            }
                        case MechEngine.Kind.Weapon:
                            {
                                SoundEngine.Sound.Eqip();
                                old = Rogue.RAM.Player.Equipment.Weapon;
                                New = i;
                                Rogue.RAM.Player.Equipment.Weapon = (i as MechEngine.Item.Weapon);                                
                                break;
                            }
                        default: { DrawEngine.InfoWindow.Warning = "Можно экипировать только одежду!"; return; }
                    }
                    if (old != null) { Rogue.RAM.Player.Inventory[index] = old; old.UnDress(); New.Dress(); }
                    else { Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[index]); New.Dress(); }
                    DrawEngine.InfoWindow.UseEquip(old, New, true);
                }
                else { DrawEngine.InfoWindow.UseEquip(null, null, false); }
            }

            private static void useScroll(MechEngine.Item.Scroll i)
            {
                i.Spell.Activate();
            }

            private static void useElixir(MechEngine.Item.Elixir i)
            {

            }

            private static void DropItem(int Slot)
            {
                var Lab = Rogue.RAM.Map;

                bool once = false;
                bool open = false;

                for (int y = 0; y < 23; y++)
                {
                    for (int x = 0; x < 71; x++)
                    {
                        if (once == false)
                        {
                            if (Lab.Map[x][y].Player != null)
                            {
                                if (Lab.Map[x + 1][y].Item == null && Lab.Map[x + 1][y].Wall == null && Lab.Map[x + 1][y].Object == null && Lab.Map[x + 1][y].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.DropItem();
                                        Lab.Map[x + 1][y].Item = Rogue.RAM.Player.Inventory[Slot];
                                        Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[Slot]);
                                        DrawEngine.LabDraw.ReDrawObject(x + 1, y, Lab.Map[x + 1][y].Item);
                                        DrawEngine.InfoWindow.DropLoot(Lab.Map[x + 1][y].Item);
                                        once = true;
                                        open = true;
                                    }
                                }
                                if (Lab.Map[x - 1][y].Item == null && Lab.Map[x - 1][y].Wall == null && Lab.Map[x - 1][y].Object == null && Lab.Map[x - 1][y].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.DropItem();
                                        Lab.Map[x - 1][y].Item = Rogue.RAM.Player.Inventory[Slot];
                                        Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[Slot]);
                                        DrawEngine.LabDraw.ReDrawObject(x - 1, y, Lab.Map[x - 1][y].Item);
                                        DrawEngine.InfoWindow.DropLoot(Lab.Map[x - 1][y].Item);
                                        once = true;
                                        open = true;
                                    }
                                }
                                if (Lab.Map[x][y + 1].Item == null && Lab.Map[x][y + 1].Wall == null && Lab.Map[x][y + 1].Object == null && Lab.Map[x][y + 1].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.DropItem();
                                        Lab.Map[x][y + 1].Item = Rogue.RAM.Player.Inventory[Slot];
                                        Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[Slot]);
                                        DrawEngine.LabDraw.ReDrawObject(x, y + 1, Lab.Map[x][y + 1].Item);
                                        DrawEngine.InfoWindow.DropLoot(Lab.Map[x][y + 1].Item);
                                        once = true;
                                        open = true;
                                    }
                                }
                                if (Lab.Map[x][y - 1].Item == null && Lab.Map[x][y - 1].Wall == null && Lab.Map[x][y - 1].Object == null && Lab.Map[x][y - 1].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.DropItem();
                                        Lab.Map[x][y - 1].Item = Rogue.RAM.Player.Inventory[Slot];
                                        Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[Slot]);
                                        DrawEngine.LabDraw.ReDrawObject(x, y - 1, Lab.Map[x][y - 1].Item);
                                        DrawEngine.InfoWindow.DropLoot(Lab.Map[x][y - 1].Item);
                                        once = true;
                                        open = true;
                                    }
                                }
                                if (Lab.Map[x + 1][y + 1].Item == null && Lab.Map[x + 1][y + 1].Wall == null && Lab.Map[x + 1][y + 1].Object == null && Lab.Map[x + 1][y + 1].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.DropItem();
                                        Lab.Map[x + 1][y + 1].Item = Rogue.RAM.Player.Inventory[Slot];
                                        Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[Slot]);
                                        DrawEngine.LabDraw.ReDrawObject(x + 1, y + 1, Lab.Map[x + 1][y + 1].Item);
                                        DrawEngine.InfoWindow.DropLoot(Lab.Map[x + 1][y + 1].Item);
                                        once = true;
                                        open = true;
                                    }
                                }
                                if (Lab.Map[x + 1][y - 1].Item == null && Lab.Map[x + 1][y - 1].Wall == null && Lab.Map[x + 1][y - 1].Object == null && Lab.Map[x + 1][y - 1].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.DropItem();
                                        Lab.Map[x + 1][y - 1].Item = Rogue.RAM.Player.Inventory[Slot];
                                        Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[Slot]);
                                        DrawEngine.LabDraw.ReDrawObject(x + 1, y - 1, Lab.Map[x + 1][y - 1].Item);
                                        DrawEngine.InfoWindow.DropLoot(Lab.Map[x + 1][y - 1].Item);
                                        once = true;
                                        open = true;
                                    }
                                }
                                if (Lab.Map[x - 1][y - 1].Item == null && Lab.Map[x - 1][y - 1].Wall == null && Lab.Map[x - 1][y - 1].Object == null && Lab.Map[x - 1][y - 1].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.DropItem();
                                        Lab.Map[x - 1][y - 1].Item = Rogue.RAM.Player.Inventory[Slot];
                                        Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[Slot]);
                                        DrawEngine.LabDraw.ReDrawObject(x - 1, y - 1, Lab.Map[x - 1][y - 1].Item);
                                        DrawEngine.InfoWindow.DropLoot(Lab.Map[x - 1][y - 1].Item);
                                        once = true;
                                        open = true;
                                    }
                                }
                                if (Lab.Map[x - 1][y + 1].Item == null && Lab.Map[x - 1][y + 1].Wall == null && Lab.Map[x - 1][y + 1].Object == null && Lab.Map[x - 1][y + 1].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.DropItem();
                                        Lab.Map[x - 1][y + 1].Item = Rogue.RAM.Player.Inventory[Slot];
                                        Rogue.RAM.Player.Inventory.Remove(Rogue.RAM.Player.Inventory[Slot]);
                                        DrawEngine.LabDraw.ReDrawObject(x - 1, y + 1, Lab.Map[x - 1][y + 1].Item);
                                        DrawEngine.InfoWindow.DropLoot(Lab.Map[x - 1][y + 1].Item);
                                        once = true;
                                        open = true;
                                    }
                                }
                            }
                        }
                    }
                }
                Rogue.RAM.Map = Lab;
                //DrawEngine.GUIDraw.ReDrawCharStat();
                DrawEngine.GUIDraw.ReDrawCharInventory();
                if (open == false)
                {
                    DrawEngine.InfoWindow.Custom("Невозможно выбросить предмет.");
                    Play();
                }
            }

            public static void DropItem(MechEngine.Item it)
            {
                var Lab = Rogue.RAM.Map;

                bool once = false;
                bool open = false;

                for (int y = 0; y < 23; y++)
                {
                    for (int x = 0; x < 71; x++)
                    {
                        if (once == false)
                        {
                            if (Lab.Map[x][y].Player != null)
                            {
                                if (Lab.Map[x + 1][y].Item == null && Lab.Map[x + 1][y].Wall == null && Lab.Map[x + 1][y].Object == null && Lab.Map[x + 1][y].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.DropItem();
                                        Lab.Map[x + 1][y].Item = it;                                        
                                        DrawEngine.LabDraw.ReDrawObject(x + 1, y, Lab.Map[x + 1][y].Item);
                                        DrawEngine.InfoWindow.DropLoot(Lab.Map[x + 1][y].Item);
                                        once = true;
                                        open = true;
                                    }
                                }
                                if (Lab.Map[x - 1][y].Item == null && Lab.Map[x - 1][y].Wall == null && Lab.Map[x - 1][y].Object == null && Lab.Map[x - 1][y].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.DropItem();
                                        Lab.Map[x - 1][y].Item = it;                                        
                                        DrawEngine.LabDraw.ReDrawObject(x - 1, y, Lab.Map[x - 1][y].Item);
                                        DrawEngine.InfoWindow.DropLoot(Lab.Map[x - 1][y].Item);
                                        once = true;
                                        open = true;
                                    }
                                }
                                if (Lab.Map[x][y + 1].Item == null && Lab.Map[x][y + 1].Wall == null && Lab.Map[x][y + 1].Object == null && Lab.Map[x][y + 1].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.DropItem();
                                        Lab.Map[x][y + 1].Item = it;                                        
                                        DrawEngine.LabDraw.ReDrawObject(x, y + 1, Lab.Map[x][y + 1].Item);
                                        DrawEngine.InfoWindow.DropLoot(Lab.Map[x][y + 1].Item);
                                        once = true;
                                        open = true;
                                    }
                                }
                                if (Lab.Map[x][y - 1].Item == null && Lab.Map[x][y - 1].Wall == null && Lab.Map[x][y - 1].Object == null && Lab.Map[x][y - 1].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.DropItem();
                                        Lab.Map[x][y - 1].Item = it;                                        
                                        DrawEngine.LabDraw.ReDrawObject(x, y - 1, Lab.Map[x][y - 1].Item);
                                        DrawEngine.InfoWindow.DropLoot(Lab.Map[x][y - 1].Item);
                                        once = true;
                                        open = true;
                                    }
                                }
                                if (Lab.Map[x + 1][y + 1].Item == null && Lab.Map[x + 1][y + 1].Wall == null && Lab.Map[x + 1][y + 1].Object == null && Lab.Map[x + 1][y + 1].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.DropItem();
                                        Lab.Map[x + 1][y + 1].Item = it;                                        
                                        DrawEngine.LabDraw.ReDrawObject(x + 1, y + 1, Lab.Map[x + 1][y + 1].Item);
                                        DrawEngine.InfoWindow.DropLoot(Lab.Map[x + 1][y + 1].Item);
                                        once = true;
                                        open = true;
                                    }
                                }
                                if (Lab.Map[x + 1][y - 1].Item == null && Lab.Map[x + 1][y - 1].Wall == null && Lab.Map[x + 1][y - 1].Object == null && Lab.Map[x + 1][y - 1].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.DropItem();
                                        Lab.Map[x + 1][y - 1].Item = it;                                        
                                        DrawEngine.LabDraw.ReDrawObject(x + 1, y - 1, Lab.Map[x + 1][y - 1].Item);
                                        DrawEngine.InfoWindow.DropLoot(Lab.Map[x + 1][y - 1].Item);
                                        once = true;
                                        open = true;
                                    }
                                }
                                if (Lab.Map[x - 1][y - 1].Item == null && Lab.Map[x - 1][y - 1].Wall == null && Lab.Map[x - 1][y - 1].Object == null && Lab.Map[x - 1][y - 1].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.DropItem();
                                        Lab.Map[x - 1][y - 1].Item = it;                                        
                                        DrawEngine.LabDraw.ReDrawObject(x - 1, y - 1, Lab.Map[x - 1][y - 1].Item);
                                        DrawEngine.InfoWindow.DropLoot(Lab.Map[x - 1][y - 1].Item);
                                        once = true;
                                        open = true;
                                    }
                                }
                                if (Lab.Map[x - 1][y + 1].Item == null && Lab.Map[x - 1][y + 1].Wall == null && Lab.Map[x - 1][y + 1].Object == null && Lab.Map[x - 1][y + 1].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.DropItem();
                                        Lab.Map[x - 1][y + 1].Item = it;                                        
                                        DrawEngine.LabDraw.ReDrawObject(x - 1, y + 1, Lab.Map[x - 1][y + 1].Item);
                                        DrawEngine.InfoWindow.DropLoot(Lab.Map[x - 1][y + 1].Item);
                                        once = true;
                                        open = true;
                                    }
                                }
                            }
                        }
                    }
                }
                Rogue.RAM.Map = Lab;
                //DrawEngine.GUIDraw.ReDrawCharStat();
                DrawEngine.GUIDraw.ReDrawCharInventory();
                if (open == false)
                {
                    DrawEngine.InfoWindow.Custom("Невозможно выбросить предмет.");
                    Play();
                }
            }

            public static void DropItemI(MechEngine.Item i)
            {
                var Lab = Rogue.RAM.Map;

                bool once = false;
                bool open = false;

                for (int y = 0; y < 23; y++)
                {
                    for (int x = 0; x < 71; x++)
                    {
                        if (once == false)
                        {
                            if (Lab.Map[x][y].Player != null)
                            {
                                if (Lab.Map[x + 1][y].Item == null && Lab.Map[x + 1][y].Wall == null && Lab.Map[x + 1][y].Object == null && Lab.Map[x + 1][y].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        Lab.Map[x + 1][y].Item = i;
                                        if (Rogue.RAM.Enemy != null)
                                        {
                                            DrawEngine.LabDraw.ReDrawObject(x + 1, y, Lab.Map[x + 1][y].Item);
                                        }
                                        once = true;
                                        open = true;
                                    }
                                }
                                if (Lab.Map[x - 1][y].Item == null && Lab.Map[x - 1][y].Wall == null && Lab.Map[x - 1][y].Object == null && Lab.Map[x - 1][y].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        Lab.Map[x - 1][y].Item = i;
                                        if (Rogue.RAM.Enemy != null)
                                        {
                                            DrawEngine.LabDraw.ReDrawObject(x - 1, y, Lab.Map[x - 1][y].Item);
                                        }
                                        once = true;
                                        open = true;
                                    }
                                }
                                if (Lab.Map[x][y + 1].Item == null && Lab.Map[x][y + 1].Wall == null && Lab.Map[x][y + 1].Object == null && Lab.Map[x][y + 1].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        Lab.Map[x][y + 1].Item = i;
                                        if (Rogue.RAM.Enemy != null)
                                        {
                                            DrawEngine.LabDraw.ReDrawObject(x, y + 1, Lab.Map[x][y + 1].Item);
                                        }
                                        once = true;
                                        open = true;
                                    }
                                }
                                if (Lab.Map[x][y - 1].Item == null && Lab.Map[x][y - 1].Wall == null && Lab.Map[x][y - 1].Object == null && Lab.Map[x][y - 1].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        Lab.Map[x][y - 1].Item = i;
                                        if (Rogue.RAM.Enemy != null)
                                        {
                                            DrawEngine.LabDraw.ReDrawObject(x, y - 1, Lab.Map[x][y - 1].Item);
                                        }
                                        once = true;
                                        open = true;
                                    }
                                }
                                if (Lab.Map[x + 1][y + 1].Item == null && Lab.Map[x + 1][y + 1].Wall == null && Lab.Map[x + 1][y + 1].Object == null && Lab.Map[x + 1][y + 1].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        Lab.Map[x + 1][y + 1].Item = i;
                                        if (Rogue.RAM.Enemy != null)
                                        {
                                            DrawEngine.LabDraw.ReDrawObject(x + 1, y + 1, Lab.Map[x + 1][y + 1].Item);
                                        }
                                        once = true;
                                        open = true;
                                    }
                                }
                                if (Lab.Map[x + 1][y - 1].Item == null && Lab.Map[x + 1][y - 1].Wall == null && Lab.Map[x + 1][y - 1].Object == null && Lab.Map[x + 1][y - 1].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        Lab.Map[x + 1][y - 1].Item = i;
                                        if (Rogue.RAM.Enemy != null)
                                        {
                                            DrawEngine.LabDraw.ReDrawObject(x + 1, y - 1, Lab.Map[x + 1][y - 1].Item);
                                        }
                                        once = true;
                                        open = true;
                                    }
                                }
                                if (Lab.Map[x - 1][y - 1].Item == null && Lab.Map[x - 1][y - 1].Wall == null && Lab.Map[x - 1][y - 1].Object == null && Lab.Map[x - 1][y - 1].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        Lab.Map[x - 1][y - 1].Item = i;
                                        if (Rogue.RAM.Enemy != null)
                                        {
                                            DrawEngine.LabDraw.ReDrawObject(x - 1, y - 1, Lab.Map[x - 1][y - 1].Item);
                                        }
                                        once = true;
                                        open = true;
                                    }
                                }
                                if (Lab.Map[x - 1][y + 1].Item == null && Lab.Map[x - 1][y + 1].Wall == null && Lab.Map[x - 1][y + 1].Object == null && Lab.Map[x - 1][y + 1].Enemy == null)
                                {
                                    if (open == false)
                                    {
                                        Lab.Map[x - 1][y + 1].Item = i;
                                        if (Rogue.RAM.Enemy != null)
                                        {
                                            DrawEngine.LabDraw.ReDrawObject(x - 1, y + 1, Lab.Map[x - 1][y + 1].Item);
                                        }
                                        once = true;
                                        open = true;
                                    }
                                }
                            }
                        }
                    }
                }
                Rogue.RAM.Map = Lab;
                if (open == false)
                {
                    DrawEngine.InfoWindow.Custom("Наградной предмет(ы) был утерян...");
                }
                else
                {
                    DrawEngine.InfoWindow.Message = "Под ногами вы находите наградной предмет(ы)!";
                }
            }

            private static void OpenDoor()
            {
                var Lab = Rogue.RAM.Map;

                bool once = false;
                bool open = false;

                for (int y = 0; y < 23; y++)
                {
                    for (int x = 0; x < 71; x++)
                    {
                        if (once == false)
                        {
                            if (Lab.Map[x][y].Player != null)
                            {
                                #region Merchant Head

                                if (Lab.Map[x][y].Object != null && Lab.Map[x][y].Object.Icon=='$' && !once)
                                {
                                    if (open == false)
                                    {
                                        if (Lab.Map[x][y].Object.Use() == true)
                                        {                                            
                                            Lab.Map[x][y].Object = null;
                                            DrawEngine.LabDraw.ReDrawObject(x, y);
                                            DrawEngine.GUIDraw.ReDrawCharStat();
                                            open = true;
                                            once = true;
                                        }
                                    }
                                }

                                #endregion

                                if (Lab.Map[x + 1][y].Object != null && !once)
                                {
                                    if (open == false)
                                    {
                                        if (Lab.Map[x + 1][y].Object.Use() == true)
                                        {                                            
                                            Lab.Map[x + 1][y].Object = null;
                                            DrawEngine.LabDraw.ReDrawObject(x + 1, y);
                                            DrawEngine.GUIDraw.ReDrawCharStat();
                                            open = true;
                                            once = true;
                                        }
                                    }
                                }
                                if (Lab.Map[x - 1][y].Object != null && !once)
                                {
                                    if (open == false)
                                    {
                                        if (Lab.Map[x - 1][y].Object.Use() == true)
                                        {                                           
                                            Lab.Map[x - 1][y].Object = null;
                                            DrawEngine.LabDraw.ReDrawObject(x - 1, y);
                                            DrawEngine.GUIDraw.ReDrawCharStat();
                                            open = true;
                                            once = true;
                                        }
                                    }
                                }
                                if (Lab.Map[x][y + 1].Object != null && !once)
                                {
                                    if (open == false)
                                    {
                                        if (Lab.Map[x][y + 1].Object.Use() == true)
                                        {                                            
                                            Lab.Map[x][y + 1].Object = null;
                                            DrawEngine.LabDraw.ReDrawObject(x, y + 1);
                                            DrawEngine.GUIDraw.ReDrawCharStat();
                                            open = true;
                                            once = true;
                                        }
                                    }
                                }
                                if (Lab.Map[x][y - 1].Object != null && !once)
                                {
                                    if (open == false)
                                    {
                                        if (Lab.Map[x][y - 1].Object.Use() == true)
                                        {                                           
                                            Lab.Map[x][y - 1].Object = null;
                                            DrawEngine.LabDraw.ReDrawObject(x, y - 1);
                                            DrawEngine.GUIDraw.ReDrawCharStat();
                                            open = true;
                                            once = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                Rogue.RAM.Map = Lab;                
                //DrawEngine.GUIDraw.DrawGUI();
            }

            private static void Destroy()
            {
                var Lab = Rogue.RAM.Map;

                bool once = false;
                bool open = false;

                for (int y = 0; y < 23; y++)
                {
                    for (int x = 0; x < 71; x++)
                    {
                        if (once == false)
                        {
                            if (Lab.Map[x][y].Player != null)
                            {
                                if (Lab.Map[x + 1][y].Item != null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.DestroyItem();
                                        DrawEngine.InfoWindow.DestroyLoot(Lab.Map[x + 1][y].Item);
                                        Lab.Map[x + 1][y].Item = null;
                                        DrawEngine.LabDraw.ReDrawObject(x + 1, y);
                                        open = true;
                                        once = true;
                                    }
                                }
                                if (Lab.Map[x - 1][y].Item != null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.DestroyItem();
                                        DrawEngine.InfoWindow.DestroyLoot(Lab.Map[x - 1][y].Item);
                                        Lab.Map[x - 1][y].Item = null;
                                        DrawEngine.LabDraw.ReDrawObject(x - 1, y);
                                        open = true;
                                        once = true;
                                    }
                                }
                                if (Lab.Map[x][y + 1].Item != null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.DestroyItem();
                                        DrawEngine.InfoWindow.DestroyLoot(Lab.Map[x][y + 1].Item);
                                        Lab.Map[x][y + 1].Item = null;
                                        DrawEngine.LabDraw.ReDrawObject(x, y + 1);
                                        open = true;
                                        once = true;
                                    }
                                }
                                if (Lab.Map[x][y - 1].Item != null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.DestroyItem();
                                        DrawEngine.InfoWindow.DestroyLoot(Lab.Map[x][y - 1].Item);
                                        Lab.Map[x][y - 1].Item = null;
                                        DrawEngine.LabDraw.ReDrawObject(x, y - 1);
                                        open = true;
                                        once = true;
                                    }
                                }
                            }
                        }
                    }
                }

                Rogue.RAM.Map = Lab;                
                //DrawEngine.GUIDraw.DrawGUI();
            }            

            private static void TakeItem()
            {
                var Lab = Rogue.RAM.Map;

                bool once = false;
                bool open = false;

                for (int y = 0; y < 23; y++)
                {
                    for (int x = 0; x < 71; x++)
                    {
                        if (once == false)
                        {
                            if (Lab.Map[x][y].Player != null)
                            {
                                #region Item Head

                                if (Lab.Map[x][y].Item != null)
                                {

                                    if (open == false)
                                    {
                                        if (Rogue.RAM.Player.InventorySlots)
                                        {
                                            SoundEngine.Sound.TakeItem();
                                            if (Lab.Map[x][y].Item.Kind == MechEngine.Kind.Poison)
                                            { Lab.Map[x][y].Item.TakePoison(); }
                                            else
                                            {
                                                DrawEngine.InfoWindow.Loot(true, Lab.Map[x][y].Item);
                                                Rogue.RAM.Player.Inventory.Add(Lab.Map[x][y].Item);
                                                Rogue.RAM.Player.QuestItem = Lab.Map[x][y].Item.Name;
                                                DrawEngine.GUIDraw.ReDrawCharInventory();
                                            }
                                            Lab.Map[x][y].Item = null;
                                            DrawEngine.LabDraw.ReDrawObject(x, y);
                                            open = true;
                                            once = true;
                                        }
                                        else
                                        {
                                            DrawEngine.InfoWindow.Loot(false, Lab.Map[x + 1][y].Item);
                                        }
                                    }

                                }

                                #endregion

                                if (Lab.Map[x + 1][y].Item != null)
                                {

                                    if (open == false)
                                    {
                                        if (Rogue.RAM.Player.InventorySlots)
                                        {
                                            SoundEngine.Sound.TakeItem();
                                            if (Lab.Map[x + 1][y].Item.Kind == MechEngine.Kind.Poison)
                                            { Lab.Map[x + 1][y].Item.TakePoison(); }
                                            else
                                            {
                                                DrawEngine.InfoWindow.Loot(true, Lab.Map[x + 1][y].Item);
                                                Rogue.RAM.Player.Inventory.Add(Lab.Map[x + 1][y].Item);
                                                Rogue.RAM.Player.QuestItem = Lab.Map[x + 1][y].Item.Name;
                                                DrawEngine.GUIDraw.ReDrawCharInventory();
                                            }
                                            Lab.Map[x + 1][y].Item = null;
                                            DrawEngine.LabDraw.ReDrawObject(x + 1, y);
                                            open = true;
                                            once = true;
                                        }
                                        else
                                        {
                                            DrawEngine.InfoWindow.Loot(false, Lab.Map[x + 1][y].Item);
                                        }
                                    }

                                }
                                if (Lab.Map[x - 1][y].Item != null)
                                {
                                    if (open == false)
                                    {
                                        if (Rogue.RAM.Player.InventorySlots)
                                        {
                                            SoundEngine.Sound.TakeItem();
                                            if (Lab.Map[x - 1][y].Item.Kind == MechEngine.Kind.Poison)
                                            { Lab.Map[x - 1][y].Item.TakePoison(); }
                                            else
                                            {
                                                DrawEngine.InfoWindow.Loot(true, Lab.Map[x - 1][y].Item);
                                                Rogue.RAM.Player.Inventory.Add(Lab.Map[x - 1][y].Item);
                                                Rogue.RAM.Player.QuestItem = Lab.Map[x - 1][y].Item.Name;
                                                DrawEngine.GUIDraw.ReDrawCharInventory();
                                            }
                                            Lab.Map[x - 1][y].Item = null;
                                            DrawEngine.LabDraw.ReDrawObject(x - 1, y);
                                            open = true;
                                            once = true;
                                        }
                                        else
                                        {
                                            DrawEngine.InfoWindow.Loot(false, Lab.Map[x - 1][y].Item);
                                        }
                                    }
                                }
                                if (Lab.Map[x][y + 1].Item != null)
                                {
                                    if (open == false)
                                    {
                                        if (Rogue.RAM.Player.InventorySlots)
                                        {
                                            SoundEngine.Sound.TakeItem();
                                            if (Lab.Map[x][y+1].Item.Kind == MechEngine.Kind.Poison)
                                            { Lab.Map[x][y+1].Item.TakePoison(); }
                                            else
                                            {
                                                DrawEngine.InfoWindow.Loot(true, Lab.Map[x][y + 1].Item);
                                                Rogue.RAM.Player.Inventory.Add(Lab.Map[x][y + 1].Item);
                                                Rogue.RAM.Player.QuestItem = Lab.Map[x][y + 1].Item.Name;
                                                DrawEngine.GUIDraw.ReDrawCharInventory();
                                            }
                                            Lab.Map[x][y + 1].Item = null;
                                            DrawEngine.LabDraw.ReDrawObject(x, y + 1);
                                            open = true;
                                            once = true;
                                        }
                                        else
                                        {
                                            DrawEngine.InfoWindow.Loot(false, Lab.Map[x][y + 1].Item);
                                        }
                                    }
                                }
                                if (Lab.Map[x][y - 1].Item != null)
                                {
                                    if (open == false)
                                    {
                                        if (Rogue.RAM.Player.InventorySlots)
                                        {
                                            SoundEngine.Sound.TakeItem();
                                            if (Lab.Map[x ][y-1].Item.Kind == MechEngine.Kind.Poison)
                                            { Lab.Map[x ][y-1].Item.TakePoison(); }
                                            else
                                            {
                                                DrawEngine.InfoWindow.Loot(true, Lab.Map[x][y - 1].Item);
                                                Rogue.RAM.Player.Inventory.Add(Lab.Map[x][y - 1].Item);
                                                Rogue.RAM.Player.QuestItem = Lab.Map[x][y - 1].Item.Name;
                                                DrawEngine.GUIDraw.ReDrawCharInventory();
                                            }
                                            Lab.Map[x][y - 1].Item = null;
                                            DrawEngine.LabDraw.ReDrawObject(x, y - 1);
                                            open = true;
                                            once = true;
                                        }
                                        else
                                        {
                                            DrawEngine.InfoWindow.Loot(false, Lab.Map[x][y - 1].Item);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                Rogue.RAM.Map = Lab;
                //DrawEngine.GUIDraw.ReDrawCharInventory();
            }

            private static void Attack()
            {
                var Lab = Rogue.RAM.Map;

                

                bool once = false;
                bool open = false;

                for (int y = 0; y < 23; y++)
                {
                    for (int x = 0; x < 71; x++)
                    {
                        if (once == false)
                        {
                            if (Lab.Map[x][y].Player != null)
                            {
                                if (Lab.Map[x + 1][y].Enemy != null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.Attack();
                                        EnemyMoves.Move(false);
                                        Rogue.RAM.Enemy = Lab.Map[x + 1][y].Enemy;
                                        DrawEngine.FightDraw.DrawFight();
                                        Rogue.RAM.Log = new List<string>();
                                        Rogue.RAM.EnemyX = x + 1;
                                        Rogue.RAM.EnemyY = y;
                                        Lab.Map[x + 1][y].Item = Lab.Map[x + 1][y].Enemy.Loot;
                                        Lab.Map[x + 1][y].Enemy = null;
                                        SystemEngine.Helper.Activation.StartBattle();
                                        DrawEngine.CharMap.DrawCMap(new List<string>() 
                                        { 
                                            "[Q] - "+Rogue.RAM.Player.Ability[0].Name,
                                            "[W] - "+Rogue.RAM.Player.Ability[1].Name, 
                                            "[E] - "+Rogue.RAM.Player.Ability[2].Name,
                                            "[R] - "+Rogue.RAM.Player.Ability[3].Name,
                                            "[A] - Удар рукой",
                                            "[D] - Защищаться ",
                                            "[S] - Попытаться сбежать",
                                            "[1-6] - Инвентарь",
                                        });
                                        Fight(true);
                                    }
                                }
                                if (Lab.Map[x - 1][y].Enemy != null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.Attack();
                                        EnemyMoves.Move(false);
                                        Rogue.RAM.Enemy = Lab.Map[x - 1][y].Enemy;
                                        DrawEngine.FightDraw.DrawFight();
                                        Rogue.RAM.Log = new List<string>();
                                        Rogue.RAM.EnemyX = x - 1;
                                        Rogue.RAM.EnemyY = y;
                                        Lab.Map[x - 1][y].Item = Lab.Map[x - 1][y].Enemy.Loot;
                                        Lab.Map[x - 1][y].Enemy = null;
                                        SystemEngine.Helper.Activation.StartBattle();
                                        DrawEngine.CharMap.DrawCMap(new List<string>() 
                                        { 
                                            "[Q] - "+Rogue.RAM.Player.Ability[0].Name,
                                            "[W] - "+Rogue.RAM.Player.Ability[1].Name, 
                                            "[E] - "+Rogue.RAM.Player.Ability[2].Name,
                                            "[R] - "+Rogue.RAM.Player.Ability[3].Name,
                                            "[A] - Удар рукой",
                                            "[D] - Защищаться ",
                                            "[S] - Попытаться сбежать",
                                            "[1-6] - Инвентарь",
                                        });
                                        Fight(true);
                                    }
                                }
                                if (Lab.Map[x][y + 1].Enemy != null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.Attack();
                                        EnemyMoves.Move(false);
                                        Rogue.RAM.Enemy = Lab.Map[x][y + 1].Enemy;
                                        DrawEngine.FightDraw.DrawFight();
                                        Rogue.RAM.Log = new List<string>();
                                        Rogue.RAM.EnemyX = x;
                                        Rogue.RAM.EnemyY = y + 1;
                                        Lab.Map[x][y + 1].Item = Lab.Map[x][y + 1].Enemy.Loot;
                                        Lab.Map[x][y + 1].Enemy = null;
                                        SystemEngine.Helper.Activation.StartBattle();
                                        DrawEngine.CharMap.DrawCMap(new List<string>() 
                                        { 
                                            "[Q] - "+Rogue.RAM.Player.Ability[0].Name,
                                            "[W] - "+Rogue.RAM.Player.Ability[1].Name, 
                                            "[E] - "+Rogue.RAM.Player.Ability[2].Name,
                                            "[R] - "+Rogue.RAM.Player.Ability[3].Name,
                                            "[A] - Удар рукой",
                                            "[D] - Защищаться ",
                                            "[S] - Попытаться сбежать",
                                            "[1-6] - Инвентарь",
                                        });
                                        Fight(true);
                                    }
                                }
                                if (Lab.Map[x][y - 1].Enemy != null)
                                {
                                    if (open == false)
                                    {
                                        SoundEngine.Sound.Attack();
                                        EnemyMoves.Move(false);
                                        Rogue.RAM.Enemy = Lab.Map[x][y - 1].Enemy;
                                        DrawEngine.FightDraw.DrawFight();
                                        Rogue.RAM.Log = new List<string>();
                                        Rogue.RAM.EnemyX = x;
                                        Rogue.RAM.EnemyY = y - 1;
                                        Lab.Map[x][y - 1].Item = Lab.Map[x][y - 1].Enemy.Loot;
                                        Lab.Map[x][y - 1].Enemy = null;
                                        SystemEngine.Helper.Activation.StartBattle();
                                        DrawEngine.CharMap.DrawCMap(new List<string>() 
                                        { 
                                            "[Q] - "+Rogue.RAM.Player.Ability[0].Name,
                                            "[W] - "+Rogue.RAM.Player.Ability[1].Name, 
                                            "[E] - "+Rogue.RAM.Player.Ability[2].Name,
                                            "[R] - "+Rogue.RAM.Player.Ability[3].Name,
                                            "[A] - Удар рукой",
                                            "[D] - Защищаться ",
                                            "[S] - Попытаться сбежать",
                                            "[1-6] - Инвентарь",
                                        });
                                        Fight(true);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            public static void EnemyAttack(int x, int y)
            {
                Rogue.RAM.Enemy = Rogue.RAM.Map.Map[x][y].Enemy;
                DrawEngine.FightDraw.DrawFight();
                Rogue.RAM.Log = new List<string>();
                Rogue.RAM.EnemyX = x;
                Rogue.RAM.EnemyY = y;
                Rogue.RAM.Map.Map[x][y].Item = Rogue.RAM.Map.Map[x][y].Enemy.Loot;
                Rogue.RAM.Map.Map[x][y].Enemy = null;
                SystemEngine.Helper.Activation.StartBattle();
                DrawEngine.CharMap.DrawCMap(new List<string>() 
                                        { 
                                            "[Q] - "+Rogue.RAM.Player.Ability[0].Name,
                                            "[W] - "+Rogue.RAM.Player.Ability[1].Name, 
                                            "[E] - "+Rogue.RAM.Player.Ability[2].Name,
                                            "[R] - "+Rogue.RAM.Player.Ability[3].Name,
                                            "[A] - Удар рукой",
                                            "[D] - Защищаться ",
                                            "[S] - Попытаться сбежать",
                                            "[1-6] - Инвентарь",
                                        });
                Fight(true);
            }

            private static void EnemyBack()
            {
                Rogue.RAM.Map.Map[Rogue.RAM.EnemyX][Rogue.RAM.EnemyY].Item = null;
                Rogue.RAM.Map.Map[Rogue.RAM.EnemyX][Rogue.RAM.EnemyY].Enemy = Rogue.RAM.Enemy;
            }

            private static void CharInfo()
            {
                DrawEngine.CharacterDraw.DrawInterface();
                Character();
            }

            //FIGHT

            public static void Strike(bool education=false)
            {
                if (Rogue.RAM.Enemy != null)
                {
                    if (Rogue.RAM.Enemy.CHP > 0)
                    {
                        int attacks = 1;
                        if (Rogue.RAM.Player.Class == MechEngine.BattleClass.LightWarrior)
                        {
                            attacks += Rogue.RAM.Player.Ability[3].Power;                            
                        }
                        for (int i = 0; i < attacks; i++)
                        {
                            SoundEngine.Sound.Strike();

                            var Mob = Rogue.RAM.Enemy;
                            var Play = Rogue.RAM.Player;
                            int dmg = (r.Next(Play.MIDMG, Play.MADMG + 1) + Convert.ToInt32(0.25 * Play.AD));
                            #region Lighting Mark
                            bool yesmark = false; foreach (MechEngine.Ability a in Rogue.RAM.Enemy.DoT) { if (a.Name == DataBase.EliteAbilityBase.LightMark.Name) { yesmark = true; } } if (yesmark) { dmg += Rogue.RAM.Player.Ability[0].Power; }
                            #endregion
                            if (Mob.ARM != 0) { dmg = dmg - Convert.ToInt32(Mob.ARM * 0.75); }
                            if (dmg < 0) { dmg = 0; }
                            Rogue.RAM.Enemy.CHP -= dmg;
                            Rogue.RAM.Log.Add(Rogue.RAM.Player.Name + " наносит " + dmg.ToString() + " урона " + Rogue.RAM.Enemy.Name + "!");
                            DrawEngine.FightDraw.ReDrawCombatLog();


                            if (Rogue.RAM.Player.Class == MechEngine.BattleClass.Warrior)
                            {
                                int additionalrage = 0;
                                foreach (SystemEngine.ArmorSet aSet in Rogue.RAM.ArmSet)
                                {
                                    if (aSet.Name == "Hodar" && aSet.Active)
                                    {
                                        additionalrage += 10;
                                    }
                                }
                                Rogue.RAM.Player.CMP += (additionalrage + (5 + Rogue.RAM.Player.Rage));
                            }
                        }
                    }
                    if (Rogue.RAM.Enemy.CHP <= 0)
                    {
                        if (Rogue.RAM.EducationFight)
                        {
                            Rogue.RAM.EducationFight = false;
                            return;
                        }
                        else
                        {
                            Rogue.RAM.Player.Kill();
                        }
                    }
                    else
                    {
                        EnemyAttack();
                        //DrawEngine.FightDraw.DrawEnemyStat();
                        //DrawEngine.GUIDraw.ReDrawCharStat();
                    }
                }
            }

            private static void Defense()
            {
                if (Rogue.RAM.Enemy != null)
                {
                    if (Rogue.RAM.Enemy.CHP > 0)
                    {
                        Rogue.RAM.Log.Add("Вы принимаете защитную стойку!");
                        DrawEngine.GUIDraw.ReDrawCharStat();
                        
                        var Mob = Rogue.RAM.Enemy;
                        int ra = r.Next(2);
                        if (Mob.Cast[ra] != null && ra != 2)
                        {
                            Rogue.RAM.Log.Add(Mob.Cast[ra].Activate);
                            DrawEngine.FightDraw.DrawEnemyStat();
                            DrawEngine.FightDraw.ReDrawCombatLog();
                            DrawEngine.GUIDraw.ReDrawCharStat();
                            if (Rogue.RAM.Player.CHP <= 0)
                            {
                                Rogue.RAM.Log.Add("Вы проигрываете битву...");
                                DrawEngine.FightDraw.ReDrawCombatLog();
                                Thread.Sleep(2000);
                                DrawEngine.SplashScreen.EndOfGame();                                
                                Rogue.Main(new string[0]);
                            }
                        }
                        else
                        {
                            int dmg = (r.Next(Mob.MIDMG, Mob.MADMG + 1) + Mob.AD / 2);
                            if (Mob.ARM != 0) { dmg = dmg - Convert.ToInt32(Rogue.RAM.Player.ARM * 0.5); }
                            if (dmg < 0) { dmg = 0; }
                            Rogue.RAM.Player.CHP -= Math.Abs(dmg);
                            Rogue.RAM.Log.Add(Rogue.RAM.Player.Name + " получает " + Math.Abs(dmg).ToString() + " урона!");
                            DrawEngine.FightDraw.DrawEnemyStat();
                            DrawEngine.GUIDraw.ReDrawCharStat();
                            DrawEngine.FightDraw.ReDrawCombatLog();
                            if (Rogue.RAM.Player.CHP <= 0)
                            {
                                Rogue.RAM.Log.Add("Вы проигрываете битву...");
                                DrawEngine.FightDraw.ReDrawCombatLog();
                                Thread.Sleep(2000);
                                DrawEngine.SplashScreen.EndOfGame();                                
                                Rogue.Main(new string[0]);
                            }
                        }
                    }
                    else { Strike(); }
                }
            }

            private static void Stealth()
            {
                if (Rogue.RAM.Enemy.CHP > 0)
                {
                    Rogue.RAM.Log.Add("Вы пытаетесь тихо сбежать...");
                    DrawEngine.GUIDraw.ReDrawCharStat();
                    
                    if (r.Next(100) > 15 + Rogue.RAM.Player.AP)
                    {
                        var Mob = Rogue.RAM.Enemy;
                        int ra = r.Next(3);
                        if (ra != 2 && Mob.Cast[ra] != null)
                        {
                            Rogue.RAM.Log.Add(Mob.Cast[ra].Activate);
                            DrawEngine.FightDraw.DrawEnemyStat();
                            DrawEngine.FightDraw.ReDrawCombatLog();
                            DrawEngine.GUIDraw.ReDrawCharStat();
                            if (Rogue.RAM.Player.CHP <= 0)
                            {
                                Rogue.RAM.Log.Add("Вы проигрываете битву...");
                                DrawEngine.FightDraw.ReDrawCombatLog();
                                Thread.Sleep(2000);
                                DrawEngine.SplashScreen.EndOfGame();
                                Rogue.Main(new string[0]);
                            }
                        }
                        else
                        {
                            int dmg = (r.Next(Mob.MIDMG, Mob.MADMG + 1) + Mob.AD);
                            if (Mob.ARM != 0) { dmg = dmg - Convert.ToInt32(Rogue.RAM.Player.ARM * 0.2); }
                            if (dmg < 0) { dmg = 0; }
                            Rogue.RAM.Player.CHP -= Math.Abs(dmg);
                            Rogue.RAM.Log.Add(Rogue.RAM.Player.Name + " получает " + dmg.ToString() + " урона!");
                            DrawEngine.FightDraw.DrawEnemyStat();
                            DrawEngine.GUIDraw.ReDrawCharStat();
                            DrawEngine.FightDraw.ReDrawCombatLog();
                        }
                        if (Rogue.RAM.Player.CHP <= 0)
                        {
                            Rogue.RAM.Log.Add("Вы проигрываете битву...");
                            DrawEngine.FightDraw.ReDrawCombatLog();
                            Thread.Sleep(2000);
                            DrawEngine.SplashScreen.EndOfGame();
                            Rogue.Main(new string[0]);
                        }
                    }
                    else
                    {
                        Rogue.RAM.Player.RemoteMonsterAbil();
                        Rogue.RAM.Log.Add("Вам удалось сбежать!");
                        DrawEngine.FightDraw.DrawEnemyStat();
                        DrawEngine.GUIDraw.ReDrawCharStat();
                        DrawEngine.FightDraw.ReDrawCombatLog();
                        EnemyBack();
                        Thread.Sleep(1000);
                        Rogue.RAM.Enemy = null;
                        DrawEngine.GUIDraw.DrawLab();
                        EnemyMoves.Move(true);
                        PlayEngine.GamePlay.Play();
                    }
                }
                else { Strike(); }
            }

            public static void EnemyAttack()
            {
                
                var Mob = Rogue.RAM.Enemy;
                int ra = r.Next(3);
                if (ra != 2 && Mob.Cast[ra] != null)
                {
                    Rogue.RAM.Log.Add(Mob.Cast[ra].Activate);
                    DrawEngine.FightDraw.DrawEnemyStat();
                    DrawEngine.FightDraw.ReDrawCombatLog();
                    DrawEngine.GUIDraw.ReDrawCharStat();
                    if (Rogue.RAM.Player.CHP <= 0)
                    {
                        Rogue.RAM.Log.Add("Вы проигрываете битву...");
                        DrawEngine.FightDraw.ReDrawCombatLog();
                        Thread.Sleep(2000);
                        DrawEngine.SplashScreen.EndOfGame();                        
                    }

                }
                else
                {
                    int dmg = (r.Next(Mob.MIDMG, Mob.MADMG + 1) + Mob.AD / 2);
                    dmg = checkdmg(dmg);
                    Rogue.RAM.Player.CHP -= dmg;
                    Rogue.RAM.Log.Add(Rogue.RAM.Enemy.Name + " наносит " + dmg.ToString() + " урона " + Rogue.RAM.Player.Name + "!");
                    //Rogue.RAM.Log.Add(Rogue.RAM.Player.Name + " получает " + dmg.ToString() + " урона!");
                    DrawEngine.FightDraw.DrawEnemyStat();
                    DrawEngine.GUIDraw.ReDrawCharStat();
                    DrawEngine.FightDraw.ReDrawCombatLog();
                }
                if (Rogue.RAM.Player.CHP <= 0)
                {
                    Rogue.RAM.Log.Add("Вы проигрываете битву...");
                    DrawEngine.FightDraw.ReDrawCombatLog();
                    Thread.Sleep(2000);
                    DrawEngine.SplashScreen.EndOfGame();
                    Rogue.Main(new string[0]);
                }
            }

            private static int checkdmg(int d)
            {
                int dd = 0;
                if (Rogue.RAM.Player.ARM != 0) { dd = d - Convert.ToInt32(Rogue.RAM.Player.ARM * 0.75); }
                if (dd < 0)
                {
                    dd = 0;
                }
                return dd;
            }
            /// <summary>
            /// GUI ability
            /// </summary>
            public static class GUIa
            {
                /// <summary>
                /// Draw banish pop ups
                /// </summary>
                public static void Banish()
                {
                    PlayEngine.EnemyMoves.Move(false);
                    Rogue.RAM.PopUpTab.NowTab = 0;
                    DrawEngine.PopUpWindowDraw.Banishment_pt1();                    
                    aligmentBanish();
                }
                /// <summary>
                /// gui aligment banish
                /// </summary>
                private static void aligmentBanish()
                {
                    ConsoleKey push = Console.ReadKey(true).Key;
                    switch (push)
                    {
                        case ConsoleKey.Enter:
                            {
                                if (Rogue.RAM.PopUpTab.NowTab == 0) { Rogue.RAM.Player.Ability[Rogue.RAM.PopUpTab.MaxTab].GoodEvilAbility = true; }
                                else { Rogue.RAM.Player.Ability[Rogue.RAM.PopUpTab.MaxTab].GoodEvilAbility = false; }
                                DrawEngine.PopUpWindowDraw.Banishment_pt2();
                                directionBanish();
                                break;
                            }
                        case ConsoleKey.UpArrow:
                            {
                                DrawEngine.PopUpWindowDraw.ReDrawPopUp(SystemEngine.PopUpWindow.V1, SystemEngine.ArrowDirection.Top, MechEngine.BattleClass.Inquisitor);
                                aligmentBanish();
                                break;
                            }
                        case ConsoleKey.DownArrow:
                            {
                                DrawEngine.PopUpWindowDraw.ReDrawPopUp(SystemEngine.PopUpWindow.V1, SystemEngine.ArrowDirection.Bot, MechEngine.BattleClass.Inquisitor);
                                aligmentBanish();
                                break;
                            }                        
                        case ConsoleKey.Escape:
                            {
                                EnemyMoves.Move(true);
                                DrawEngine.InfoWindow.Custom("Действие отменено");
                                Play();
                                break;
                            }
                        default:
                            {
                                aligmentBanish();
                                break;
                            }
                    }
                }
                /// <summary>
                /// gui direction banish
                /// </summary>
                private static void directionBanish()
                {
                    ConsoleKey push = Console.ReadKey(true).Key;
                    switch (push)
                    {
                        case ConsoleKey.Enter:
                            {
                                switch (Rogue.RAM.PopUpTab.NowTab)
                                {
                                    case 1: { Rogue.RAM.Player.Ability[Rogue.RAM.PopUpTab.MaxTab].Direction = SystemEngine.ArrowDirection.Top; break; }
                                    case 2: { Rogue.RAM.Player.Ability[Rogue.RAM.PopUpTab.MaxTab].Direction = SystemEngine.ArrowDirection.Left; break; }
                                    case 3: { Rogue.RAM.Player.Ability[Rogue.RAM.PopUpTab.MaxTab].Direction = SystemEngine.ArrowDirection.Right; break; }
                                    case 4: { Rogue.RAM.Player.Ability[Rogue.RAM.PopUpTab.MaxTab].Direction = SystemEngine.ArrowDirection.Bot; break; }
                                }
                                DrawEngine.GUIDraw.DrawLab();
                                break;
                            }
                        case ConsoleKey.UpArrow:
                            {
                                DrawEngine.PopUpWindowDraw.ReDrawPopUp(SystemEngine.PopUpWindow.VA, SystemEngine.ArrowDirection.Top, MechEngine.BattleClass.Inquisitor);
                                directionBanish();
                                break;
                            }
                        case ConsoleKey.DownArrow:
                            {
                                DrawEngine.PopUpWindowDraw.ReDrawPopUp(SystemEngine.PopUpWindow.VA, SystemEngine.ArrowDirection.Bot, MechEngine.BattleClass.Inquisitor);
                                directionBanish();
                                break;
                            }
                        case ConsoleKey.LeftArrow:
                            {
                                DrawEngine.PopUpWindowDraw.ReDrawPopUp(SystemEngine.PopUpWindow.VA, SystemEngine.ArrowDirection.Left, MechEngine.BattleClass.Inquisitor);
                                directionBanish();
                                break;
                            }
                        case ConsoleKey.RightArrow:
                            {
                                DrawEngine.PopUpWindowDraw.ReDrawPopUp(SystemEngine.PopUpWindow.VA, SystemEngine.ArrowDirection.Right, MechEngine.BattleClass.Inquisitor);
                                directionBanish();
                                break;
                            }
                        case ConsoleKey.Escape:
                            {
                                EnemyMoves.Move(true);
                                DrawEngine.InfoWindow.Custom("Действие отменено");
                                Play();
                                break;
                            }
                        default:
                            {
                                directionBanish();
                                break;
                            }
                    }
                }
                /// <summary>
                /// Trap gui menu
                /// </summary>
                public static void Trap() 
                {
                    PlayEngine.EnemyMoves.Move(false);
                    Rogue.RAM.PopUpTab.NowTab = 0;
                    DrawEngine.PopUpWindowDraw.Trap_pt1();
                    typeTrap();
                }
                /// <summary>
                /// gui trap type banish
                /// </summary>
                private static void typeTrap()
                {
                    ConsoleKey push = Console.ReadKey(true).Key;
                    switch (push)
                    {
                        case ConsoleKey.Enter:
                            {
                                switch (Rogue.RAM.PopUpTab.NowTab)
                                {
                                    case 0: { Rogue.RAM.Player.Ability[Rogue.RAM.PopUpTab.MaxTab].TypeOfTrap = 0; break; }
                                    case 1: { Rogue.RAM.Player.Ability[Rogue.RAM.PopUpTab.MaxTab].TypeOfTrap = 1; break; }
                                    case 2: { Rogue.RAM.Player.Ability[Rogue.RAM.PopUpTab.MaxTab].TypeOfTrap = 2; break; }
                                    case 3: { Rogue.RAM.Player.Ability[Rogue.RAM.PopUpTab.MaxTab].TypeOfTrap = 3; break; }
                                    default: { Rogue.RAM.Player.Ability[Rogue.RAM.PopUpTab.MaxTab].TypeOfTrap = 0; break; }
                                }

                                
                                DrawEngine.PopUpWindowDraw.Banishment_pt2();
                                directionBanish();
                                break;
                            }
                        case ConsoleKey.UpArrow:
                            {
                                DrawEngine.PopUpWindowDraw.ReDrawPopUp(SystemEngine.PopUpWindow.V1, SystemEngine.ArrowDirection.Top, MechEngine.BattleClass.Assassin);
                                typeTrap();
                                break;
                            }
                        case ConsoleKey.DownArrow:
                            {
                                DrawEngine.PopUpWindowDraw.ReDrawPopUp(SystemEngine.PopUpWindow.V1, SystemEngine.ArrowDirection.Bot, MechEngine.BattleClass.Assassin);
                                typeTrap();
                                break;
                            }
                        case ConsoleKey.Escape:
                            {
                                EnemyMoves.Move(true);
                                DrawEngine.InfoWindow.Custom("Действие отменено");
                                Play();
                                break;
                            }
                        default:
                            {
                                typeTrap();
                                break;
                            }
                    }
                }
            }
        }

        public static class GateKeeperGamePlay
        {
            public static bool Main(MechEngine.CapitalDoor.GateKeeper gk)
            {
                ConsoleKey push = Console.ReadKey(true).Key;
                switch (push)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        {
                            gk.Location = 0;
                            break;
                        }
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        {
                            gk.Location = 1;
                            break;
                        }
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        {
                            gk.Location = 2;
                            break;
                        }
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        {
                            gk.Location = 3;
                            break;
                        }
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        {
                            gk.Location = 4;
                            break;
                        }
                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        {
                            gk.Location = 5;
                            break;
                        }
                    case ConsoleKey.D7:
                    case ConsoleKey.NumPad7:
                        {
                            gk.Location = 6;
                            break;
                        }
                    default: { DrawEngine.InfoWindow.Message = "Вы закончили разговор с " + gk.Name; return false; }
                }
                return true;
            }
        }
    }
}
