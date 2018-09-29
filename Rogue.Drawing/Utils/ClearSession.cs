using Rogue.Drawing.Impl;

namespace Rogue.Drawing.Utils
{
    public class ClearSession : DrawSession
    {
        public ClearSession()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = 0,
                Y = 0,
                Height = 35,
                Width = 100
            };
        }
    }
}
