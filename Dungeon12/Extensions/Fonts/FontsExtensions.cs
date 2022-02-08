namespace Dungeon12
{
    using Dungeon.View.Interfaces;

    public static class FontsExtensions
    {
        public static T Montserrat<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Montserrat";
            drawText.FontAssembly = "Dungeon12";
            //drawText.FontPath = "Dungeon.Resources.Fonts.Mont.otf";

            return drawText;
        }

        public static T Carribean<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Pieces of Eight Cyrillic AA";
            drawText.FontAssembly = "Dungeon12";

            return drawText;
        }

        public static T Gabriela<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Gabriela";
            drawText.FontAssembly = "Dungeon12";

            return drawText;
        }

        public static T Cambria<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Cambria";
            drawText.FontAssembly = "Dungeon12";

            return drawText;
        }

        public static T Runic<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "American TextC";
            drawText.FontAssembly = "Dungeon12";

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