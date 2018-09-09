using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.GUI
{
    public class WindowDrawSession : DrawSession
    {
        public override IDrawSession Run()
        {
            return this;
        }
    }
}
