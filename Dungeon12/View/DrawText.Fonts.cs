namespace Dungeon12
{
    using Dungeon.View.Interfaces;

    public static class DrawTextFonts
    {
        public static T Montserrat<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Montserrat";
            drawText.FontAssembly = "Dungeon12";
            //drawText.FontPath = "Dungeon.Resources.Fonts.Mont.otf";

            return drawText;
        }

        public static T Triforce<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Triforce(RUS BY LYAJKA)";
            drawText.FontAssembly = "Dungeon12";
            //drawText.FontPath = null;

            return drawText;
        }
    }
}
namespace Dungeon.Drawing
{
}