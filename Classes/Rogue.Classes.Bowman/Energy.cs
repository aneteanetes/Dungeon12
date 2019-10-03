using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Classes.Bowman
{
    public class Energy
    {
        public int LeftHand { get; set; } = 50;

        public int RightHand { get; set; } = 50;

        public override string ToString() => $"{LeftHand}/50 | {RightHand}/50";
    }
}
