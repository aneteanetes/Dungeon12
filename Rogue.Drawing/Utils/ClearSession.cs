using Rogue.Drawing.Impl;
using Rogue.Settings;

namespace Rogue.Drawing.Utils
{
    public class ClearSession : DrawSession
    {
        public DrawingSize DrawingSize { get; set; } = new DrawingSize();

        public ClearSession()
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
