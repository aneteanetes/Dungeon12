namespace Dungeon.Drawing
{
    using Dungeon.View.Interfaces;

    public static class DrawTextFonts
    {
        public static T Montserrat<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Montserrat";
            drawText.FontAssembly = "Dungeon.Resources";
            drawText.FontPath = "Dungeon.Resources.Fonts.Mont.otf";

            return drawText;
        }

        public static T Triforce<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Triforce";
            drawText.FontAssembly = "Dungeon.Resources";
            drawText.FontPath = null;

            return drawText;
        }
    }
}