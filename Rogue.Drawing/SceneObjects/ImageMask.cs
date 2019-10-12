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

        private Func<float> amountSource;
        private float amountPercentage;

        public float AmountPercentage
        {
            get
            {
                if (amountSource != default)
                {
                    return amountSource();
                }

                return amountPercentage;
            }
            set => amountPercentage = value;
        }

        public IDrawColor Color { get; set; }

        private Func<bool> visibleSource;
        private bool visible;

        public bool Visible
        {
            get
            {
                if (visibleSource != default)
                {
                    return visibleSource();
                }

                return visible;
            }
            set => visible = value;
        }

        public static ImageMask Radial() => new ImageMask()
        {
            CacheAvailable = true,
            Pattern = MaskPattern.RadialCounterClockwise
        };

        public ImageMask BindAmount(Func<float> source)
        {
            amountSource = source;
            return this;
        }

        public ImageMask BindVisible(Func<bool> source)
        {
            visibleSource = source;
            return this;
        }
    }
}