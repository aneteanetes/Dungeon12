namespace Rogue.Drawing
{
    using Rogue.Drawing.Impl;
    using Rogue.Settings;

    public class ClearSession : DrawSession
    {
        public ClearSession()
        {
            this.AutoClear = false;
        }

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
