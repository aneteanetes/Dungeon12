using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Monogame
{
    public class MonogameClientSettings
    {
        public bool IsFullScreen { get; set; } = true;

        public bool IsWindowedFullScreen { get; set; } = true;

        public int WidthPixel { get; set; } = 1280;

        public int HeightPixel { get; set; } = 720;

        public bool VerticalSync { get; set; } = true;

        public bool Add2DLighting { get; set; } = true;
    }
}
