using Dungeon;
using Dungeon.Drawing;
using Dungeon.View.Interfaces;
using Dungeon12.Abilities;
using Dungeon12.Abilities.Talants.TalantTrees;
using Dungeon12.Classes;
using Dungeon12.Servant.Abilities;
using Dungeon12.Servant.Talants;
using System;

namespace Dungeon12.Servant
{
    public class Servant : Character
    {
        public Servant(bool @new) : base(true) { }
        public Servant() { }

        public bool Serve { get; set; }

        public override double HitPointsPercentPlus => 8;

        public FaithPower FaithPower { get; set; } = new FaithPower();

        public override string Avatar => "servant.png".AsmImgRes();

        public override string ClassName => "Слуга веры";

        public override IDrawColor ClassColor => DrawColor.SlateBlue;

        public override string ResourceName => "Сила веры";

        public override string Resource => FaithPower.ToString();

        public override ConsoleColor ResourceColor => ConsoleColor.Yellow;

        public override string Tileset => "sprite.png".AsmImgRes();

        public FaithShot FaithShot { get; set; } = new FaithShot();

        public Prayer Prayer { get; set; } = new Prayer();

        public Heal Heal { get; set; } = new Heal();

        public Сonsecration Сonsecration { get; set; } = new Сonsecration();

        public PowerTalants PowerTalants { get; set; } = new PowerTalants();

        public FaithTalants FaithTalants { get; set; } = new FaithTalants();

        public override T[] PropertiesOfType<T>()
        {
            switch (typeof(T))
            {
                case Type t when t.IsAssignableFrom(typeof(Ability)):
                    return new T[]
                    {
                        FaithShot as T,
                        Heal as T,
                        Prayer as T,
                        Сonsecration as T
                    };
                case Type t when t.IsAssignableFrom(typeof(TalantTree)):
                    return new T[] { PowerTalants as T, FaithTalants as T };
                default: return default;
            }
        }

        public override T PropertyOfType<T>()
        {
            switch (typeof(T))
            {
                case Type abs when abs == typeof(PowerTalants): return PowerTalants as T;
                default: return default;
            }
        }

        public override string MainAbilityDamageText => $"Лечение: {Heal.ScaledValue(this) + HealPower} | Урон: {FaithShot.ScaledValue(this) + DamagePower}";

        public override IDrawText MainAbilityDamageView => MainAbilityDamageText.AsDrawText().Montserrat().InColor(DrawColor.Yellow);

        [ClassStat("Лечение", ConsoleColor.Yellow, "Все исцеляющие заклинания или имеющие компонент исцеления в способности исцеляют больше на величину характеристики.")]
        public long HealPower { get; set; }

        [ClassStat("Урон", ConsoleColor.Blue, 1, "К наносимому урону добавляется количество характеристики, а так же, к физическому урону добавляется магический урон равный проценту урона от нанесённого.")]

        public long DamagePower { get; set; }
    }
}