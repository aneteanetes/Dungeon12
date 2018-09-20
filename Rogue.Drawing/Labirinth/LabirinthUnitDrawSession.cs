using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Labirinth
{
    /// <summary>
    /// draw object in labirinth bounds, by setting rectangle and drawable object
    /// </summary>
    public class LabirinthUnitDrawSession : DrawSession
    {
        public IDrawable Object { get; set; }

        public override IDrawSession Run()
        {
            this.Write(0, 0, new DrawText(Object.Icon, Object.ForegroundColor, Object.BackgroundColor));
            return base.Run();
        }
    }
}
