using System;
using System.Collections.Generic;
using System.Text;

namespace SidusXII.ConsoleUI
{
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

        private static Border _double = default;

        public static Border Double
        {
            get
            {
                if (_double == default)
                {
                    _double = new Border
                    {
                        HorizontalLine = '═',
                        LowerLeftCorner = '╚',
                        LowerRightCorner = '╝',
                        PerpendicularLeftward = '╣',
                        PerpendicularRightward = '╠',
                        UpperLeftCorner = '╔',
                        UpperRightCorner = '╗',
                        VerticalLine = '║'
                    };
                }
                return _double;
            }
        }
    }
}