using Dungeon.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace VegaPQ.Entities
{
    public class Cell : Entity
    {
        public bool Painted { get; set; }

        public CellType Type { get; set; }

        public int X { get; set; }

        public int Y { get; set; }
    }
}
