using Rogue.Drawing.Impl;
using Rogue.Map;
using Rogue.Types;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Labirinth
{
    /// <summary>
    /// draw object in labirinth bounds, by setting rectangle and drawable object
    /// </summary>
    public class LabirinthUnitDrawSession : DrawSession
    {
        public Location Location { get; set; }

        public IDrawable Object { get; set; }

        public Point Position { get; set; }

        public override IDrawSession Run()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = 2+Position.X,
                Y = 2+Position.Y,
                Width = 1,
                Height = 1,
            };

            return base.Run();
        }
    }
}
