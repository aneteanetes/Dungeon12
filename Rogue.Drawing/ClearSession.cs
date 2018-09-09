using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.Impl;
using Rogue.Settings;

namespace Rogue.Drawing
{
    public class ClearSession : DrawSession
    {
        public DrawingSize DrawingSize { get; set; }

        public bool ClearAll
        {
            set
            {
                this.DrawRegion = new Types.Rectangle
                {
                    X = 0,
                    Y = 0,
                    Height = DrawingSize.WindowLines,
                    Width = DrawingSize.WindowChars
                };
            }
        }
    }
}
