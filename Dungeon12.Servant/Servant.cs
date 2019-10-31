using Rogue.Abilities;
using Rogue.Abilities.Talants.TalantTrees;
using Rogue.Classes.Servant.Abilities;
using Rogue.Classes.Servant.Talants;
using Rogue.Drawing.Impl;
using System;
using System.Collections.Generic;

namespace Dungeon12.Classes.Servant
{
    public class Servant : BaseCharacterTileset
    {
        public bool Serve { get; set; }

        public FaithPower FaithPower { get; set; } = new FaithPower();

        public override string Avatar => "servant.png".ImgPath();

        public override string ClassName => "Слуга веры";

        public override string ResourceName => "Сила веры";

        public override string Resource => FaithPower.ToString();

        public override ConsoleColor ResourceColor => ConsoleColor.Yellow;

        public override string Tileset => "sprite.png".ImgPath();

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

        //public double AtackSpeed { get; set; } = 1;

        //public double Range { get; set; } = 3;

        //public double ArmorPenetration { get; set; }

        //public int CritChance { get; set; }

        //public override IEnumerable<ClassStat> ClassStats => new ClassStat[]
        //{
        //    new ClassStat("Скорость атаки",this.AtackSpeed.ToString(), new DrawColor(ConsoleColor.White)),
        //    new ClassStat("Дальность",$"{this.Range}%", new DrawColor(ConsoleColor.DarkCyan)),
        //    new ClassStat("Шанс пробить", $"{this.ArmorPenetration}",new DrawColor(ConsoleColor.Yellow)){  Group=1},
        //    new ClassStat("Шанс крит.", $"{this.CritChance}%",new DrawColor(ConsoleColor.DarkRed)){  Group=1},
        //};
    }
}