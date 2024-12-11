using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nabunassar.Game
{
    internal class GameVariables
    {
        public int RegionId { get; set; }

        public int PointId { get; set; } = 1;

        public ulong GlobalId { get; set; }

        public bool IsBattle { get; set; }
    }
}