namespace Dungeon12
{
    using Dungeon;
    using Dungeon.View.Interfaces;

    public static class FontsExtensions
    {
        public static IDrawText Calibri(this string text) => text.AsDrawText().Calibri();

        public static T Calibri<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Calibri";
            drawText.FontAssembly = "Dungeon12";

            if (drawText.Bold)
                drawText.FontName+=" Bold";

            return drawText;
        }

        public static IDrawText Gabriela(this string text) => text.AsDrawText().Gabriela();

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

        public static IDrawText SegoeUI(this string text) => text.AsDrawText().SegoeUI();

        public static T SegoeUI<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Segoe UI";
            drawText.FontAssembly = "Dungeon12";

            return drawText;
        }
        public static IDrawText SegoeUIBold(this string text) => text.AsDrawText().Calibri();

        public static T SegoeUIBold<T>(this T drawText) where T : IDrawText
        {
            return Calibri(drawText);

            drawText.FontName = "Segoe UI Bold";
            drawText.FontAssembly = "Dungeon12";

            return drawText;
        }

        public static T FrizQuad<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Fritz";
            drawText.FontAssembly = "Dungeon12";

            return drawText;
        }

        public static T FrizBold<T>(this T drawText) where T : IDrawText
        {            
            drawText.FontName = "Friz";
            drawText.FontAssembly = "Dungeon12";

            return drawText;
        }
    }
}