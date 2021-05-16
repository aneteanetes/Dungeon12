using Dungeon.Utils;
using Microsoft.Xna.Framework;
using System.ComponentModel.DataAnnotations;

namespace Dungeon.Monogame
{
    public class MonogameClientSettings
    {
        [Display(Name ="Во весь экран")]
        public bool IsFullScreen { get; set; } = true;

        [Display(Name = "Во весь экран (в окне)")]
        public bool IsWindowedFullScreen { get; set; } = false;

        [Display(Name = "Ширина в px")]
        public int WidthPixel { get; set; } = 1280;

        [Display(Name = "Высота в px")]
        public int HeightPixel { get; set; } = 720;

        [Display(Name = "V-sync")]
        public bool VerticalSync { get; set; } = true;

        [Display(Name = "2D свет")]
        public bool Add2DLighting { get; set; } = true;

        [Display(Name = "2D свет - цвет")]
        public Color AmbientColor2DLight { get; set; }

        [Hidden]
        public int CellSize { get; set; }

        public bool Borderless { get; set; } = false;
    }
}
