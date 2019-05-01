using Rogue.DataAccess;
using Rogue.Physics;
using Rogue.Settings;
using Rogue.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Rogue.Map
{
    public partial class GameMap
    {
        private bool g = false;

        public void Load(string identity)
        {
            if (identity != "Capital")
            {

                if (!g)
                {
                    Generate();
                    g = true;
                    return;
                }

                g = false;
            }

            var persistMap = Database.Entity<Data.Maps.Map>(e => e.Identity == identity).First();

            this.Name = persistMap.Name;

            int x = 0;
            int y = 0;

            var template = persistMap.Template.Trim();

            if (persistMap.Procedural)
            {
                template = ProcedureGeneration(template);
                return;
            }

            foreach (var line in template.Replace("\r","").Split('\n'))
            {
                var listLine = new List<List<Map.MapObject>>();

                x = 0;

                foreach (var @char in line)
                {
                    var mapObj = MapObject.Create(@char.ToString());
                    mapObj.Location = new Point(x, y);
                    mapObj.Region = new Rectangle
                    {
                        Height = 32,
                        Width = 32,
                        Pos = mapObj.Location
                    };

                    if (mapObj.Obstruction)
                    {
                        var nodes = this.Map.Query(mapObj, true);

                        if (nodes.Count == 0)
                        {
                            Debugger.Break();
                        }

                        foreach (var node in nodes)
                        {
                            node.Nodes.Add(mapObj);
                        }
                    }

                    listLine.Add(new List<MapObject>() { mapObj });
                    x++;
                }

                y++;

                this.MapOld.Add(listLine);
            }
        }

        private string ProcedureGeneration(string template)
        {
            string result = string.Empty;

            var max_x = DrawingSize.Chars;
            var max_y = DrawingSize.Lines;

            StringBuilder[][] Maz = new StringBuilder[max_x][];

            for (int i = 0; i < max_x; i++)
                Maz[i] = new StringBuilder[max_y];

            var entryCoordinate = ReadTemplate(template, ref Maz);

            generate(ref Maz, max_x, max_y, entryCoordinate);

            for (int y = 0; y < max_y; y++)
            {
                for (int x = 0; x < max_x; x++)
                {
                    result += Maz[x][y].ToString();
                }

                result += Environment.NewLine;
            }

            Console.WriteLine(result.Replace("."," "));

            return result;
        }

        private static TCoord ReadTemplate(string template, ref StringBuilder[][] Maz)
        {
            int y = 0;
            int x = 0;

            TCoord player = new TCoord();

            //var stream = new MemoryStream();
            //var writer = new StreamWriter(stream);
            //writer.Write(template);
            //writer.Flush();
            //stream.Position = 0;

            //StreamReader sr = new StreamReader(stream, Encoding.Default);
            //string str = new string(new char[256]);
            //char c;
            //// коорди

            //do
            //{
            //    c = (char)sr.Read();
            //    if (x == DrawingSize.Lines)
            //    { y++; x = 0; }
            //    if (c != '\n')
            //    {
            //        if (c == '3')
            //        {
            //            player.x = x;
            //            player.y = y;
            //        }
            //        Maz[x][y] = new StringBuilder(c.ToString());
            //        x++;
            //    }

            //} while ((!sr.EndOfStream) && (y < DrawingSize.Chars));

            foreach (var line in template.Split(Environment.NewLine))
            {
                foreach (var charTemplate in line)
                {
                    if (charTemplate == '3')
                    {
                        player.x = x;
                        player.y = y;
                    }

                    Maz[x][y] = new StringBuilder(charTemplate.ToString());
                    x++;
                }
                x = 0;
                y++;
            }

            return player;
        }

        #region C++ legacy              

        //class myClass : ICloneable
        //{
        //    public String test;
        //    public object Clone()
        //    {
        //        return this.MemberwiseClone();
        //    }
        //}

        // *** описание списка: конец
        // *** проверка допустимости пробива поля: начало
        private static bool valid_proboi(char c)
        {
            if ((c == '2') || (c == '0'))
                return true;
            else
                return false;
        }
        // *** проверка допустимости пробива поля: конец
        // *** проверка соседнего с пробивом поля: начало
        private static bool valid_sosed(char c)
        {
            if ((c == '1') || (c == '2') || (c == '0') || (c == '#'))
                return true;
            else
                return false;
        }
        // *** проверка соседнего с пробивом поля: конец
        // *** заполняем комнату: начало
        private static void fill_room(ref StringBuilder[][] Maz, int x, int y, TList list)
        {
            // заполнять будем по рекурсивному алгоритму:
            Maz[x][y] = new StringBuilder(".");
            // и добавлять заполненные узловые точки в список list:
            if ((x % 2 == 1) && (y % 2 == 1))
                list.add_node(x, y);
            // а расширяться будем во все стороны где нули:
            // север:
            if (Maz[x][y - 1].ToString() == "0")
            {
                fill_room(ref Maz, x, y - 1, list);
            }
            // запад:
            if (Maz[x - 1][y].ToString() == "0")
            {
                fill_room(ref Maz, x - 1, y, list);
            }
            // юг:
            if (Maz[x][y + 1].ToString() == "0")
            {
                fill_room(ref Maz, x, y + 1, list);
            }
            // восток:
            if (Maz[x + 1][y].ToString() == "0")
            {
                fill_room(ref Maz, x + 1, y, list);
            }
        }
        // *** заполняем комнату: конец
        // *** заполняем массив стенами "#" и пустотами ".": начало
        private static void generate(ref StringBuilder[][] Maz, int max_x, int max_y, TCoord first_field)
        {
            // создаём список:
            TList list = new TList();
            // вспомогательные переменные:
            TCoord coord = new TCoord(); // для координат
            string dir = new string(new char[4]); // для направлений с з ю в
            int r;
            int i;
            // ----------------------------------------------------------------------------------------
            // ------------- весь алгоритм генерации- здесь: ------------------------------------------
            // ----------------------------------------------------------------------------------------
            // выбираем начальное поле:
            coord.x = first_field.x;
            coord.y = first_field.y;
            Maz[coord.x][coord.y] = new StringBuilder("@");
            // заносим поле в список:
            list.add_node(coord.x, coord.y);
            // основной цикл генерации:
            do
            {
                // выбираем наугад поле из списка:
                r = Random.Next() % list.nodes_num;
                coord = list.get_node(r);
                // считаем направления,
                // куда с этого поля можно расшириться:
                i = 0;
                // север:
                if ((coord.y > 2) && (Maz[coord.x][coord.y - 2].ToString() == "2") || (coord.y > 2) && (Maz[coord.x][coord.y - 2].ToString() == "0"))
                {
                    dir = StringHelper.ChangeCharacter(dir, i, 'с');
                    i++;
                }
                // запад:
                if ((coord.x > 2) && (Maz[coord.x - 2][coord.y].ToString() == "2") || (coord.x > 2) && (Maz[coord.x - 2][coord.y].ToString() == "0"))
                {
                    dir = StringHelper.ChangeCharacter(dir, i, 'з');
                    i++;
                }
                // юг:
                if ((coord.y < (max_y - 3)) && (Maz[coord.x][coord.y + 2].ToString() == "2") || (coord.y < (max_y - 3)) && (Maz[coord.x][coord.y + 2].ToString() == "0"))
                {
                    dir = StringHelper.ChangeCharacter(dir, i, 'ю');
                    i++;
                }
                // восток:
                if ((coord.x < (max_x - 3)) && (Maz[coord.x + 2][coord.y].ToString() == "2") || (coord.x < (max_x - 3)) && (Maz[coord.x + 2][coord.y].ToString() == "0"))
                {
                    dir = StringHelper.ChangeCharacter(dir, i, 'в');
                    i++;
                }
                // если некуда расширяться,
                //то выкидываем выбранное поле из списка:
                if (i == 0)
                {
                    list.del_node(r, list);
                }
                else
                {
                    // иначе- расширяемся куда-нибудь:
                    i = Random.Next() % i;
                    if (dir[i] == 'с')
                    {
                        Maz[coord.x][coord.y - 1] = new StringBuilder(".");
                        if (Maz[coord.x][coord.y - 2].ToString() == "0")
                        {
                            // заполняем комнату
                            fill_room(ref Maz, coord.x, coord.y - 2, list);
                            // отмечаем, что заносить новое поле в список уже не надо:
                            coord.x = -1;
                        }
                        else
                        {
                            // пробиваемся на север:
                            Maz[coord.x][coord.y - 2] = new StringBuilder(".");
                            // координаты нового поля:
                            coord.y = coord.y - 2;
                        }
                    }
                    if (dir[i] == 'з')
                    {
                        Maz[coord.x - 1][coord.y] = new StringBuilder(".");
                        if (Maz[coord.x - 2][coord.y].ToString() == "0")
                        {
                            // заполняем комнату
                            fill_room(ref Maz, coord.x - 2, coord.y, list);
                            // отмечаем, что заносить новое поле в список уже не надо:
                            coord.x = -1;
                        }
                        else
                        {
                            // пробиваемся на запад:
                            Maz[coord.x - 2][coord.y] = new StringBuilder(".");
                            // координаты нового поля:
                            coord.x = coord.x - 2;
                        }
                    }
                    if (dir[i] == 'ю')
                    {
                        Maz[coord.x][coord.y + 1] = new StringBuilder(".");
                        if (Maz[coord.x][coord.y + 2].ToString() == "0")
                        {
                            // заполняем комнату
                            fill_room(ref Maz, coord.x, coord.y + 2, list);
                            // отмечаем, что заносить новое поле в список уже не надо:
                            coord.x = -1;
                        }
                        else
                        {
                            // пробиваемся на юг:
                            Maz[coord.x][coord.y + 2] = new StringBuilder(".");
                            // координаты нового поля:
                            coord.y = coord.y + 2;
                        }
                    }
                    if (dir[i] == 'в')
                    {
                        Maz[coord.x + 1][coord.y] = new StringBuilder(".");
                        if (Maz[coord.x + 2][coord.y].ToString() == "0")
                        {
                            // заполняем комнату
                            fill_room(ref Maz, coord.x + 2, coord.y, list);
                            // отмечаем, что заносить новое поле в список уже не надо:
                            coord.x = -1;
                        }
                        else
                        {
                            // пробиваемся на восток:
                            Maz[coord.x + 2][coord.y] = new StringBuilder(".");
                            // координаты нового поля:
                            coord.x = coord.x + 2;
                        }
                    }
                    // и заносим новое поле в список:
                    if (coord.x != -1)
                        list.add_node(coord.x, coord.y);
                }
                // крутим основной цикл, пока список не пуст:
            } while (list.head != null);
            // ----------------------------------------------------------------------------------------
            // ----------------------------------------------------------------------------------------
            // ----------------------------------------------------------------------------------------
            // лабиринт готов, но нужно ещё заменить оставшиеся цифры стенами:
            for (int y = 0; y < max_y; y++)
            {
                for (int x = 0; x < max_x; x++)
                {
                    if ((Maz[x][y].ToString() == "1") || (Maz[x][y].ToString() == "2"))
                        Maz[x][y] = new StringBuilder("#");
                }
            }
        }
        // *** заполняем массив стенами "#" и пустотами ".": конец

        // описываем структуру для хранения входных данных: конец
        // *** описание списка: начало
        // координаты на плоскости:
        private class TCoord
        {
            public int x;
            public int y;
        }
        // элемент списка:
        private class node : ICloneable
        {
            public TCoord coord = new TCoord(); // каждый элемент хранит координаты соответствующего поля лабиринта
            public node next; // и указатель на следующий элемент
            public object Clone()
            {
                return this.MemberwiseClone();
            }
        }
        // список:
        private class TList
        {
            public node head; // указатели на начало списка и на конец
            public node tail;
            public int nodes_num; // количество элементов списка
            public TList() // инициализация указателей как пустых
            {
                head = null;
                tail = null;
                nodes_num = 0;
            }
            // * деструктор для освобождения памяти от списка: начало
            public void Dispose()
            {
                node temp = head; // временный указатель на начало списка
                while (temp != null) // пока в списке что-то есть
                {
                    temp = head.next; // резерв адреса на следующий элемент списка
                    head = null; // освобождение памяти от первой структуры как элемента списка
                    head = temp; // сдвиг начала на следующий адрес, который берем из резерва
                }
            }
            // * деструктор для освобождения памяти от списка: конец
            // * добавление элемента в список: начало
            public void add_node(int x, int y) // функция добавления элемента
            {
                node temp = new node(); //Выделение памяти для нового звена списка
                temp.coord.x = x; // временное запоминание принятых параметров
                temp.coord.y = y;
                temp.next = null; // следующее звено новосозданной структуры пока пустое
                if (head != null) // если список не пуст
                {
                    tail.next = temp; // следующее звено списка это новосозданная структура
                    tail = temp; // и теперь хвост указывает на новое звено
                }
                else
                    head = tail = temp; // если список пуст, добавление первого элемента
                                        // наращиваем количество элементов в списке:
                nodes_num++;
            }
            // * добавление элемента в список: конец
            // * выбор элемента по номеру в списке: начало
            public TCoord get_node(int n)
            {
                // заводим вспомогательные переменные:
                //C++ TO C# CONVERTER TODO TASK: Pointer arithmetic is detected on this variable, so pointers on this variable are left unchanged.
                node pnode = new node();
                int i;
                TCoord coord = new TCoord();
                // становимся на нулевой элемент списка:
                //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a "CopyFrom" method should be created if it does not yet exist:
                //ORIGINAL LINE: pnode = head;
                //pnode = new node(head);
                pnode = (node)head.Clone();
                i = 0;
                // перебираем список, пока не дойдём до нужного элемента:
                while ((pnode != null) && (i < n))
                {
                    //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a "CopyFrom" method should be created if it does not yet exist:
                    //ORIGINAL LINE: pnode = pnode->next;
                    pnode = (node)pnode.next.Clone();
                    //pnode. //CopyFrom(pnode.next);
                    i++;
                }
                // если нужный элемент существует:
                if (pnode != null)
                {
                    // берём данные из нужного элемента:
                    coord.x = pnode.coord.x;
                    coord.y = pnode.coord.y;
                }
                else // иначе
                {
                    // подставляем (-1,-1) как код ошибки:
                    coord.x = -1;
                    coord.y = -1;
                    // но вообще такого быть не должно-
                    // мы будем запрашивать только корректные номера
                }
                // возвращаем координаты элемента:
                return coord;
            }
            // * выбор элемента по номеру в списке: конец
            // * удаление элемента из списка по его номеру: начало
            public void del_node(int n, TList list)
            {
                // заводим вспомогательные переменные:
                node curr_node;
                node prev_node;
                int i;
                // если нужно удалить нулевой элемент,
                if (n == 0)
                {
                    // то просто удаляем его:
                    curr_node = head.next;
                    // если этот элемент был последним,
                    // то надо поправить хвост:
                    if (tail == head)
                        tail = null;

                    head = null;
                    // поправляем голову списка:
                    head = curr_node;
                    // уменьшаем количество элементов в списке:
                    nodes_num--;
                    // конец функции:
                    return;
                }
                // становимся на нулевой элемент списка:
                curr_node = head;
                i = 0;
                do
                {
                    // перебираем список:
                    prev_node = curr_node;
                    curr_node = curr_node.next;
                    i++;
                    // пока не дойдём до нужного элемента:
                } while ((curr_node != null) && (i < n));
                // если нужный элемент существует:
                if (curr_node != null)
                {
                    // если он последний, то поправляем хвост списка:
                    if (curr_node == tail)
                        tail = prev_node;
                    // предыдущий элемент должен теперь указывать на следующий:
                    prev_node.next = curr_node.next;
                    // удаляем:
                    curr_node = null;
                    // уменьшаем количество элементов в списке:
                    nodes_num--;
                }
                return;
            }
            // * удаление элемента из списка по его номеру: конец
        }

        internal static class StringHelper
        {
            //------------------------------------------------------------------------------------
            //	This method allows replacing a single character in a string, to help convert
            //	C++ code where a single character in a character array is replaced.
            //------------------------------------------------------------------------------------
            internal static string ChangeCharacter(string sourcestring, int charindex, char changechar)
            {
                return (charindex > 0 ? sourcestring.Substring(0, charindex) : "")
                    + changechar.ToString() + (charindex < sourcestring.Length - 1 ? sourcestring.Substring(charindex + 1) : "");
            }
        }

        #endregion
    }
}
