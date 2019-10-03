namespace Rogue.Drawing.SceneObjects.Main.CharacterInfo
{
    using Rogue.Classes;
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.SceneObjects.Base;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class StatsInfo : HandleSceneControl
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        private Character character;

        public StatsInfo(Character character)
        {
            this.Image = "Rogue.Resources.Images.ui.stats.png";
            this.Width = 6;
            this.Height = 16;

            this.character = character;

            this.DrawStats(character);
        }

        public override double Height
        {
            get
            {
                //this.DrawStats(this.character);
                return base.Height;
            }
            set => base.Height = value;
        }

        public override string Image
        {
            get
            {
                Refresh();

                return base.Image;
            }
            set => base.Image = value;
        }

        private Dictionary<TextControl, Func<Character, string>> statsText = new Dictionary<TextControl, Func<Character, string>>();

        private void Refresh()
        {
            foreach (var stat in statsText)
            {
                stat.Key.Text.SetText(stat.Value?.Invoke(character));
            }
        }

        private void DrawStats(Character @char)
        {
            double top = 0.2;

            var common = this.AddTextCenter(new DrawText("Основные"), true, false);
            common.Top += top;
            top += MeasureText(common.Text).Y/32+0.5;

            this.AddChild(new DarkRectangle() { Color = ConsoleColor.Black, Opacity = 1, Left = 0.5, Width = this.Width - 1, Height = 0.1, Top = top - 0.25 });

            foreach (var stat in BaseStats)
            {
                var text = new DrawText($"{stat.Title(@char)}:  {stat.Value(@char)}", new DrawColor(stat.Color(@char))).Montserrat();
                TextControl txt = null;
                if (MeasureText(text).X / 32 > this.Width)
                {
                    var statTitle = new DrawText($"{stat.Title(@char)}:", new DrawColor(stat.Color(@char))).Montserrat();
                    var statTxt = this.AddTextCenter(statTitle);
                    statTxt.Left = 0.5;
                    statTxt.Top = top;

                    top += MeasureText(text).Y / 32 + (stat.EndGroup ? 0.3 : 0);

                    text = new DrawText($"{stat.Value(@char)}", new DrawColor(stat.Color(@char))).Montserrat();

                    txt = this.AddTextCenter(text);
                    txt.Left = 0.5;
                    txt.Top = top;

                    statsText.Add(txt, c => $"{stat.Value(c)}");
                }
                else
                {
                    txt = this.AddTextCenter(text);
                    txt.Left = 0.5;
                    txt.Top = top;
                    statsText.Add(txt, c => $"{stat.Title(c)}:  {stat.Value(c)}");
                }

                top += MeasureText(txt.Text).Y / 32 + (stat.EndGroup ? 0.3 : 0);

                if (stat.EndGroup)
                {
                    this.AddChild(new DarkRectangle() { Color = ConsoleColor.Black, Opacity = 1, Left = 0.5, Width = this.Width - 1, Height = 0.1, Top = top - 0.25 });
                }
            }

            top += 0.3;

            var classed = this.AddTextCenter(new DrawText("Классовые"), true, false);
            classed.Top += top;
            top += MeasureText(classed.Text).Y / 32 + 0.5;
            this.AddChild(new DarkRectangle() { Color = ConsoleColor.Black, Opacity = 1, Left = 0.5, Width = this.Width - 1, Height = 0.1, Top = top - 0.25 });

            foreach (var group in @char.ClassStats.GroupBy(cs => cs.Group))
            {
                foreach (var cs in group)
                {
                    var txt = this.AddTextCenter(new DrawText($"{cs.Title}:  {cs.Value}", cs.Color).Montserrat());
                    txt.Left = 0.5;
                    txt.Top = top;

                    statsText.Add(txt, c => $"{cs.Title}:  {cs.Value}");

                    top += MeasureText(txt.Text).Y / 32;
                }
                top += 0.5;
                this.AddChild(new DarkRectangle() { Color = ConsoleColor.Black, Opacity = 1, Left = 0.5, Width = this.Width - 1, Height = 0.1, Top = top - 0.25 });
            }
        }

        private List<StatDrawData> BaseStats = new List<StatDrawData>()
        {
            new StatDrawData(c=>"Здоровье",c=>$"{c.HitPoints}/{c.MaxHitPoints}",c=>ConsoleColor.Red,false),

            new StatDrawData(c=>c.ResourceName,c=>c.Resource,c=>c.ResourceColor,true),

            new StatDrawData(c=>"Урон",c=>$"{c.MinDMG} - {c.MaxDMG}",c=>ConsoleColor.DarkYellow,true),

            new StatDrawData(c=>"Сила атаки",c=>$"{c.AttackPower}",c=>ConsoleColor.Cyan,false),

            new StatDrawData(c=>"Сила магии",c=>$"{c.AbilityPower}",c=>ConsoleColor.Magenta,true),

            new StatDrawData(c=>"Защита",c=>$"{c.Defence}",c=>ConsoleColor.DarkCyan,false),

            new StatDrawData(c=>"Барьер",c=>$"{c.Barrier}",c=>ConsoleColor.DarkMagenta,true),
        };

        public class StatDrawData
        {
            public StatDrawData(Func<Character, string> title, Func<Character, string> formula, Func<Character, ConsoleColor> color, bool group)
            {
                Title = title;
                Value = formula;
                Color = color;
                EndGroup = group;
            }

            public Func<Character,string> Title { get; set; }

            public Func<Character, string> Value { get; set; }

            public Func<Character, ConsoleColor> Color { get; set; }

            public bool EndGroup { get; set; }

        }
    }
}
