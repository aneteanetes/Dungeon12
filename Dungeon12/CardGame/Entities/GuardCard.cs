using Dungeon;
using Dungeon.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.CardGame.Entities
{
    public class GuardCard : Card
    {
        /// <summary>
        /// 
        /// <para>
        /// [Рассчётное через сеть]
        /// </para>
        /// </summary>
        [Proxied(typeof(NetProxy))]
        public int Shield { get => Get(___Shield, typeof(GuardCard).AssemblyQualifiedName); set => Set(value, typeof(GuardCard).AssemblyQualifiedName); }
        private int ___Shield;

        public string TurnTriggerName { get; set; }
    }
}
