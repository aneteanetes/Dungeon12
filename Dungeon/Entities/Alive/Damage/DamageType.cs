using Dungeon.Data;
using Dungeon.Drawing;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon.Entities.Alive
{
    public class DamageType : Persist
    {
        public Elements Element { get; set; }

        public string Name { get; set; }

        public string Display { get; set; }

        public string Description { get; set; }

        public double MagicRate { get; set; }

        public double PhysicRate { get; set; }

        public string Image { get; set; }

        public PersistColor Color { get; set; }

        private static bool initialized;

        private static Dictionary<string, DamageType> DamageTypeCache = new Dictionary<string, DamageType>();

        public static DamageType Make(string damageTypeName)
        {
            if (!initialized)
            {
                Database.Entity<DamageType>().ToList().ForEach(dt => DamageTypeCache[dt.Name] = dt);
                initialized = true;
            }

            return DamageTypeCache[damageTypeName];
        }

        public static bool operator ==(DamageType a, DamageType b)
        {
            return a.Name.Equals(b.Name);
        }

        public static bool operator !=(DamageType a, DamageType b)
        {
            return !a.Name.Equals(b.Name);
        }

        public override bool Equals(object obj)
        {
            if(obj is DamageType b)
            {
                return this == b;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static DamageType Physical = Make(nameof(Physical));
        public static DamageType Kenetic = Make(nameof(Kenetic));
        public static DamageType Fire = Make(nameof(Fire));
        public static DamageType Cold = Make(nameof(Cold));
        public static DamageType Magical = Make(nameof(Magical));
        public static DamageType HolyMagic = Make(nameof(HolyMagic));
        public static DamageType DarkMagic = Make(nameof(DarkMagic));
        public static DamageType FireMagic = Make(nameof(FireMagic));
        public static DamageType FrostMagic = Make(nameof(FrostMagic));
        public static DamageType Elemental = Make(nameof(Elemental));
        public static DamageType Nature = Make(nameof(Nature));
        public static DamageType Light = Make(nameof(Light));
        public static DamageType Death = Make(nameof(Death));
    }

    //public enum DamageElement
    //{
    //    [Title( Name = "Физический урон" })]
    //    Physical = 0,
    //    [Title("Кинетический урон")]
    //    Kenetic = 2,

    //    [Title("Огонь")]
    //    Fire = 10,
    //    [Title("Холод")]
    //    Cold = 20,

    //    [Title("Магический урон")]
    //    Magical = 100,
    //    [Title("Светлая магия")]
    //    HolyMagic = 200,
    //    [Title("Тёмная магия")]
    //    DarkMagic = 300,
    //    [Title("Магия огня")]
    //    FireMagic = 400,
    //    [Title("Магия льда")]
    //    FrostMagic = 500,


    //    [Title("Стихийный урон")]
    //    Elemental = 1000,
    //    [Title("Природа")]
    //    Nature = 2000,
    //    [Title("Свет")]
    //    Light = 3000,
    //    [Title("Смерть")]
    //    Death = 4000,
    //}
}