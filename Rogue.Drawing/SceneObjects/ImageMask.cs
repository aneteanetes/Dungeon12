using Rogue.View.Enums;
using Rogue.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing.SceneObjects
{
    public class ImageMask : IImageMask
    {
        public MaskPattern Pattern { get; set; }

        public bool CacheAvailable { get; set; }

        public float Opacity { get; set; }

        public float AmountPercentage { get; set; }

        public IDrawColor Color { get; set; }

        public static ImageMask Radial() => new ImageMask()
        {
            CacheAvailable = true,
            Pattern = MaskPattern.RadialClockwise
        };
    }
}