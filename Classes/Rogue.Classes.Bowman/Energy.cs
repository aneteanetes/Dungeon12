using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Classes.Bowman
{
    public class Energy
    {
        public Energy()
        {
            var timer = new System.Timers.Timer(500);
            timer.Elapsed += EnergyRestore;
            timer.Start();
        }

        private void EnergyRestore(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (LeftHand < 50)
            {
                LeftHand += 5;
            }

            if (RightHand < 50)
            {
                RightHand += 5;
            }
        }

        public int LeftHand { get; set; } = 50;

        public int RightHand { get; set; } = 50;

        public override string ToString() => $"{LeftHand}/50 | {RightHand}/50";
    }
}
