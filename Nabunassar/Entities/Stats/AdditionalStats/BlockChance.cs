using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nabunassar.Entities.Stats.AdditionalStats
{
    internal class BlockChance : BaseStandaloneStat
    {
        public override void BindPersona(Persona persona)
        {
            Value = ((int)persona.PrimaryStats.Constitution) * 5;
        }
    }
}
