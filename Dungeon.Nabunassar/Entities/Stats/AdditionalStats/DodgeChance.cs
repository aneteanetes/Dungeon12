using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nabunassar.Entities.Stats.AdditionalStats
{
    internal class DodgeChance : BaseStandaloneStat
    {
        public override void BindPersona(Persona persona)
        {
            Value = ((int)persona.PrimaryStats.Agility) * 5;
        }
    }
}
