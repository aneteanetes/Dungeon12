using Dungeon;
using Dungeon.Entities;
using Dungeon.Map;
using Dungeon.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.CardGame.Entities
{
    public class AreaCard : Card
    {
        /// <summary>
        /// 
        /// <para>
        /// [Рассчётное через сеть]
        /// </para>
        /// </summary>
        [Proxied(typeof(NetProxy))]
        public int Rounds { get => Get(___Rounds, typeof(AreaCard).AssemblyQualifiedName); set => Set(value, typeof(AreaCard).AssemblyQualifiedName); }
        private int ___Rounds;

        public int Size { get; set; } = 1;
    }
}
