using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing.Console
{
    /// <summary>
    /// Charset for window border
    /// </summary>
    public class Border
    {
        public char UpperLeftCorner { set; get; }
        public char UpperRightCorner { set; get; }
        public char HorizontalLine { set; get; }
        public char VerticalLine { set; get; }
        public char LowerLeftCorner { set; get; }
        public char LowerRightCorner { set; get; }
        public char PerpendicularLeftward { set; get; }
        public char PerpendicularRightward { set; get; }
    }
}
