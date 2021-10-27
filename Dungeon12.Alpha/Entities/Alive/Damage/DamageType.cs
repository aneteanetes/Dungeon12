using Dungeon.Data;
using Dungeon.Drawing;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.Entities.Alive
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
                Dungeon.Store.Entity<DamageType>().ToList().ForEach(dt => DamageTypeCache[dt.Name] = dt);
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
}