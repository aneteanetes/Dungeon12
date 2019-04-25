using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.Impl;
using Rogue.Map;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.WorldMap
{
    public class WorldMapHitDrawSession : DrawSession
    {
        public IView<Biom> BiomView { get; set; }

        public GameMap Map { get; set; }

        public WorldMapHitDrawSession()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = 75,
                Y = 1,
                Width = 23,
                Height = 17
            };
        }

        public override IDrawSession Run()
        {
            var height = 24;

            var color = BiomView.GetView().ForegroundColor;

            //name
            int Count = (23 / 2) - ((Map._Name.Length) / 2);
            string WriteThis = Map._Name;
            this.Write(1, Count, new DrawText(WriteThis, color));

            //name
            Count = (23 / 2) - ((Map._Affics.Length) / 2);
            WriteThis = Map._Affics;
            this.Write(2, Count, new DrawText(WriteThis, color));

            //objects
            Count = (23 / 2) - (("Объекты:".Length) / 2);
            WriteThis = "Объекты:";
            this.Write(4, Count, new DrawText(WriteThis, color));

            //door               
            Count = (23 / 2) - (("▒  -  Обычная дверь".Length) / 2);
            WriteThis = "`" + ConsoleColor.Gray.ToString() + "`▒ - Обычная дверь";
            this.Write(6, Count, new DrawText(WriteThis, color));

            //magic door
            Count = (23 / 2) - (("? - Волшебная дверь".Length) / 2);
            WriteThis = "`" + ConsoleColor.DarkMagenta.ToString() + "`░ - Волшебная дверь";
            this.Write(7, Count, new DrawText(WriteThis, color));

            //wall               
            Count = (23 / 2) - (("#   -   Препятствие".Length) / 2);
            WriteThis = "# - Препятствие";
            this.Write(8, Count, new DrawText(WriteThis, color));

            //merch              
            Count = (23 / 2) - (("$     -    Торговец".Length) / 2);
            WriteThis = "`" + ConsoleColor.Yellow.ToString() + "`$ - Торговец";
            this.Write(9, Count, new DrawText(WriteThis, color));

            //quest              
            //Count = (23 / 2) - (("!     -     Задание".Length) / 2);
            //WriteThis = "`" + ConsoleColor.Red.ToString() + "`! - Задание";
            //winAPIDraw.DrawLeftWindow.AddLine(Count, 10, WriteThis, Map.Biom);

            //objects
            Count = (23 / 2) - (("Жители:".Length) / 2);
            WriteThis = "Жители:";
            this.Write(12, Count, new DrawText(WriteThis, color));

            int q = 14;

            //mobs here - ненужная хуйня с патчем введётся
            //foreach (SystemEngine.Helper.Information.Mob m in SystemEngine.Helper.Information.MobsHere())
            //{
            //    Count = (23 / 2) - (("? - " + m.Name).Length / 2);
            //    WriteThis = "`" + m.Color.ToString() + "`" + m.Icon + " - " + m.Name;
            //    this.Write(q, Count, new DrawText(WriteThis, color));
            //    q++;
            //}


            //DrawEngine.CharMap.DrawCMap(new List<string>()
            //                    {
            //                        "[O] - Действие",
            //                        "[M] - Карта ",
            //                        "[T] - Взять вещь ",
            //                        "[A] - Атаковать ",
            //                        "[С] - Персонаж ",
            //                        "[I] - Идентификация ",
            //                        "[1-6] - Инвентарь ",
            //                        "[Q,W,E,R],+[Shift] - Навыки",
            //                    });


            return base.Run();
        }
    }
}
