using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.Data;
using Rogue.Drawing.GUI;
using Rogue.Drawing.Labirinth;

namespace Rogue.Drawing
{

    public static class DialogueDraw
    {

        public static void Helpers()
        {
            PlayEngine.Enemy = false;

            ConsoleWindows.Window w = new ConsoleWindows.Window();
            w.Animated = true;
            w.Animation = ConsoleWindows.Additional.StadartAnimation;
            w.Border = ConsoleWindows.Additional.BoldBorder;
            w.BorderColor = ConsoleColor.DarkGreen;
            w.Header = true;
            w.Height = 20;
            w.Width = 60;
            w.Left = 20;
            w.Top = 5;

            ConsoleWindows.Text t = new ConsoleWindows.Text(w);
            t.BackgroundColor = ConsoleColor.Black;
            t.TextPosition = ConsoleWindows.TextPosition.Center;
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("Добро пожаловать в ");
            t.ForegroundColor = ConsoleColor.DarkCyan;
            t.Write("Dungeon");
            t.ForegroundColor = ConsoleColor.Red;
            t.Write(" 12");
            //t.ForegroundColor = ConsoleColor.Gray;
            //t.Write(" [");
            t.ForegroundColor = ConsoleColor.DarkMagenta;
            t.Write("YANA");
            t.ForegroundColor = ConsoleColor.Gray;
            //t.Write("] !");
            t.WriteLine("!");
            t.AppendLine();
            t.WriteLine("Вы желатете пройти обучение?");
            t.AppendLine();
            t.AppendLine();

            t.Write("Нажмите ");
            t.ForegroundColor = ConsoleColor.DarkYellow;
            t.Write("[Tab]");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write(" для перемещения между кнопками.");
            t.AppendLine();

            t.Write("Текущую активную кнопку можно узнать по ");
            t.ForegroundColor = ConsoleColor.Yellow;
            t.Write("желтой");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write(" надписи.");
            t.AppendLine();

            t.Write("Нажмите ");
            t.ForegroundColor = ConsoleColor.DarkYellow;
            t.Write("[Enter]");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write(" для выбора варианта.");
            t.AppendLine();

            w.Text = t;

            //Controls 
            ConsoleWindows.Button by = new ConsoleWindows.Button(w);
            by.Top = 11;
            by.Left = 12;
            by.Width = 8;
            by.Height = 5;
            by.ActiveColor = ConsoleColor.Yellow;
            by.InactiveColor = ConsoleColor.DarkGray;
            by.CloseAfterUse = true;
            by.Label = "Да";
            w.AddControl(by);

            ConsoleWindows.Button bn = new ConsoleWindows.Button(w);
            bn.Top = 11;
            bn.Left = 40;
            bn.Width = 8;
            bn.Height = 12;
            bn.ActiveColor = ConsoleColor.Yellow;
            bn.InactiveColor = ConsoleColor.DarkGray;
            bn.Label = "Нет";
            bn.CloseAfterUse = true;
            w.AddControl(bn);

            w.Draw();

            if (w.Sender.Label == "Нет")
            {
                w = new ConsoleWindows.Window();
                w.Animated = true;
                w.Animation = ConsoleWindows.Additional.StadartAnimation;
                w.Border = ConsoleWindows.Additional.BoldBorder;
                w.BorderColor = ConsoleColor.DarkGreen;
                w.Header = true;
                w.Height = 20;
                w.Width = 60;
                w.Left = 20;
                w.Top = 5;

                t = new ConsoleWindows.Text(w);
                t.BackgroundColor = ConsoleColor.Black;
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("Добро пожаловать в ");
                t.ForegroundColor = ConsoleColor.DarkCyan;
                t.Write("Dungeon");
                t.ForegroundColor = ConsoleColor.Red;
                t.Write(" 12");
                //t.ForegroundColor = ConsoleColor.Gray;
                //t.Write(" [");
                t.ForegroundColor = ConsoleColor.DarkMagenta;
                t.Write("YANA");
                t.ForegroundColor = ConsoleColor.Gray;
                //t.Write("] !");
                t.WriteLine("!");
                t.AppendLine();
                t.AppendLine();
                t.AppendLine();
                t.AppendLine();
                t.WriteLine("Не забудьте поговорить со своим ангелом-хранителем!");
                t.WriteLine("Команда разработчиков желает вам приятной игры!");
                w.Text = t;

                //Controls 
                by = new ConsoleWindows.Button(w);
                by.Top = 11;
                by.Left = 20;
                by.Width = 20;
                by.Height = 5;
                by.ActiveColor = ConsoleColor.Yellow;
                by.InactiveColor = ConsoleColor.DarkGray;
                by.Label = "Продолжить";
                by.CloseAfterUse = true;
                w.AddControl(by);

                w.Draw();
            }
            else
            {
                Education();
            }


            Draw.Session<ClearSession>(x => x.ClearAll = true)
                .Then<GUIBorderDrawSession>()
                .Then<CharStatDrawSession>()
                .Publish();


            PlayEngine.Enemy = true;
        }
        public static void Education()
        {
            prt1();
            prt2();
            prt3();
            prt4();
            prt5();
            prt6();
            prt7();
            prt8();
            prt9();
            prt10();
            prt11();
        }
        private static void prt1()
        {
            ConsoleWindows.Window w = new ConsoleWindows.Window();
            w.Animated = true;
            w.Animation = ConsoleWindows.Additional.StadartAnimation;
            w.Border = ConsoleWindows.Additional.BoldBorder;
            w.BorderColor = ConsoleColor.DarkGreen;
            w.Header = true;
            w.Height = 20;
            w.Width = 65;
            w.Left = 15;
            w.Top = 5;

            ConsoleWindows.Text t = new ConsoleWindows.Text(w);
            t.BackgroundColor = ConsoleColor.Black;
            t.TextPosition = ConsoleWindows.TextPosition.Center;
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("Обучение: Интерфейс - ");
            t.ForegroundColor = ConsoleColor.Red;
            t.Write("1");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("/");
            t.ForegroundColor = ConsoleColor.Green;
            t.Write("4");
            t.AppendLine();

            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("Справа (");
            t.ForegroundColor = ConsoleColor.Yellow;
            t.Write("→");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write(") располагается окно персонажа.");
            t.AppendLine();

            t.WriteLine("В окне персонажа показываются ваши текущие характеристики:");

            t.ForegroundColor = ConsoleColor.Cyan;
            t.Write("Имя персонажа,"); t.ForegroundColor = ConsoleColor.Gray; t.Write(" Раса, Класс,"); t.ForegroundColor = ConsoleColor.DarkGray; t.Write(" Уровень, Опыт, ");
            t.ForegroundColor = ConsoleColor.Red;
            t.Write("Здоровье,"); t.ForegroundColor = ConsoleColor.White; t.Write(" Ресурсы,");
            t.AppendLine();
            t.ForegroundColor = ConsoleColor.DarkYellow;
            t.Write("Урон от удара,"); t.ForegroundColor = ConsoleColor.DarkRed; t.Write("Сила атаки,"); t.ForegroundColor = ConsoleColor.DarkCyan; t.Write(" Сила магии,"); t.ForegroundColor = ConsoleColor.DarkGreen; t.Write(" Два вида защиты,");
            t.AppendLine();
            t.ForegroundColor = ConsoleColor.Yellow;
            t.Write("Золото,"); t.ForegroundColor = ConsoleColor.DarkRed; t.Write(" Инвентарь.");
            t.AppendLine();
            t.AppendLine();

            t.ForegroundColor = ConsoleColor.Gray;
            t.WriteLine("Подробнее о них вы узнаете в процессе игры.");

            w.Text = t;

            //Controls 
            ConsoleWindows.Button by = new ConsoleWindows.Button(w);
            by.Top = 12;
            by.Left = 22;
            by.Width = 20;
            by.Height = 5;
            by.ActiveColor = ConsoleColor.Yellow;
            by.InactiveColor = ConsoleColor.DarkGray;
            by.Label = "Продолжить";
            by.CloseAfterUse = true;
            w.AddControl(by);

            w.Draw();

        }
        private static void prt2()
        {
            ConsoleWindows.Window w = new ConsoleWindows.Window();
            w.Animated = true;
            w.Animation = ConsoleWindows.Additional.StadartAnimation;
            w.Border = ConsoleWindows.Additional.BoldBorder;
            w.BorderColor = ConsoleColor.DarkGreen;
            w.Header = true;
            w.Height = 20;
            w.Width = 78;
            w.Left = 12;
            w.Top = 5;

            ConsoleWindows.Text t = new ConsoleWindows.Text(w);
            t.BackgroundColor = ConsoleColor.Black;
            t.TextPosition = ConsoleWindows.TextPosition.Center;
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("Обучение: Интерфейс - ");
            t.ForegroundColor = ConsoleColor.DarkYellow;
            t.Write("2");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("/");
            t.ForegroundColor = ConsoleColor.Green;
            t.Write("4");
            t.AppendLine();

            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("Слева от окна персонажа (");
            t.ForegroundColor = ConsoleColor.Yellow;
            t.Write("←");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write(") располагается окно карты.");
            t.AppendLine();

            t.WriteLine("Это главное окно игры, в нём вы можете взаимодействовать с миром.");
            t.WriteLine("Для того что бы узнать условные обозначения, нажмите клавишу [M]");
            t.WriteLine("в режиме активной игры, и вы увидите основные обозначение объектов.");
            t.WriteLine("Для того что бы с ними взаимодействовать, посмотрите нижнее окно.");
            t.WriteLine("Это окно доступных клавиш. В зависимости от игрового экрана, на");
            t.WriteLine("нем отображается список доступных действий. На главном экране,");
            t.WriteLine("Основными клавишами являются: [C],[I],[Q,W,E,R],[O],[1-6].");

            w.Text = t;

            //Controls 
            ConsoleWindows.Button by = new ConsoleWindows.Button(w);
            by.Top = 12;
            by.Left = 27;
            by.Width = 20;
            by.Height = 5;
            by.ActiveColor = ConsoleColor.Yellow;
            by.InactiveColor = ConsoleColor.DarkGray;
            by.Label = "Продолжить";
            by.CloseAfterUse = true;
            w.AddControl(by);

            w.Draw();
        }
        private static void prt3()
        {
            ConsoleWindows.Window w = new ConsoleWindows.Window();
            w.Animated = true;
            w.Animation = ConsoleWindows.Additional.StadartAnimation;
            w.Border = ConsoleWindows.Additional.BoldBorder;
            w.BorderColor = ConsoleColor.DarkGreen;
            w.Header = true;
            w.Height = 20;
            w.Width = 78;
            w.Left = 12;
            w.Top = 5;

            ConsoleWindows.Text t = new ConsoleWindows.Text(w);
            t.BackgroundColor = ConsoleColor.Black;
            t.TextPosition = ConsoleWindows.TextPosition.Center;
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("Обучение: Интерфейс - ");
            t.ForegroundColor = ConsoleColor.Yellow;
            t.Write("3");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("/");
            t.ForegroundColor = ConsoleColor.Green;
            t.Write("4");
            t.AppendLine();

            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("Под окном карты и персонажа (");
            t.ForegroundColor = ConsoleColor.Yellow;
            t.Write("↓");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write(") располагается окно информации.");
            t.AppendLine();

            t.WriteLine("В окне информации вы получаете основной отклик своих действий.");
            t.WriteLine("Цвет текста сообщения имеет значение:");
            t.AppendLine();
            List<char> rainbow = new List<char>() { 'Ц', 'в', 'е', 'т', 'н', 'о', 'й' };
            for (int i = 0; i < 7; i++)
            {
                t.ForegroundColor = (ConsoleColor)i + 5;
                t.Write(Convert.ToString(rainbow[i]));
            }
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write(" текст - в котором различные слова цветные, обычный.");
            t.AppendLine();
            t.ForegroundColor = ConsoleColor.DarkGreen;
            t.Write("Цвет карты");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write(" - чаще всего такой цвет означает не важные действия.");
            t.AppendLine();
            t.ForegroundColor = ConsoleColor.White;
            t.Write("Белый");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write(" - цвет предупреждений. Обращайте внимание на белые сообщения.");
            t.AppendLine();
            t.ForegroundColor = ConsoleColor.Red;
            t.Write("Красный");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write(" - цвет критических предупреждений. Сообщение очень важно.");
            t.AppendLine();

            w.Text = t;

            //Controls 
            ConsoleWindows.Button by = new ConsoleWindows.Button(w);
            by.Top = 12;
            by.Left = 27;
            by.Width = 20;
            by.Height = 5;
            by.ActiveColor = ConsoleColor.Yellow;
            by.InactiveColor = ConsoleColor.DarkGray;
            by.Label = "Продолжить";
            by.CloseAfterUse = true;
            w.AddControl(by);

            w.Draw();
        }
        private static void prt4()
        {
            ConsoleWindows.Window w = new ConsoleWindows.Window();
            w.Animated = true;
            w.Animation = ConsoleWindows.Additional.StadartAnimation;
            w.Border = ConsoleWindows.Additional.BoldBorder;
            w.BorderColor = ConsoleColor.DarkGreen;
            w.Header = true;
            w.Height = 20;
            w.Width = 78;
            w.Left = 12;
            w.Top = 5;

            ConsoleWindows.Text t = new ConsoleWindows.Text(w);
            t.BackgroundColor = ConsoleColor.Black;
            t.TextPosition = ConsoleWindows.TextPosition.Center;
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("Обучение: Интерфейс - ");
            t.ForegroundColor = ConsoleColor.Green;
            t.Write("4");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("/");
            t.ForegroundColor = ConsoleColor.Green;
            t.Write("4");
            t.AppendLine();

            t.ForegroundColor = ConsoleColor.Gray;
            t.AppendLine();
            t.AppendLine();
            t.WriteLine("Существует множество разных окон и экранов.");
            t.WriteLine("Для того что бы в них разобраться, следует чаще смотреть");
            t.WriteLine("на окно доступных клавиш. Так же, следует знать, что основные");
            t.WriteLine("клавиши перемещения по объектам на экране - стрелки.");

            w.Text = t;

            //Controls 
            ConsoleWindows.Button by = new ConsoleWindows.Button(w);
            by.Top = 11;
            by.Left = 17;
            by.Width = 40;
            by.Height = 5;
            by.ActiveColor = ConsoleColor.Yellow;
            by.InactiveColor = ConsoleColor.DarkGray;
            by.Label = "Перейти к обучению на карте.";
            by.CloseAfterUse = true;
            w.AddControl(by);

            w.Draw();
        }
        private static void prt5()
        {
            ConsoleWindows.Window w = new ConsoleWindows.Window();
            w.Animated = true;
            w.Animation = ConsoleWindows.Additional.StadartAnimation;
            w.Border = ConsoleWindows.Additional.BoldBorder;
            w.BorderColor = ConsoleColor.DarkGreen;
            w.Header = true;
            w.Height = 20;
            w.Width = 78;
            w.Left = 12;
            w.Top = 5;

            ConsoleWindows.Text t = new ConsoleWindows.Text(w);
            t.BackgroundColor = ConsoleColor.Black;
            t.TextPosition = ConsoleWindows.TextPosition.Center;
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("Обучение: Карта - ");
            t.ForegroundColor = ConsoleColor.Red;
            t.Write("1");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("/");
            t.ForegroundColor = ConsoleColor.Green;
            t.Write("3");
            t.AppendLine();

            t.ForegroundColor = ConsoleColor.Gray;
            t.AppendLine();
            t.AppendLine();
            t.WriteLine("Передвижение.");
            t.WriteLine("Для того что бы сдвинуться с места, закройте это окно, и");
            t.Write("нажмите на клавишу ");
            t.ForegroundColor = ConsoleColor.Yellow;
            t.Write("[↓]");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write(" .");
            t.AppendLine();

            w.Text = t;

            //Controls 
            ConsoleWindows.Button by = new ConsoleWindows.Button(w);
            by.Top = 11;
            by.Left = 17;
            by.Width = 40;
            by.Height = 5;
            by.ActiveColor = ConsoleColor.Yellow;
            by.InactiveColor = ConsoleColor.DarkGray;
            by.Label = "Закрыть окно.";
            by.CloseAfterUse = true;
            w.AddControl(by);

            w.Draw();


            Draw.Session<ClearSession>(x => x.ClearAll = true)
                .Then<LabirinthDrawSession>()
                .Then<GUIBorderDrawSession>()
                .Then<CharacterDataDrawSession>()
                .Then<CharMapDrawSession>(x => x.Commands = new List<string>()
                                {
                                    "[↓] - Идти вниз "
                                })
                .Publish();

            bool end = false;
            while (!end)
            {
                ConsoleKey r = Console.ReadKey(true).Key;
                switch (r)
                {
                    case ConsoleKey.DownArrow:
                        {
                            Rogue.RAM.Step.Step();
                            PlayEngine.GamePlay.MoveCharacter(2);
                            end = true; break;
                        }
                    default: { break; }
                }
            }
        }
        private static void prt6()
        {
            ConsoleWindows.Window w = new ConsoleWindows.Window();
            w.Animated = true;
            w.Animation = ConsoleWindows.Additional.StadartAnimation;
            w.Border = ConsoleWindows.Additional.BoldBorder;
            w.BorderColor = ConsoleColor.DarkGreen;
            w.Header = true;
            w.Height = 20;
            w.Width = 80;
            w.Left = 12;
            w.Top = 5;

            ConsoleWindows.Text t = new ConsoleWindows.Text(w);
            t.BackgroundColor = ConsoleColor.Black;
            t.TextPosition = ConsoleWindows.TextPosition.Center;
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("Обучение: Карта - ");
            t.ForegroundColor = ConsoleColor.Yellow;
            t.Write("2");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("/");
            t.ForegroundColor = ConsoleColor.Green;
            t.Write("3");
            t.AppendLine();

            t.ForegroundColor = ConsoleColor.Gray;
            t.AppendLine();
            t.AppendLine();
            t.WriteLine("Передвижение.");
            t.WriteLine("Ваш персонаж переместился вниз, теперь он может ходить по всем направлениям.");
            t.Write("Закройте окно, найдите любого NPC, и нажмите ");
            t.ForegroundColor = ConsoleColor.Yellow;
            t.Write("[I]");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write(" .");
            t.AppendLine();
            w.Text = t;

            ConsoleWindows.Button by = new ConsoleWindows.Button(w);
            by.Top = 11;
            by.Left = 17;
            by.Width = 40;
            by.Height = 5;
            by.ActiveColor = ConsoleColor.Yellow;
            by.InactiveColor = ConsoleColor.DarkGray;
            by.Label = "Закрыть окно.";
            by.CloseAfterUse = true;
            w.AddControl(by);

            w.Draw();



            Draw.Session<ClearSession>(x => x.ClearAll = true)
                .Then<LabirinthDrawSession>()
                .Then<GUIBorderDrawSession>()
                .Then<CharacterDataDrawSession>()
                .Then<CharMapDrawSession>(x => x.Commands = new List<string>()
                                {
                                    "[I] - Идентификация ",
                                    "[↓] - Идти вниз ",
                                    "[↑] - Идти вверх ",
                                    "[→] - Идти влево ",
                                    "[←] - Идти вправо "
                                })
                .Publish();


            bool end = false;
            while (!end)
            {
                ConsoleKey r = Console.ReadKey(true).Key;
                switch (r)
                {
                    case ConsoleKey.UpArrow:
                        {
                            PlayEngine.GamePlay.MoveCharacter(1);
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            PlayEngine.GamePlay.MoveCharacter(2);
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            PlayEngine.GamePlay.MoveCharacter(3);
                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            PlayEngine.GamePlay.MoveCharacter(4);
                            break;
                        }
                    case ConsoleKey.I:
                        {
                            PlayEngine.GamePlay.GetInfo.GetInfoFromMap();
                            PlayEngine.Enemy = false;
                            end = true;
                            break;
                        }
                }
            }
        }
        private static void prt7()
        {
            ConsoleWindows.Window w = new ConsoleWindows.Window();
            w.Animated = true;
            w.Animation = ConsoleWindows.Additional.StadartAnimation;
            w.Border = ConsoleWindows.Additional.BoldBorder;
            w.BorderColor = ConsoleColor.DarkGreen;
            w.Header = true;
            w.Height = 20;
            w.Width = 80;
            w.Left = 12;
            w.Top = 5;

            ConsoleWindows.Text t = new ConsoleWindows.Text(w);
            t.BackgroundColor = ConsoleColor.Black;
            t.TextPosition = ConsoleWindows.TextPosition.Center;
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("Обучение: Карта - ");
            t.ForegroundColor = ConsoleColor.Green;
            t.Write("3");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("/");
            t.ForegroundColor = ConsoleColor.Green;
            t.Write("3");
            t.AppendLine();

            t.ForegroundColor = ConsoleColor.Gray;
            t.WriteLine("Передвижение.");
            t.WriteLine("Все активные команды:");
            t.WriteLine("Начать разговор, Атаковать, Инфо о персонаже - работают аналогично");
            t.AppendLine();
            t.Write("Чаще используйте команду - ");
            t.ForegroundColor = ConsoleColor.Yellow;
            t.Write("[I]");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write(" .");
            t.AppendLine();
            t.WriteLine("Многие непонятные детали станут сразу объяснимы.");
            t.AppendLine();
            w.Text = t;

            ConsoleWindows.Button by = new ConsoleWindows.Button(w);
            by.Top = 11;
            by.Left = 17;
            by.Width = 40;
            by.Height = 5;
            by.ActiveColor = ConsoleColor.Yellow;
            by.InactiveColor = ConsoleColor.DarkGray;
            by.Label = "Перейти к обучению в битве.";
            by.CloseAfterUse = true;
            w.AddControl(by);

            w.Draw();
        }
        private static void prt8()
        {
            ConsoleWindows.Window w = new ConsoleWindows.Window();
            w.Animated = true;
            w.Animation = ConsoleWindows.Additional.StadartAnimation;
            w.Border = ConsoleWindows.Additional.BoldBorder;
            w.BorderColor = ConsoleColor.DarkGreen;
            w.Header = true;
            w.Height = 20;
            w.Width = 80;
            w.Left = 12;
            w.Top = 5;

            ConsoleWindows.Text t = new ConsoleWindows.Text(w);
            t.BackgroundColor = ConsoleColor.Black;
            t.TextPosition = ConsoleWindows.TextPosition.Center;
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("Обучение: Битва - ");
            t.ForegroundColor = ConsoleColor.Red;
            t.Write("1");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("/");
            t.ForegroundColor = ConsoleColor.Green;
            t.Write("3");
            t.AppendLine();

            t.ForegroundColor = ConsoleColor.Gray;
            t.WriteLine("Битва.");
            t.WriteLine("Для того что бы начать битву, персонаж должен атаковать врага.");
            t.WriteLine("В битве у вас будет доступно несколько команд:");
            t.ForegroundColor = ConsoleColor.Red;
            t.Write("[A] : Атака");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write(" - Персонаж будет наносить удар оружием/рукой.");
            t.AppendLine();
            t.ForegroundColor = ConsoleColor.DarkCyan;
            t.Write("[D] : Защита");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write(" - Защищаться. Защита персонажа увеличена в двое.");
            t.AppendLine();
            t.ForegroundColor = ConsoleColor.DarkGray;
            t.Write("[S] : Побег");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write(" - Попытка сбержать. Защитные хар-ки уменьшены в трое.");
            t.AppendLine();
            t.ForegroundColor = ConsoleColor.DarkMagenta;
            t.Write("[Q,W,E,R] : Навыки");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write(" - Подробне в следующем этапе обучения.");
            t.AppendLine();
            t.ForegroundColor = ConsoleColor.Gray;
            t.WriteLine("Теперь закройте окно и попробуйте сразиться с гоблином.");
            w.Text = t;

            ConsoleWindows.Button by = new ConsoleWindows.Button(w);
            by.Top = 11;
            by.Left = 17;
            by.Width = 40;
            by.Height = 5;
            by.ActiveColor = ConsoleColor.Yellow;
            by.InactiveColor = ConsoleColor.DarkGray;
            by.Label = "Перейти к битве.";
            by.CloseAfterUse = true;
            w.AddControl(by);

            w.Draw();


            Draw.Session<ClearSession>(x => x.ClearAll = true)
                .Then<GUIBorderDrawSession>()
                .Then<CharacterDataDrawSession>()
                .Then<CharMapDrawSession>(x=>x.Commands= new List<string>()
                                        {
                                            "[A] - Удар рукой",
                                            "[D] - Защищаться ",
                                            "[S] - Сбежать"
                                        })
                .Publish();

            Rogue.RAM.Enemy = DataBase.MobBase.Goblin;
            DrawEngine.FightDraw.DrawFight();
        }
        private static void prt9()
        {
            ConsoleWindows.Window w = new ConsoleWindows.Window();
            w.Animated = true;
            w.Animation = ConsoleWindows.Additional.StadartAnimation;
            w.Border = ConsoleWindows.Additional.BoldBorder;
            w.BorderColor = ConsoleColor.DarkGreen;
            w.Header = true;
            w.Height = 20;
            w.Width = 80;
            w.Left = 12;
            w.Top = 5;

            ConsoleWindows.Text t = new ConsoleWindows.Text(w);
            t.BackgroundColor = ConsoleColor.Black;
            t.TextPosition = ConsoleWindows.TextPosition.Center;
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("Обучение: Битва - ");
            t.ForegroundColor = ConsoleColor.Yellow;
            t.Write("2");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("/");
            t.ForegroundColor = ConsoleColor.Green;
            t.Write("3");
            t.AppendLine();

            t.ForegroundColor = ConsoleColor.Gray;
            t.WriteLine("Битва.");
            t.WriteLine("Итак, вы вступили в битву.");
            t.WriteLine("Закройте окно и атакуйте врага.");
            w.Text = t;

            ConsoleWindows.Button by = new ConsoleWindows.Button(w);
            by.Top = 11;
            by.Left = 17;
            by.Width = 40;
            by.Height = 5;
            by.ActiveColor = ConsoleColor.Yellow;
            by.InactiveColor = ConsoleColor.DarkGray;
            by.Label = "Закрыть.";
            by.CloseAfterUse = true;
            w.AddControl(by);

            w.Draw();

            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;

            Draw.Session<GUIBorderDrawSession>()
                .Then<CharacterDataDrawSession>()
                .Then<CharMapDrawSession>(x => x.Commands = new List<string>()
                                        {
                                            "[A] - Удар рукой",
                                            "[D] - Защищаться ",
                                            "[S] - Сбежать"
                                        })
                .Publish();                                        

            DrawEngine.FightDraw.DrawFight();

            bool end = false;
            while (!end)
            {
                ConsoleKey k = Console.ReadKey(true).Key;
                switch (k)
                {
                    case ConsoleKey.A: { PlayEngine.GamePlay.Strike(); end = true; break; }
                    default: { break; }
                }
            }
        }
        private static void prt10()
        {
            ConsoleWindows.Window w = new ConsoleWindows.Window();
            w.Animated = true;
            w.Animation = ConsoleWindows.Additional.StadartAnimation;
            w.Border = ConsoleWindows.Additional.BoldBorder;
            w.BorderColor = ConsoleColor.DarkGreen;
            w.Header = true;
            w.Height = 20;
            w.Width = 80;
            w.Left = 12;
            w.Top = 5;

            ConsoleWindows.Text t = new ConsoleWindows.Text(w);
            t.BackgroundColor = ConsoleColor.Black;
            t.TextPosition = ConsoleWindows.TextPosition.Center;
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("Обучение: Битва - ");
            t.ForegroundColor = ConsoleColor.Green;
            t.Write("3");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write("/");
            t.ForegroundColor = ConsoleColor.Green;
            t.Write("3");
            t.AppendLine();

            t.ForegroundColor = ConsoleColor.Gray;
            t.WriteLine("Битва.");
            t.WriteLine("Великолепно! Теперь вы точно знаете как атаковать.");
            t.WriteLine("Но не все классы специализируются на атаке оружием.");
            t.WriteLine("Маги и приближенные к ним классы, используют в бою навыки.");
            t.WriteLine("Для того что бы использовать навык (в бою или нет),");
            t.Write("нужно нажать одну из клавиш - ");
            t.ForegroundColor = ConsoleColor.Yellow;
            t.Write("[Q] [W] [E] [R]");
            t.AppendLine();
            t.ForegroundColor = ConsoleColor.Gray;
            t.WriteLine("Закройте окно и закончите бой используя навыки.");

            w.Text = t;

            ConsoleWindows.Button by = new ConsoleWindows.Button(w);
            by.Top = 11;
            by.Left = 17;
            by.Width = 40;
            by.Height = 5;
            by.ActiveColor = ConsoleColor.Yellow;
            by.InactiveColor = ConsoleColor.DarkGray;
            by.Label = "Закрыть.";
            by.CloseAfterUse = true;
            w.AddControl(by);

            w.Draw();

            Draw.Session<ClearSession>(x => x.ClearAll = true)
                .Then<GUIBorderDrawSession>()
                .Then<CharacterDataDrawSession>()
                .Then<CharMapDrawSession>(x=>x.Commands= new List<string>()
                                        {
                                            "[Q] - "+Rogue.RAM.Player.Ability[0].Name,
                                            "[W] - "+Rogue.RAM.Player.Ability[1].Name,
                                            "[E] - "+Rogue.RAM.Player.Ability[2].Name,
                                            "[R] - "+Rogue.RAM.Player.Ability[3].Name,
                                            "[A] - Удар рукой",
                                            "[D] - Защищаться ",
                                            "[S] - Сбежать",
                                        })
                .Publish();


            DrawEngine.FightDraw.DrawFight();
            PlayEngine.GamePlay.Fight(true, true);
        }
        private static void prt11()
        {
            PlayEngine.Enemy = false;
            ConsoleWindows.Window w = new ConsoleWindows.Window();
            w.Animated = true;
            w.Animation = ConsoleWindows.Additional.StadartAnimation;
            w.Border = ConsoleWindows.Additional.BoldBorder;
            w.BorderColor = ConsoleColor.DarkGreen;
            w.Header = true;
            w.Height = 22;
            w.Width = 82;
            w.Left = 12;
            w.Top = 5;

            ConsoleWindows.Text t = new ConsoleWindows.Text(w);
            t.BackgroundColor = ConsoleColor.Black;
            t.TextPosition = ConsoleWindows.TextPosition.Center;
            t.ForegroundColor = ConsoleColor.DarkCyan;
            t.Write("Dungeon");
            t.ForegroundColor = ConsoleColor.Red;
            t.Write(" 12");
            //t.ForegroundColor = ConsoleColor.Gray;
            //t.Write(" [");
            t.ForegroundColor = ConsoleColor.DarkMagenta;
            t.Write("YANA");
            t.AppendLine();
            t.AppendLine();
            t.ForegroundColor = ConsoleColor.Gray;
            t.WriteLine("На этом небольшой экскурс и обучение закончено.");
            t.WriteLine("В своём приключении вы встретите ещё много других,");
            t.WriteLine("увлекательных и занимательных вещей.");
            t.WriteLine("Но о них вы должны будете узнать сами.");
            t.Write("Если у вас возникнут трудности в игре, не забывайте о команде - ");
            t.ForegroundColor = ConsoleColor.Yellow;
            t.Write("[I]");
            t.ForegroundColor = ConsoleColor.Gray;
            t.Write(".");
            t.AppendLine();
            t.AppendLine();
            t.Write("Когда вы будете готовы, закройте окно и поговорите с ");
            t.ForegroundColor = ConsoleColor.Red;
            t.Write("персонажем в центре ");
            t.ForegroundColor = ConsoleColor.White;
            t.Write("(@)");
            t.ForegroundColor = ConsoleColor.Gray;
            t.WriteLine(" !");

            w.Text = t;

            ConsoleWindows.Button by = new ConsoleWindows.Button(w);
            by.Top = 12;
            by.Left = 20;
            by.Width = 40;
            by.Height = 5;
            by.ActiveColor = ConsoleColor.Yellow;
            by.InactiveColor = ConsoleColor.DarkGray;
            by.Label = "Начать играть!";
            by.CloseAfterUse = true;
            w.AddControl(by);

            w.Draw();
            
            Draw.Session<ClearSession>(x => x.ClearAll = true)
                .Then<GUIBorderDrawSession>()
                .Then<CharStatDrawSession>()
                .Publish();
        }
    }
}
