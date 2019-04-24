using System;
using Rogue.Drawing.GUI;
using Rogue.Drawing.Impl;
using Rogue.Entites.Alive.Character;
using Rogue.Entites.Items;
using Rogue.Settings;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Data
{
    public class CharacterDataDrawSession : DrawSession
    {
        public DrawingSize DrawingSize { get; set; } = new DrawingSize();

        public CharacterDataDrawSession()
        {
            this.AutoClear = false;
            this.DrawRegion = new Types.Rectangle
            {
                X = DrawingSize.MapChars + 2f,
                Y = 1.3f,
                Width = DrawingSize.WindowChars - DrawingSize.MapChars - 1.7f,
                Height = DrawingSize.MapLines + 1.4f
            };
        }

        public Player Player { get; set; }

        public HotPanel HotPanel { get; set; }

        public override IDrawSession Run()
        {
            new CharacterWindow()
            {
                Left = DrawingSize.MapChars + 1.9f,
                Top = 1.3f,
                Width = DrawingSize.WindowChars - DrawingSize.MapChars - 1.7f,
                Height = DrawingSize.MapLines + 1.4f

            }.Run().Publish();

            var top = 1.7f;

            Text(Player.Name, 40, 14, ConsoleColor.Black, top += 1);

            Text("LVL:" + Player.Level.ToString() + " EXP:" + Player.EXP.ToString() + "/" + Player.MaxExp.ToString(),
                24, 10.5f, ConsoleColor.Black, top += 2);

            Text("HP: " + Player.MaxHitPoints.ToString() + "/" + Player.MaxHitPoints.ToString(),
                24, 10.5f, ConsoleColor.Red, top += 2);

            Text($"{Player.ResourceName}: {Player.Resource}",
                24, 10.5f, Player.ResourceColor, top += 1);

            Text(" Урон: " + Player.MinDMG.ToString() + "-" + Player.MaxDMG.ToString(),
                24, 10.5f, ConsoleColor.Black, top += 2);

            Text("Сила атаки: " + Player.AttackPower.ToString(),
                24, 10.5f, ConsoleColor.DarkRed, top += 2);

            Text("Сила магии: " + Player.AbilityPower.ToString(),
                24, 10.5f, ConsoleColor.DarkCyan, top += 1);

            Text("Защита Физ: " + Player.Defence.ToString(),
                24, 10.5f, ConsoleColor.DarkGreen, top += 2);

            Text("Защита Маг: " + Player.Barrier.ToString(),
                24, 10.5f, ConsoleColor.DarkMagenta, top += 1);

            Text("       $  ",
                50, 12f, ConsoleColor.Blue, top += 3);
            Text("          " + Player.Gold.ToString(),
                35, 12f, ConsoleColor.Blue, top-=0.2f);
            
            return this;
        }

        private void Text(string data, float size, float letter, ConsoleColor color,float top)
        {
            double positionInLine = PositionInLine(data, 14);
            this.WanderingText.Add(new DrawText(data, color, positionInLine, top) { Size = size, LetterSpacing = letter });
        }

        private double PositionInLine(string data, float size)
        {
            var pos = (this.DrawRegion.Width * 24 / 2) - (Player.Name.Length * size / 2);
            return pos / 24 
                + this.DrawRegion.X;
        }
    }
}