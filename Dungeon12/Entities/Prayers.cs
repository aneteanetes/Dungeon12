using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon12.Entities
{
    internal class Prayers
    {
        private Dictionary<MonthYear, Prayer> Values = new();

        public Prayer Current
        {
            get
            {
                var month = Global.Game.Calendar.MonthYear;
                if (!Values.TryGetValue(month, out var prayer))
                {
                    prayer = Values[month]=new Prayer()
                    {
                        God = month,
                        Value=10,
                        Level=1
                    };
                }
                return prayer;
            }
        }

        public int Level=>Current.Level;

        public int Value => Current.Value;
    }
}