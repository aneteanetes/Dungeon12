using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.Globalization;

namespace Rogue
{
    class Rogue
    {
        #region window

        const int SWP_NOSIZE = 0x0001;

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        private static IntPtr MyConsole = GetConsoleWindow();

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("coredll.dll")]
        public static extern int GetSystemMetrics(SYSTEM_METRICS nIndex);

        public enum SYSTEM_METRICS : int
        {
            SM_CXSCREEN = 0,
            SM_CYSCREEN = 1,
            SM_CXVSCROLL = 2,
            SM_CYHSCROLL = 3,
            SM_CYCAPTION = 4,
            SM_CXBORDER = 5,
            SM_CYBORDER = 6,
            SM_CXDLGFRAME = 7,
            SM_CYDLGFRAME = 8,
            SM_CXICON = 11,
            SM_CYICON = 12,
            SM_CYMENU = 15,
            SM_CXFULLSCREEN = 16,
            SM_CYFULLSCREEN = 17,
            SM_MOUSEPRESENT = 19,
            SM_CYVSCROLL = 20,
            SM_CXHSCROLL = 21,
            SM_DEBUG = 22,
            SM_CXDOUBLECLK = 36,
            SM_CYDOUBLECLK = 37,
            SM_CXICONSPACING = 38,
            SM_CYICONSPACING = 39,
            SM_CXEDGE = 45,
            SM_CYEDGE = 46,
            SM_CXSMICON = 49,
            SM_CYSMICON = 50,
            SM_XVIRTUALSCREEN = 76,
            SM_YVIRTUALSCREEN = 77,
            SM_CXVIRTUALSCREEN = 78,
            SM_CYVIRTUALSCREEN = 79,
            SM_CMONITORS = 80,
            SM_SAMEDISPLAYFORMAT = 81,
            SM_CXFIXEDFRAME = 7,
            SM_CYFIXEDFRAME = 8
        }

        #endregion

        public static void Main(string[] args)
        {
            #region Initialize
            try
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Music");
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Sound");
            }
            catch { }
            #endregion

            Console.ResetColor();

            Console.Title = "Dungeon 12 - v0.6.0.2: You are not alone";

            #region Desktop

            Console.SetWindowSize(100, 34);
            Console.SetBufferSize(100, 34);
            Console.CursorVisible = false;

            Size q = SystemInformation.PrimaryMonitorSize;
            int xpos = q.Width / 4;
            int ypos = q.Height / 4;
            SetWindowPos(MyConsole, 0, xpos, ypos, 0, 0, SWP_NOSIZE);

            #endregion

            #region Random Access Memory

            Rogue.RAM = new RAMC();
            RAM.Log = new List<string>();
            RAM.Step = new MechEngine.StepManager();
            RAM.ColorSet = new SystemEngine.ColorSettings();
            RAM.SoundSet = new SystemEngine.SoundSettings() { Volume = 10 };
            RAM.CUIColor = ConsoleColor.DarkGreen;
            RAM.GraphHeroSet = new SystemEngine.GraphicHeroSettings();
            RAM.SummonedList = new List<MechEngine.Summoned>();
            RAM.MerchTab = new MechEngine.InventoryTab();
            RAM.Qtab = new MechEngine.InventoryTab();
            RAM.Merch = new MechEngine.Merchant();
            //RAM.qGiver = new MechEngine.Questgiver();

            Rogue.RAM.Map = new MechEngine.Labirinth();
            Rogue.RAM.Map.Level = 1;

            RAM.Timers = new List<SystemEngine.Timers>() {
                new SystemEngine.Timers(){Activated=false,Type=SystemEngine.TimerType.Que},
                new SystemEngine.Timers(){Activated=false,Type= SystemEngine.TimerType.Deb},
                new SystemEngine.Timers(){Activated=false,Type= SystemEngine.TimerType.Buf},
                new SystemEngine.Timers(){Activated=false,Type= SystemEngine.TimerType.DoT},
                new SystemEngine.Timers(){Activated=false,Type= SystemEngine.TimerType.HoT},
                new SystemEngine.Timers(){Activated=false,Type= SystemEngine.TimerType.Smd},
                new SystemEngine.Timers(){Activated=false,Type= SystemEngine.TimerType.Sum},
                new SystemEngine.Timers(){Activated=false,Type= SystemEngine.TimerType.Pro}
            };

