using Dungeon;
using Dungeon.Network;
using Dungeon.Proxy;
using SidusXII.Entities;
using SidusXII.Enums;

namespace SidusXII.Characters
{
    public class Character : ProxyObject
    {
        private Race _race;
        public Race Race
        {
            get => _race;
            set
            {
                _race = value;
                Damage.ElementType = _race.Element;
            }
        }

        private Class _class;
        public Class Class
        {
            get => _class;
            set
            {
                _class = value;
            }
        }

        public Spec Spec { get; set; }

        public Profession Profession { get; set; }

        public Damage Damage { get; set; } = new Damage();

        public Defence Defence { get; set; } = new Defence();

        /// <summary>
        /// Здоровье
        /// <para>
        /// [Рассчётное через сеть]
        /// </para>
        /// </summary>
        [Proxied(typeof(NetProxy))]
        public long HP { get => Get(___HP, typeof(Character).AssemblyQualifiedName); set => Set(value, typeof(Character).AssemblyQualifiedName); }
        private long ___HP = default;
    }
}