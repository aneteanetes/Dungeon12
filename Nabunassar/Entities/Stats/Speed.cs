using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nabunassar.Entities.Stats
{
    internal class Speed : BaseStandaloneStat
    {
        public override void BindPersona(Persona persona)
        {
            Value = (int)persona.PrimaryStats.Agility;
        }
    }
}
