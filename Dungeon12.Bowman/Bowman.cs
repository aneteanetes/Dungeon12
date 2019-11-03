﻿using Dungeon.Abilities;
using Dungeon.Abilities.Talants.TalantTrees;
using Dungeon12;
using Dungeon12.Bowman.Talants;
using Dungeon12.Bowman.Abilities;
using System;
using System.Collections.Generic;
using Dungeon12.Bowman;
using Dungeon.Classes;
using Dungeon;
using Dungeon.Drawing;

namespace Dungeon12.Bowman
{
    public class Bowman : Dungeon12Class
    {
        public Energy Energy { get; set; } = new Energy();

        public override string Avatar => "archer.png".AsmImgRes();

        public override string ClassName => "Лучник";

        public override string ResourceName => "Натяжение";

        public override string Resource => Energy.ToString();

        public override ConsoleColor ResourceColor => ConsoleColor.Yellow;

        public override string Tileset => "sprite.png".AsmImgRes();

        public SpeedShot SpeedShot { get; set; } = new SpeedShot();

        public MightShot MightShot { get; set; } = new MightShot();

        public RainOfArrows RainOfArrows { get; set; } = new RainOfArrows();

        public Dodge Dodge { get; set; } = new Dodge();

        public ArrowMakingTalants ArrowMakingTalants { get; set; } = new ArrowMakingTalants();

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
                        Dodge as T
                    };
                case Type t when t.IsAssignableFrom(typeof(TalantTree)):
                    return new T[] { ArrowMakingTalants as T };
                default: return default;
            }
        }

        public override T PropertyOfType<T>()
        {
            switch (typeof(T))
            {
                case Type abs when abs == typeof(ArrowMakingTalants): return ArrowMakingTalants as T;
                default: return default;
            }
        }

        public double AtackSpeed { get; set; } = 1;

        public double Range { get; set; } = 3;

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