            RAM.Judge = new SystemEngine.Judge();
            RAM.ColorSet.DeadEyesColors = "true";

            #endregion

            #region Addon
            Rogue.RAM.AddonName = "[You are not alone]";
            #endregion

            Console.Clear();
                       

            SoundEngine.Player.SoundVolume = (float)0.4;

            if (Debugger.IsAttached)
            {
                //DrawEngine.PopUpWindowDraw.SwitchCraftAbilWindow(2, new List<MechEngine.Ability>());
                //Console.Read(); Console.Read(); Console.Read(); Console.Read();
                //tests.FindMyLoverColors.Print = 0;
                //Console.Read(); Console.Read(); Console.Read(); Console.Read();
                //GenEngine.MobGen.TestGen(1);
                //tests.Inception.Deeper();                
                //tests.Magician.Daze();
                //Console.Read(); Console.Read(); Console.Read(); Console.Read();
                //SoundEngine.Boom();                
                //Windows.Example(true,true,true,true,2);
                //ConsoleWindows.Example(true,true,true,true,0);
                //tests.bufferconsole.f();
                //Console.Out.Write(
                //DrawEngine.SplashScreen.DeamonEnd();
                //Console.Read(); Console.Read(); Console.Read(); Console.Read();
                //ConsoleWindows.Window.DRAW_MAIN_VOID(new List<List<ConsoleWindows.ColouredChar>>() { new List<ConsoleWindows.ColouredChar>() { new ConsoleWindows.ColouredChar() { Char = '(', Color = 14, BackColor = 0 }, new ConsoleWindows.ColouredChar() { Char = '@', Color = 12, BackColor = 0 }, new ConsoleWindows.ColouredChar() { Char = ')', Color = 14, BackColor = 0 } } }, 0, 0, 3, 1);
            }

            //if (Debugger.IsAttached)
            //{
                if (args.Length == 0)
                {
                    //DrawEngine.SplashScreen.CompanyLogo();
                    SoundEngine.Music.MainTheme();
                    //DrawEngine.SplashScreen.StartGame();
                }
            //}  
                MenuEngine.PressAnyKey.Draw();
            MenuEngine.MainMenu.Draw();

            //Console.BackgroundColor = ConsoleColor.White;
            //Console.ForegroundColor = ConsoleColor.Black;
            //DrawEngine.ConsoleDraw.WriteTitle("Добро пожаловать в Dungeon 12!\nНажмите любую клавишу для продолжения...");
            //Console.ReadKey(true);
            try
            {
                //bool bone = false;
                //while (!bone) { bone = PlayEngine.Menu.MainMenu; }
                DrawEngine.GUIDraw.DrawGUI();
                if (!Rogue.RAM.History)
                {
                    LabirinthEngine.Create(1, true);
                    SoundEngine.Music.TownTheme();
                }
                else
                {
                    LabirinthEngine.Create(1, true, 0, true);
                    SoundEngine.Music.SilentTheme();
                }
                PlayEngine.EnemyMoves.Move(true);
                PlayEngine.GamePlay.Play();
            }
            catch (Exception ex)
            {
                try
                {
                    SystemEngine.BugReport Bug = new SystemEngine.BugReport();
                    Bug.Author = Environment.UserName;
                    Bug.Category = "Unknown";
                    Bug.Title = "AUTO";
                    Bug.Text = ex.Message + "\r\n\r\n\r\n" + ex.StackTrace;
                    SystemEngine.Helper.Net.SendBugReport(Bug);
                }
                catch { Console.Title = "Вы поймали багованный баг!"; }
                DrawEngine.SplashScreen.Bug();
            }
        }

        public static RAMC RAM;
    }
}
