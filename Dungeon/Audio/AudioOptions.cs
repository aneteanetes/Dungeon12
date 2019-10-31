namespace Dungeon.Audio
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AudioOptions
    {
        private double volume = 0.5;
        public double Volume
        {
            get => volume;
            set
            {
                volume = value;
                if (volume > 1.0)
                {
                    volume = 1.0;
                }
                if (volume < 0)
                {
                    volume = 0;
                }
            }
        }

        public bool Repeat { get; set; }
    }
}