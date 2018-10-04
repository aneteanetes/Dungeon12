using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.GUI
{
    public class MessageDrawSession : DrawSession
    {
        public MessageDrawSession()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = 2,
                Y = 26,
                Width = 88,
                Height = 1
            };
        }

        public DrawText Message { get; set; }

        public override IDrawSession Run()
        {
            this.Write(0, 0, this.Message);
            return base.Run();
        }
    }
}
