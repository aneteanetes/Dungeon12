using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue
{
    public static class MenuEngine
    {
        public static class MainMenu
        {
            public static void Draw()
            {
                DrawEngine.SplashScreen.MainScreen2();
                DrawEngine.SplashScreen.MainScreen();

                ConsoleWindows.Window w = new ConsoleWindows.Window();
                w.Animated = true;
                w.Animation = ConsoleWindows.Additional.HorizontalAnimation;
                w.Animation.Frames = 2;
                w.Speed = 10;
                w.Border = ConsoleWindows.Additional.BoldBorder;                
                w.BorderColor = ConsoleColor.DarkGray;
                //w.Border.HorizontalLine = '░';
                //w.Border.VerticalLine = '⌠';
                w.Border.LowerLeftCorner='@';
                w.Border.LowerRightCorner = '@';
                w.Border.UpperLeftCorner = '@';
                w.Border.UpperRightCorner = '@';
                w.Border.PerpendicularRightward = '@';
                w.Border.PerpendicularLeftward = '@';
                
                w.Header = true;
                w.Height = 24;
                w.Width = 26;
                w.Left = 36;
                w.Top = 5;

                ConsoleWindows.Text t = new ConsoleWindows.Text(w);
                t.BackgroundColor = ConsoleColor.Black;
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.DarkCyan;
                t.Write("Dungeon");
                t.ForegroundColor = ConsoleColor.Red;
                t.Write(" 12");
                t.AppendLine();
                t.TextPosition = ConsoleWindows.TextPosition.Right;
                t.ForegroundColor = ConsoleColor.Red;
                t.WriteLine("Альфа");
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.Magenta;
                t.Write("[You are not alone]");
                t.AppendLine();

                w.Text = t;

                //Controls 
                ConsoleWindows.Button bng = new ConsoleWindows.Button(w);
                bng.Top = 6;
                bng.Left = 3;
                bng.Width = 20;
                bng.Height = 6;
                bng.ActiveColor = ConsoleColor.Red;
                bng.InactiveColor = ConsoleColor.DarkRed;
                bng.CloseAfterUse = true;
                bng.Label = "Новая игра";
                bng.OnClick = () =>
                    {
                        DrawEngine.ConsoleDraw.WriteTitle("Начало новой игры...\n \n Нажмите любую клавишу для продолжения...");
                        PlayEngine.GamePlay.NewGame.CharacterCreation();
                    };
                w.AddControl(bng);

                ConsoleWindows.Button bfg = new ConsoleWindows.Button(w);
                bfg.Top = 9;
                bfg.Left = 3;
                bfg.Width = 20;
                bfg.Height = 6;
                bfg.ActiveColor = ConsoleColor.Red;
                bfg.InactiveColor = ConsoleColor.DarkRed;
                bfg.Label = "Быстрая игра";
                bfg.CloseAfterUse = true;
                bfg.OnClick = () => { PlayEngine.GamePlay.NewGame.CharacterCreation(true); };
                w.AddControl(bfg);

                ConsoleWindows.Button ba = new ConsoleWindows.Button(w);
                ba.Top = 12;
                ba.Left = 3;
                ba.Width = 20;
                ba.Height = 6;
                ba.ActiveColor = ConsoleColor.Red;
                ba.InactiveColor = ConsoleColor.DarkRed;
                ba.Label = "Создатели";
                ba.CloseAfterUse = true;
                ba.OnClick = () => { MenuEngine.CreditsWindow.Draw(); MenuEngine.MainMenu.Draw(); };
                w.AddControl(ba);

                ConsoleWindows.Button bex = new ConsoleWindows.Button(w);
                bex.Top = 15;
                bex.Left = 3;
                bex.Width = 20;
                bex.Height = 6;
                bex.ActiveColor = ConsoleColor.Red;
                bex.InactiveColor = ConsoleColor.DarkRed;
                bex.Label = "Выход";
                bex.CloseAfterUse = true;
                bex.OnClick = () =>
                {
                    Environment.Exit(0);
                };
                w.AddControl(bex);

                w.Draw();
            }
        }

        public static class EscapeMenu
        {
            public static void Draw()
            {
                DrawEngine.SplashScreen.MainScreen2();
                DrawEngine.SplashScreen.MainScreen();

                ConsoleWindows.Window w = new ConsoleWindows.Window();
                w.Animated = true;
                w.Animation = ConsoleWindows.Additional.HorizontalAnimation;
                w.Animation.Frames = 2;
                w.Speed = 10;
                w.Border = ConsoleWindows.Additional.BoldBorder;
                w.BorderColor = ConsoleColor.DarkGray;
                //w.Border.HorizontalLine = '░';
                //w.Border.VerticalLine = '⌠';
                w.Border.LowerLeftCorner = '@';
                w.Border.LowerRightCorner = '@';
                w.Border.UpperLeftCorner = '@';
                w.Border.UpperRightCorner = '@';
                w.Border.PerpendicularRightward = '@';
                w.Border.PerpendicularLeftward = '@';

                w.Header = true;
                w.Height = 24;
                w.Width = 26;
                w.Left = 36;
                w.Top = 5;

                ConsoleWindows.Text t = new ConsoleWindows.Text(w);
                t.BackgroundColor = ConsoleColor.Black;
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.DarkCyan;
                t.Write("Dungeon");
                t.ForegroundColor = ConsoleColor.Red;
                t.Write(" 12");
                t.AppendLine();
                t.TextPosition = ConsoleWindows.TextPosition.Right;
                t.ForegroundColor = ConsoleColor.Red;
                t.WriteLine("Альфа");
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.Magenta;
                t.Write("[You are not alone]");
                t.AppendLine();

                w.Text = t;

                //Controls 
                ConsoleWindows.Button bng = new ConsoleWindows.Button(w);
                bng.Top = 6;
                bng.Left = 3;
                bng.Width = 20;
                bng.Height = 6;
                bng.ActiveColor = ConsoleColor.Red;
                bng.InactiveColor = ConsoleColor.DarkRed;
                bng.CloseAfterUse = true;
                bng.Label = "Вернуться в игру";
                bng.OnClick = () =>
                {
                    if (Rogue.RAM.Map.Name.IndexOf("Мраумир") != -1) { SoundEngine.Music.TownTheme(); }
                    else
                    { SoundEngine.Music.DungeonTheme(); }
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.Black;
                    DrawEngine.GUIDraw.DrawGUI();
                    DrawEngine.GUIDraw.drawstat();
                };
                w.AddControl(bng);

                ConsoleWindows.Button bfg = new ConsoleWindows.Button(w);
                bfg.Top = 9;
                bfg.Left = 3;
                bfg.Width = 20;
                bfg.Height = 6;
                bfg.ActiveColor = ConsoleColor.Red;
                bfg.InactiveColor = ConsoleColor.DarkRed;
                bfg.Label = "Главное меню";
                bfg.CloseAfterUse = true;
                bfg.OnClick = () => { SoundEngine.Music.MainTheme(); Rogue.Main(new string[] { "no" }); };
                w.AddControl(bfg);

                ConsoleWindows.Button ba = new ConsoleWindows.Button(w);
                ba.Top = 12;
                ba.Left = 3;
                ba.Width = 20;
                ba.Height = 6;
                ba.ActiveColor = ConsoleColor.Red;
                ba.InactiveColor = ConsoleColor.DarkRed;
                ba.Label = "Глобальный лог";
                ba.CloseAfterUse = true;
                ba.OnClick = () => { PlayEngine.InGameSettings.Keymap(); };
                w.AddControl(ba);

                ConsoleWindows.Button bex = new ConsoleWindows.Button(w);
                bex.Top = 15;
                bex.Left = 3;
                bex.Width = 20;
                bex.Height = 6;
                bex.ActiveColor = ConsoleColor.Red;
                bex.InactiveColor = ConsoleColor.DarkRed;
                bex.Label = "Выход";
                bex.CloseAfterUse = true;
                bex.OnClick = () =>
                {
                    Environment.Exit(0);
                };
                w.AddControl(bex);

                w.Draw();
            }
        }

        public static class InfoWindow
        {
            public static void Draw()
            {
                DrawEngine.SplashScreen.MainScreen2();
                DrawEngine.SplashScreen.MainScreen();

                ConsoleWindows.Window w = new ConsoleWindows.Window();
                w.Animated = true;
                w.Animation = ConsoleWindows.Additional.StadartAnimation;
                w.Animation.Frames = 2;
                w.Speed = 10;
                w.Border = ConsoleWindows.Additional.BoldBorder;
                w.BorderColor = ConsoleColor.DarkGray;
                //w.Border.HorizontalLine = '░';
                //w.Border.VerticalLine = '⌠';
                w.Border.LowerLeftCorner = '@';
                w.Border.LowerRightCorner = '@';
                w.Border.UpperLeftCorner = '@';
                w.Border.UpperRightCorner = '@';
                w.Border.PerpendicularRightward = '@';
                w.Border.PerpendicularLeftward = '@';

                w.Header = true;
                w.Height = 29;
                w.Width = 24;
                w.Left = 37;
                w.Top = 4;

                ConsoleWindows.Text t = new ConsoleWindows.Text(w);
                t.BackgroundColor = ConsoleColor.Black;
                t.TextPosition = ConsoleWindows.TextPosition.Left;
                t.ForegroundColor = ConsoleColor.DarkCyan;
                t.Write("Ваш персонаж");
                t.ForegroundColor = ConsoleColor.Red;
                t.Write(" D12");
                t.AppendLine();
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.DarkRed;
                t.Write("Имя: ");
                t.ForegroundColor = ConsoleColor.DarkGray;
                t.Write(Rogue.RAM.Player.Name);
                t.AppendLine();
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.DarkRed;
                t.Write("Раса: ");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write(Rogue.RAM.Player.GetClassRace(1));
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.DarkRed;
                t.Write("Класс: ");
                t.ForegroundColor = SystemEngine.Helper.Information.ClassC;
                t.Write(Rogue.RAM.Player.GetClassRace(2));
                t.AppendLine();
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.DarkRed;
                t.Write("Здоровье: ");
                t.ForegroundColor = ConsoleColor.Red;
                t.Write(string.Format("{0}/{0}", Rogue.RAM.Player.MHP));
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.DarkRed;
                t.Write(Rogue.RAM.Player.ManaName+": ");
                t.ForegroundColor = SystemEngine.Helper.Information.ClassC;
                t.Write(string.Format("{0}/{0}", Rogue.RAM.Player.MMP));
                t.AppendLine();
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.DarkRed;
                t.Write("Урон: ");
                t.ForegroundColor = ConsoleColor.DarkYellow;
                t.Write(string.Format("{0}-{1}", Rogue.RAM.Player.MIDMG, Rogue.RAM.Player.MADMG));
                t.AppendLine();
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.DarkRed;
                t.Write("Сила атаки: " + Rogue.RAM.Player.AD.ToString());
                t.AppendLine();
                t.Write("Сила магии: ");
                t.ForegroundColor = ConsoleColor.DarkCyan;
                t.Write(Rogue.RAM.Player.AP.ToString());
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.DarkRed;
                t.Write("Физ. Защита: ");
                t.ForegroundColor = ConsoleColor.DarkGreen;
                t.Write(Rogue.RAM.Player.ARM.ToString());
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.DarkRed;
                t.Write("Маг. Защита: ");
                t.ForegroundColor = ConsoleColor.DarkMagenta;
                t.Write(Rogue.RAM.Player.AP.ToString());
                t.AppendLine();

                w.Text = t;

                //("Ваш персонаж:\n\nИмя: " + My.Name + "\nРаса:" + My.GetClassRace(1) + "\nКласс: " + My.GetClassRace(2) + "\nУровень: " + My.Level.ToString());

                ConsoleWindows.Button bex = new ConsoleWindows.Button(w);
                bex.Top = 20;
                bex.Left = 2;
                bex.Width = 20;
                bex.Height = 6;
                bex.ActiveColor = ConsoleColor.Red;
                bex.InactiveColor = ConsoleColor.DarkRed;
                bex.Label = "Продолжить";
                bex.CloseAfterUse = true;
                w.AddControl(bex);

                w.Draw();
            }
        }

        public static class CreditsWindow
        {
            public static void Draw()
            {
                DrawEngine.SplashScreen.MainScreen2();
                DrawEngine.SplashScreen.MainScreen();

                ConsoleWindows.Window w = new ConsoleWindows.Window();
                w.Animated = true;
                w.Animation = ConsoleWindows.Additional.StadartAnimation;
                w.Animation.Frames = 2;
                w.Speed = 10;
                w.Border = ConsoleWindows.Additional.BoldBorder;
                w.BorderColor = ConsoleColor.DarkGray;
                //w.Border.HorizontalLine = '░';
                //w.Border.VerticalLine = '⌠';
                w.Border.LowerLeftCorner = '@';
                w.Border.LowerRightCorner = '@';
                w.Border.UpperLeftCorner = '@';
                w.Border.UpperRightCorner = '@';
                w.Border.PerpendicularRightward = '@';
                w.Border.PerpendicularLeftward = '@';

                w.Header = true;
                w.Height = 23;
                w.Width = 34;
                w.Left = 34;
                w.Top = 6;

                ConsoleWindows.Text t = new ConsoleWindows.Text(w);
                t.BackgroundColor = ConsoleColor.Black;
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.DarkCyan;
                t.Write("Создатели");
                t.ForegroundColor = ConsoleColor.Red;
                t.Write(" D12");
                t.AppendLine();
                t.AppendLine();                
                t.ForegroundColor = ConsoleColor.DarkRed;
                t.Write("Код и реализация: ");
                t.ForegroundColor = ConsoleColor.DarkYellow;
                t.Write("anete.anetes");                
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.DarkRed;
                t.Write("Помощь в создании: ");
                t.ForegroundColor = ConsoleColor.DarkYellow;
                t.Write("Phomm");
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.DarkRed;
                t.Write("Музыка/Звук: ");
                t.ForegroundColor = ConsoleColor.DarkYellow;
                t.Write("***");
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.DarkRed;
                t.Write("Геймдизайн: ");
                t.ForegroundColor = ConsoleColor.DarkYellow;
                t.Write("anete.anetes");
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.DarkRed;
                t.Write("Тестирование: ");
                t.ForegroundColor = ConsoleColor.DarkYellow;
                t.Write("Poniraq");
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.DarkRed;
                t.Write("Тестирование: ");
                t.ForegroundColor = ConsoleColor.DarkYellow;
                t.Write("Dilemial");
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.DarkRed;
                t.Write("Тестирование: ");
                t.ForegroundColor = ConsoleColor.DarkYellow;
                t.Write("rudream2");
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.DarkRed;
                t.Write("Тестирование: ");
                t.ForegroundColor = ConsoleColor.DarkYellow;
                t.Write("terma95");
                t.AppendLine();

                w.Text = t;

                //("Ваш персонаж:\n\nИмя: " + My.Name + "\nРаса:" + My.GetClassRace(1) + "\nКласс: " + My.GetClassRace(2) + "\nУровень: " + My.Level.ToString());

                ConsoleWindows.Button bex = new ConsoleWindows.Button(w);
                bex.Top = 14;
                bex.Left = 7;
                bex.Width = 20;
                bex.Height = 6;
                bex.ActiveColor = ConsoleColor.Red;
                bex.InactiveColor = ConsoleColor.DarkRed;
                bex.Label = "Продолжить";
                bex.CloseAfterUse = true;
                w.AddControl(bex);

                w.Draw();
            }
        }

        public static class PressAnyKey
        {
            public static void Draw()
            {
                DrawEngine.SplashScreen.MainScreen2();
                DrawEngine.SplashScreen.MainScreen();

                ConsoleWindows.Window w = new ConsoleWindows.Window();
                w.Animated = true;
                w.Animation = ConsoleWindows.Additional.StadartAnimation;
                w.Animation.Frames = 2;
                w.Speed = 10;
                w.Border = ConsoleWindows.Additional.BoldBorder;
                w.BorderColor = ConsoleColor.DarkGray;
                //w.Border.HorizontalLine = '░';
                //w.Border.VerticalLine = '⌠';
                w.Border.LowerLeftCorner = '@';
                w.Border.LowerRightCorner = '@';
                w.Border.UpperLeftCorner = '@';
                w.Border.UpperRightCorner = '@';
                w.Border.PerpendicularRightward = '@';
                w.Border.PerpendicularLeftward = '@';

                w.Header = true;
                w.Height = 13;
                w.Width = 26;
                w.Left = 38;
                w.Top = 11;

                ConsoleWindows.Text t = new ConsoleWindows.Text(w);
                t.BackgroundColor = ConsoleColor.Black;
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.DarkCyan;
                t.Write("Dungeon");
                t.ForegroundColor = ConsoleColor.Red;
                t.Write(" D12");
                t.AppendLine();
                w.Text = t;

                //("Ваш персонаж:\n\nИмя: " + My.Name + "\nРаса:" + My.GetClassRace(1) + "\nКласс: " + My.GetClassRace(2) + "\nУровень: " + My.Level.ToString());

                ConsoleWindows.Button bex = new ConsoleWindows.Button(w);
                bex.Top = 4;
                bex.Left = 3;
                bex.Width = 20;
                bex.Height = 6;
                bex.ActiveColor = ConsoleColor.Green;
                bex.InactiveColor = ConsoleColor.Green;
                bex.Label = "Нажмите Enter...";
                bex.CloseAfterUse = true;
                w.AddControl(bex);

                w.Draw();
            }
        }

        public static class EnterName
        {
            public static void Draw()
            {
                DrawEngine.SplashScreen.MainScreen2();
                DrawEngine.SplashScreen.MainScreen();

                ConsoleWindows.Window w = new ConsoleWindows.Window();
                w.Animated = true;
                w.Animation = ConsoleWindows.Additional.HorizontalAnimation;
                w.Animation.Frames = 2;
                w.Speed = 10;
                w.Border = ConsoleWindows.Additional.BoldBorder;
                w.BorderColor = ConsoleColor.DarkGray;
                //w.Border.HorizontalLine = '░';
                //w.Border.VerticalLine = '⌠';
                w.Border.LowerLeftCorner = '@';
                w.Border.LowerRightCorner = '@';
                w.Border.UpperLeftCorner = '@';
                w.Border.UpperRightCorner = '@';
                w.Border.PerpendicularRightward = '@';
                w.Border.PerpendicularLeftward = '@';

                w.Header = true;
                w.Height = 24;
                w.Width = 26;
                w.Left = 36;
                w.Top = 5;

                ConsoleWindows.Text t = new ConsoleWindows.Text(w);
                t.BackgroundColor = ConsoleColor.Black;
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.DarkCyan;
                t.Write("Введите имя");
                t.ForegroundColor = ConsoleColor.Red;
                t.Write(" D12");
                t.AppendLine();
                w.Text = t;




                //Controls 
                ConsoleWindows.Button bng = new ConsoleWindows.Button(w);
                bng.Top = 15;
                bng.Left = 3;
                bng.Width = 20;
                bng.Height = 6;
                bng.ActiveColor = ConsoleColor.Red;
                bng.InactiveColor = ConsoleColor.DarkRed;
                bng.CloseAfterUse = true;
                bng.Label = "Подтвердить";
                bng.OnClick = () =>
                {
                    Rogue.RAM.Player.Name = (string)w.Return;
                };
                w.AddControl(bng);

                ConsoleWindows.Button bgcl = new ConsoleWindows.Button(w);
                bgcl.Top = 4;
                bgcl.Left = 3;
                bgcl.Width = 20;
                bgcl.Height = 6;
                bgcl.ActiveColor = ConsoleColor.Red;
                bgcl.InactiveColor = ConsoleColor.DarkRed;
                bgcl.CloseAfterUse = true;
                bgcl.Label = "Очистить";
                bgcl.OnClick = () =>
                {
                    
                };
                w.AddControl(bgcl);

                ConsoleWindows.TextBox bfg = new ConsoleWindows.TextBox(w);
                bfg.Top = 9;
                bfg.Left = 3;
                bfg.Width = 20;
                bfg.Height = 6;
                bfg.ActiveColor = ConsoleColor.Red;
                bfg.InactiveColor = ConsoleColor.DarkRed;
                bfg.OnEndTyping = () => { bfg.Return = bfg.Text; };
                bfg.Label = " ";
                w.AddControl(bfg);

                w.Draw();
            }
        }

        public static class EnterRace
        {
            public static void Draw()
            {
                DrawEngine.SplashScreen.MainScreen2();
                DrawEngine.SplashScreen.MainScreen();

                ConsoleWindows.Window w = new ConsoleWindows.Window();
                w.Animated = true;
                w.Animation = ConsoleWindows.Additional.StadartAnimation;
                w.Animation.Frames = 2;
                w.Speed = 10;
                w.Border = ConsoleWindows.Additional.BoldBorder;
                w.BorderColor = ConsoleColor.DarkGray;
                //w.Border.HorizontalLine = '░';
                //w.Border.VerticalLine = '⌠';
                w.Border.LowerLeftCorner = '@';
                w.Border.LowerRightCorner = '@';
                w.Border.UpperLeftCorner = '@';
                w.Border.UpperRightCorner = '@';
                w.Border.PerpendicularRightward = '@';
                w.Border.PerpendicularLeftward = '@';

                w.Header = true;
                w.Height = 27;
                w.Width = 47;
                w.Left = 25;
                w.Top = 2;

                ConsoleWindows.Text t = new ConsoleWindows.Text(w);
                t.BackgroundColor = ConsoleColor.Black;
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.DarkCyan;
                t.Write("Dungeon");
                t.ForegroundColor = ConsoleColor.Red;
                t.Write(" 12");
                t.AppendLine();
                t.TextPosition = ConsoleWindows.TextPosition.Right;
                t.ForegroundColor = ConsoleColor.Red;
                t.WriteLine("Альфа");
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.Magenta;
                t.Write("[You are not alone]");
                t.AppendLine();

                w.Text = t;

                //Controls 
                ConsoleWindows.Button bng = new ConsoleWindows.Button(w);
                bng.Top = 6;
                bng.Left = 3;
                bng.Width = 20;
                bng.Height = 6;
                bng.ActiveColor = ConsoleColor.Red;
                bng.InactiveColor = ConsoleColor.DarkRed;
                bng.CloseAfterUse = true;
                bng.Label = "Человек";
                bng.OnClick = () => { Rogue.RAM.Player.Race = MechEngine.Race.Human; };
                w.AddControl(bng);

                ConsoleWindows.Button bfg = new ConsoleWindows.Button(w);
                bfg.Top = 9;
                bfg.Left = 3;
                bfg.Width = 20;
                bfg.Height = 6;
                bfg.ActiveColor = ConsoleColor.Red;
                bfg.InactiveColor = ConsoleColor.DarkRed;
                bfg.Label = "Азрай";
                bfg.CloseAfterUse = true;
                bfg.OnClick = () => { Rogue.RAM.Player.Race = MechEngine.Race.Elf; };
                w.AddControl(bfg);

                ConsoleWindows.Button ba = new ConsoleWindows.Button(w);
                ba.Top = 12;
                ba.Left = 3;
                ba.Width = 20;
                ba.Height = 6;
                ba.ActiveColor = ConsoleColor.Red;
                ba.InactiveColor = ConsoleColor.DarkRed;
                ba.Label = "Дроу";
                ba.CloseAfterUse = true;
                ba.OnClick = () => { Rogue.RAM.Player.Race = MechEngine.Race.DarkElf; };
                w.AddControl(ba);

                ConsoleWindows.Button bex = new ConsoleWindows.Button(w);
                bex.Top = 15;
                bex.Left = 3;
                bex.Width = 20;
                bex.Height = 6;
                bex.ActiveColor = ConsoleColor.Red;
                bex.InactiveColor = ConsoleColor.DarkRed;
                bex.Label = "Дварф";
                bex.CloseAfterUse = true;
                bex.OnClick = () => { Rogue.RAM.Player.Race = MechEngine.Race.Dwarf; };
                w.AddControl(bex);

                ConsoleWindows.Button bey = new ConsoleWindows.Button(w);
                bey.Top = 18;
                bey.Left = 3;
                bey.Width = 20;
                bey.Height = 6;
                bey.ActiveColor = ConsoleColor.Red;
                bey.InactiveColor = ConsoleColor.DarkRed;
                bey.Label = "Гном";
                bey.CloseAfterUse = true;
                bey.OnClick = () => { Rogue.RAM.Player.Race = MechEngine.Race.Gnome; };
                w.AddControl(bey);

                ConsoleWindows.Button ber = new ConsoleWindows.Button(w);
                ber.Top = 6;
                ber.Left = 24;
                ber.Width = 20;
                ber.Height = 6;
                ber.ActiveColor = ConsoleColor.Red;
                ber.InactiveColor = ConsoleColor.DarkRed;
                ber.Label = "Калдорай";
                ber.CloseAfterUse = true;
                ber.OnClick = () => { Rogue.RAM.Player.Race = MechEngine.Race.MoonElf; };
                w.AddControl(ber);

                ConsoleWindows.Button bfgq = new ConsoleWindows.Button(w);
                bfgq.Top = 9;
                bfgq.Left = 24;
                bfgq.Width = 20;
                bfgq.Height = 6;
                bfgq.ActiveColor = ConsoleColor.Red;
                bfgq.InactiveColor = ConsoleColor.DarkRed;
                bfgq.Label = "Орк";
                bfgq.CloseAfterUse = true;
                bfgq.OnClick = () => { Rogue.RAM.Player.Race = MechEngine.Race.Orc; };
                w.AddControl(bfgq);

                ConsoleWindows.Button baq = new ConsoleWindows.Button(w);
                baq.Top = 12;
                baq.Left = 24;
                baq.Width = 20;
                baq.Height = 6;
                baq.ActiveColor = ConsoleColor.Red;
                baq.InactiveColor = ConsoleColor.DarkRed;
                baq.Label = "Тролль";
                baq.CloseAfterUse = true;
                baq.OnClick = () => { Rogue.RAM.Player.Race = MechEngine.Race.Troll; };
                w.AddControl(baq);

                ConsoleWindows.Button bexq = new ConsoleWindows.Button(w);
                bexq.Top = 15;
                bexq.Left = 24;
                bexq.Width = 20;
                bexq.Height = 6;
                bexq.ActiveColor = ConsoleColor.Red;
                bexq.InactiveColor = ConsoleColor.DarkRed;
                bexq.Label = "Нежить";
                bexq.CloseAfterUse = true;
                bexq.OnClick = () => { Rogue.RAM.Player.Race = MechEngine.Race.Undead; };
                w.AddControl(bexq);

                ConsoleWindows.Button beyq = new ConsoleWindows.Button(w);
                beyq.Top = 18;
                beyq.Left = 24;
                beyq.Width = 20;
                beyq.Height = 6;
                beyq.ActiveColor = ConsoleColor.Red;
                beyq.InactiveColor = ConsoleColor.DarkRed;
                beyq.Label = "Падший";
                beyq.CloseAfterUse = true;
                beyq.OnClick = () => { Rogue.RAM.Player.Race = MechEngine.Race.FallenAngel; };
                w.AddControl(beyq);

                w.Draw();
            }
        }

        public static class EnterClass
        {
            public static void Draw()
            {
                DrawEngine.SplashScreen.MainScreen2();
                DrawEngine.SplashScreen.MainScreen();

                ConsoleWindows.Window w = new ConsoleWindows.Window();
                w.Animated = true;
                w.Animation = ConsoleWindows.Additional.StadartAnimation;
                w.Animation.Frames = 2;
                w.Speed = 10;
                w.Border = ConsoleWindows.Additional.BoldBorder;
                w.BorderColor = ConsoleColor.DarkGray;
                //w.Border.HorizontalLine = '░';
                //w.Border.VerticalLine = '⌠';
                w.Border.LowerLeftCorner = '@';
                w.Border.LowerRightCorner = '@';
                w.Border.UpperLeftCorner = '@';
                w.Border.UpperRightCorner = '@';
                w.Border.PerpendicularRightward = '@';
                w.Border.PerpendicularLeftward = '@';

                w.Header = true;
                w.Height = 27;
                w.Width = 47;
                w.Left = 25;
                w.Top = 2;

                ConsoleWindows.Text t = new ConsoleWindows.Text(w);
                t.BackgroundColor = ConsoleColor.Black;
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.DarkCyan;
                t.Write("Dungeon");
                t.ForegroundColor = ConsoleColor.Red;
                t.Write(" 12");
                t.AppendLine();
                t.TextPosition = ConsoleWindows.TextPosition.Right;
                t.ForegroundColor = ConsoleColor.Red;
                t.WriteLine("Альфа");
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.Magenta;
                t.Write("[You are not alone]");
                t.AppendLine();

                w.Text = t;

                //Controls 
                ConsoleWindows.Button bng = new ConsoleWindows.Button(w);
                bng.Top = 6;
                bng.Left = 3;
                bng.Width = 20;
                bng.Height = 6;
                bng.ActiveColor = ConsoleColor.Red;
                bng.InactiveColor = ConsoleColor.DarkRed;
                bng.CloseAfterUse = true;
                bng.Label = "Маг крови";
                bng.OnClick = () => { Rogue.RAM.Player.Class = MechEngine.BattleClass.BloodMage; };
                w.AddControl(bng);

                ConsoleWindows.Button bfg = new ConsoleWindows.Button(w);
                bfg.Top = 9;
                bfg.Left = 3;
                bfg.Width = 20;
                bfg.Height = 6;
                bfg.ActiveColor = ConsoleColor.Red;
                bfg.InactiveColor = ConsoleColor.DarkRed;
                bfg.Label = "Паладин";
                bfg.CloseAfterUse = true;
                bfg.OnClick = () => { Rogue.RAM.Player.Class = MechEngine.BattleClass.Paladin; };
                w.AddControl(bfg);

                ConsoleWindows.Button ba = new ConsoleWindows.Button(w);
                ba.Top = 12;
                ba.Left = 3;
                ba.Width = 20;
                ba.Height = 6;
                ba.ActiveColor = ConsoleColor.Red;
                ba.InactiveColor = ConsoleColor.DarkRed;
                ba.Label = "Алхимик";
                ba.CloseAfterUse = true;
                ba.OnClick = () => { Rogue.RAM.Player.Class = MechEngine.BattleClass.Alchemist; };
                w.AddControl(ba);

                ConsoleWindows.Button bex = new ConsoleWindows.Button(w);
                bex.Top = 15;
                bex.Left = 3;
                bex.Width = 20;
                bex.Height = 6;
                bex.ActiveColor = ConsoleColor.Red;
                bex.InactiveColor = ConsoleColor.DarkRed;
                bex.Label = "Разбойник";
                bex.CloseAfterUse = true;
                bex.OnClick = () => { Rogue.RAM.Player.Class = MechEngine.BattleClass.Assassin; };
                w.AddControl(bex);

                ConsoleWindows.Button bey = new ConsoleWindows.Button(w);
                bey.Top = 18;
                bey.Left = 3;
                bey.Width = 20;
                bey.Height = 6;
                bey.ActiveColor = ConsoleColor.Red;
                bey.InactiveColor = ConsoleColor.DarkRed;
                bey.Label = "Маг огня";
                bey.CloseAfterUse = true;
                bey.OnClick = () => { Rogue.RAM.Player.Class = MechEngine.BattleClass.FireMage; };
                w.AddControl(bey);

                ConsoleWindows.Button ber = new ConsoleWindows.Button(w);
                ber.Top = 6;
                ber.Left = 24;
                ber.Width = 20;
                ber.Height = 6;
                ber.ActiveColor = ConsoleColor.Red;
                ber.InactiveColor = ConsoleColor.DarkRed;
                ber.Label = "Экзорцист";
                ber.CloseAfterUse = true;
                ber.OnClick = () => { Rogue.RAM.Player.Class = MechEngine.BattleClass.Inquisitor; };
                w.AddControl(ber);

                ConsoleWindows.Button bfgq = new ConsoleWindows.Button(w);
                bfgq.Top = 9;
                bfgq.Left = 24;
                bfgq.Width = 20;
                bfgq.Height = 6;
                bfgq.ActiveColor = ConsoleColor.Red;
                bfgq.InactiveColor = ConsoleColor.DarkRed;
                bfgq.Label = "Монах";
                bfgq.CloseAfterUse = true;
                bfgq.OnClick = () => { Rogue.RAM.Player.Class = MechEngine.BattleClass.Monk; };
                w.AddControl(bfgq);

                ConsoleWindows.Button baq = new ConsoleWindows.Button(w);
                baq.Top = 12;
                baq.Left = 24;
                baq.Width = 20;
                baq.Height = 6;
                baq.ActiveColor = ConsoleColor.Red;
                baq.InactiveColor = ConsoleColor.DarkRed;
                baq.Label = "Некромант";
                baq.CloseAfterUse = true;
                baq.OnClick = () => { Rogue.RAM.Player.Class = MechEngine.BattleClass.Necromant; };
                w.AddControl(baq);

                ConsoleWindows.Button bexq = new ConsoleWindows.Button(w);
                bexq.Top = 15;
                bexq.Left = 24;
                bexq.Width = 20;
                bexq.Height = 6;
                bexq.ActiveColor = ConsoleColor.Red;
                bexq.InactiveColor = ConsoleColor.DarkRed;
                bexq.Label = "Шаман";
                bexq.CloseAfterUse = true;
                bexq.OnClick = () => { Rogue.RAM.Player.Class = MechEngine.BattleClass.Shaman; };
                w.AddControl(bexq);

                ConsoleWindows.Button beyq = new ConsoleWindows.Button(w);
                beyq.Top = 18;
                beyq.Left = 24;
                beyq.Width = 20;
                beyq.Height = 6;
                beyq.ActiveColor = ConsoleColor.Red;
                beyq.InactiveColor = ConsoleColor.DarkRed;
                beyq.Label = "Воин";
                beyq.CloseAfterUse = true;
                beyq.OnClick = () => { Rogue.RAM.Player.Class = MechEngine.BattleClass.Warrior; };
                w.AddControl(beyq);

                w.Draw();
            }
        }
    }
}
