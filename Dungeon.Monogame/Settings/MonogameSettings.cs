using Dungeon.Utils;
using Microsoft.Xna.Framework;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dungeon.Monogame.Settings
{
    public class MonogameSettings
    {
        public TimeSpan DropFpsOnUnfocus { get; set; } = TimeSpan.Zero;

        public WindowMode WindowMode { get; set; } = WindowMode.Windowed;

        [Display(Name = "Ширина в px")]
        public int WidthPixel
        {
            get
            {
                if(widthPixel == 0)
                {
                    WidthHeightAutomated = true;
                    return OriginWidthPixel;
                }
                return widthPixel;
            }
            set => widthPixel = value;
        }
        private int widthPixel = 0;

        public bool WidthHeightAutomated { get; private set; }

        [Display(Name = "Высота в px")]
        public int HeightPixel
        {
            get
            {
                if (heightPixel == 0)
                {
                    WidthHeightAutomated = true;
                    return OriginHeightPixel;
                }
                return heightPixel;
            }
            set => heightPixel = value;
        }
        private int heightPixel = 0 ;

        public int OriginWidthPixel { get; set; }

        public int OriginHeightPixel { get; set; }

        [Display(Name = "V-sync")]
        public bool VerticalSync { get; set; } = true;

        [Display(Name = "2D свет")]
        public bool Add2DLighting { get; set; } = true;

        public int MonitorIndex { get; set; } = 0;

        [Display(Name = "2D свет - цвет")]
        public Color AmbientColor2DLight { get; set; }

        [Hidden]
        public int CellSize { get; set; }

        public bool Borderless { get; set; } = false;

        public bool IsDebug { get; set; }

        public bool NeedCalculateCamera { get; set; } = true;

        /// <summary>
        /// Растягивать или ужимать изображения если нет подходящего под разрешение
        /// </summary>
        public bool ResouceStretching { get; set; } = true;
    }
}
