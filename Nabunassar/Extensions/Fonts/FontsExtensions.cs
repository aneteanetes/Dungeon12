namespace Nabunassar
{
    using Dungeon;
    using Dungeon.Drawing;
    using Dungeon.View.Interfaces;

    public static class FontsExtensions
    {
        public static IDrawText Calibri(this string text) => text.AsDrawText().Calibri();

        public static T Calibri<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Calibri";
            drawText.FontAssembly = "Global.GameAssemblyName";

            if (drawText.Bold)
                drawText.FontName+=" Bold";

            return drawText;
        }

        public static T InCommonColor<T>(this T text) where T : IDrawText
        {
            text.ForegroundColor = Global.CommonColor;
            text.BackgroundColor = Global.CommonColor;

            return text;
        }

        public static T InCommonLightColor<T>(this T text) where T : IDrawText
        {
            text.ForegroundColor = Global.CommonColorLight;
            text.BackgroundColor = Global.CommonColorLight;

            return text;
        }

        public static IDrawText Gabriela(this string text) => text.AsDrawText().Gabriela();

        public static IDrawText Navieo(this string text) => text.AsDrawText().Navieo();

        public static T Navieo<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "NAVIEO Trial";
            drawText.FontAssembly = "Global.GameAssemblyName";

            return drawText;
        }


        public static T Gabriela<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "URWGeometricBold";
            drawText.FontAssembly = "Global.GameAssemblyName";

            return drawText;
        }


        /// <summary>
        /// Оформление текста по умолчанию
        /// </summary>
        /// <returns></returns>
        public static IDrawText DefaultTxt(this string text, int size, bool wordWrap = false)
        {
            var drawText = text.AsDrawText();            

            drawText.FontName = "URWGeometricBold";
            drawText.FontAssembly = Global.GameAssemblyName;

            drawText.WordWrap = wordWrap;

            return drawText.InSize(size).InColor(Global.CommonColorLight);
        }

        public static IDrawText HeroName(this string name)
        {
            return name.AsDrawText().Gabriela().InColor(Global.CommonColor).InSize(20);
        }

        public static T Gabriola<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Gabriola";
            drawText.FontAssembly = "Global.GameAssemblyName";

            return drawText;
        }

        public static IDrawText SegoeUI(this string text) => text.AsDrawText().SegoeUI();

        public static T SegoeUI<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Segoe UI";
            drawText.FontAssembly = "Global.GameAssemblyName";

            return drawText;
        }
        public static IDrawText SegoeUIBold(this string text) => text.AsDrawText().Calibri();

        public static T SegoeUIBold<T>(this T drawText) where T : IDrawText
        {
            return Calibri(drawText);

            //drawText.FontName = "Segoe UI Bold";
            //drawText.FontAssembly = "Global.GameAssemblyName";

            //return drawText;
        }

        public static T FrizQuad<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Fritz";
            drawText.FontAssembly = "Global.GameAssemblyName";

            return drawText;
        }

        public static T FrizBold<T>(this T drawText) where T : IDrawText
        {            
            drawText.FontName = "Friz";
            drawText.FontAssembly = "Global.GameAssemblyName";

            return drawText;
        }
    }
}