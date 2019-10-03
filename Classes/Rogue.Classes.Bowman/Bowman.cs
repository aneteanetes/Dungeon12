using Rogue.Abilities;
using Rogue.Abilities.Talants.TalantTrees;
using Rogue.Classes.Bowman.Abilities;
using Rogue.Drawing.Impl;
using System;
using System.Collections.Generic;

namespace Rogue.Classes.Bowman
{
    public class Bowman : BaseCharacterTileset
    {
        public Energy Energy { get; set; } = new Energy();

        public override string Avatar => "Rogue.Classes.Bowman.Images.archer.png";

        public override string ClassName => "Лучник";

        public override string ResourceName => "Натяжение";

        public override string Resource => Energy.ToString();

        public override ConsoleColor ResourceColor => ConsoleColor.Yellow;

        public override string Tileset => "Rogue.Classes.Bowman.Images.sprite.png";

        public SpeedShot SpeedShot { get; set; } = new SpeedShot();

        public MightShot MightShot { get; set; } = new MightShot();

        public RainOfArrows RainOfArrows { get; set; } = new RainOfArrows();

        public JumpOff JumpOff { get; set; } = new JumpOff();

        public override T[] PropertiesOfType<T>()
        {
            switch (typeof(T))
            {
                case Type t when t.IsAssignableFrom(typeof(Ability)):
                    return new T[]
                    {
                        SpeedShot as T,
                        MightShot as T,
                        RainOfArrows as T,
                        JumpOff as T
                    };
                case Type t when t.IsAssignableFrom(typeof(TalantTree)):
                    return new T[] { };
                default: return default;
            }
        }

        public double AtackSpeed { get; set; }

        public double Range { get; set; }

        public double ArmorPenetration { get; set; }

        public int CritChance { get; set; }

        public override IEnumerable<ClassStat> ClassStats => new ClassStat[]
        {
            new ClassStat("Скорость атаки",this.AtackSpeed.ToString(), new DrawColor(ConsoleColor.White)),
            new ClassStat("Дальность",$"{this.Range}%", new DrawColor(ConsoleColor.DarkCyan)),
            new ClassStat("Шанс пробить", $"{this.ArmorPenetration}",new DrawColor(ConsoleColor.Yellow)){  Group=1},
            new ClassStat("Шанс крит.", $"{this.CritChance}%",new DrawColor(ConsoleColor.DarkRed)){  Group=1},
        };
    }
}