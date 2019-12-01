using Dungeon.Abilities;
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
using Dungeon.View.Interfaces;

namespace Dungeon12.Bowman
{
    public class Bowman : Dungeon12Class
    {
        public Energy Energy { get; set; } = new Energy();

        public override int InitialHP => 50;

        public override string Avatar => "archer.png".AsmImgRes();

        public override string ClassName => "Лучник";

        public override IDrawColor ClassColor => DrawColor.ForestGreen;

        public override string ResourceName => "Натяжение";

        public override string Resource => Energy.ToString();

        public override ConsoleColor ResourceColor => ConsoleColor.Yellow;

        public override string Tileset => "sprite.png".AsmImgRes();

        public SpeedShot SpeedShot { get; set; } = new SpeedShot();

        public MightShot MightShot { get; set; } = new MightShot();

        public RainOfArrows RainOfArrows { get; set; } = new RainOfArrows();

        public Dodge Dodge { get; set; } = new Dodge();
        
        public override string MainAbilityDamageText => $"Выстрел: 15";

        public override IDrawText MainAbilityDamageView => MainAbilityDamageText.AsDrawText().Montserrat().InColor(DrawColor.SandyBrown);

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

        [ClassStat("Скорость атаки", ConsoleColor.White, "Уменьшает куллдаун способностей напрямую использующих лук в соотношении 1к5. Так же незначительно увеличивает скорость полёта стрел.")]
        public long AttackSpeed { get; set; } = 1;

        [ClassStat("Дальность", ConsoleColor.DarkYellow, "Увеличивает дальность полёта стрел для ккаждых 15 едениц на 1 клетку, а так же радиус других способностей использующих стрелы в зависимости от способности.")]
        public long Range { get; set; } = 15;

        [ClassStat("Пронзание", ConsoleColor.Yellow, 1,"Позволяет игнорировать защиту врага на величину характеристики. Так же, каждые 6 едениц дают 1% шанс что обычная стрела пролетит на сквозь.")]
        public long ArmorPenetration { get; set; }

        [ClassStat("Шанс крит.", ConsoleColor.DarkGreen, 1,"Прямопропорциональный шанс нанести физический урон от способности в полтора раза больше.")]
        public long CritChance { get; set; }
    }
}