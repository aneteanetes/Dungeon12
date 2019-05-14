namespace Rogue.Drawing
{
    using Rogue.View.Interfaces;

    public static class DrawTextFonts
    {
        public static IDrawText Montserrat(this IDrawText drawText)
        {
            drawText.FontName = "Montserrat";
            drawText.FontAssembly = "Rogue.Resources";
            drawText.FontPath = "Rogue.Resources.Fonts.Mont.otf";

            return drawText;
        }
    }
}
