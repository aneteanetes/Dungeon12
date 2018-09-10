using System.Collections.Generic;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Data
{
    public class BuffDataDrawSession : DrawSession
    {
        public BuffDataDrawSession()
        {
            this.AutoClear = false;
        }

        public IDrawable Enemy { get; set; }

        public IDrawable Player { get; set; }

        public override IDrawSession Run()
        {
            this.DrawObject(3, Enemy.States);
            this.DrawObject(96, Player.States);

            return base.Run();
        }

        private void DrawObject(int left, IEnumerable<IDrawable> states)
        {
            int pos = 5;
            if (states != null)
            {
                foreach (var state in states)
                {
                    pos++;
                    this.Write(pos, left, state.Icon, state.ForegroundColor);
                }
            }
        }
    }
}
