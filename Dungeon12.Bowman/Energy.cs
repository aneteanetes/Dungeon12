using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Bowman
{
    public class Energy
    {
        public Energy()
        {
            var timer = new System.Timers.Timer(100);
            timer.Elapsed += EnergyRestore;
            timer.Start();
        }

        private void EnergyRestore(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (LeftHand < 50)
            {
                LeftHand += 1;
            }

            if (RightHand < 50)
            {
                RightHand += 1;
            }
        }

        public int LeftHand { get; set; } = 50;

        public int RightHand { get; set; } = 50;

        public override string ToString() => $"{LeftHand}/50 | {RightHand}/50";
    }
}
