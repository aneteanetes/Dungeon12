namespace Dungeon12
{
    using Dungeon.View.Interfaces;

    public static class FontsExtensions
    {

        public static T Gabriela<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Gabriela";
            drawText.FontAssembly = "Dungeon12";

            return drawText;
        }
        public static T Gabriola<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Gabriola";
            drawText.FontAssembly = "Dungeon12";

            return drawText;
        }

        public static T SegoeUI<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Segoe UI";
            drawText.FontAssembly = "Dungeon12";

            return drawText;
        }

        public static T SegoeUIBold<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Segoe UI Bold";
            drawText.FontAssembly = "Dungeon12";

            return drawText;
        }

        public static T FrizQuad<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Fritz Quadrata Cyrillic";
            drawText.FontAssembly = "Dungeon12";

            return drawText;
        }

        public static T FrizBold<T>(this T drawText) where T : IDrawText
        {            
            drawText.FontName = "Friz Quadrata Bold";
            drawText.FontAssembly = "Dungeon12";

            return drawText;
        }
    }
}