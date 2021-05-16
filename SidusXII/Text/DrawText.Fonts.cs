namespace SidusXII
{
    using Dungeon.View.Interfaces;

    public static class DrawTextFonts
    {
        public static T Montserrat<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Montserrat";
            drawText.FontAssembly = "SidusXII";
            //drawText.FontPath = "Dungeon.Resources.Fonts.Mont.otf";

            return drawText;
        }

        public static T Carribean<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Pieces of Eight Cyrillic AA";
            drawText.FontAssembly = "SidusXII";

            return drawText;
        }

        public static T Triforce<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Triforce(RUS BY LYAJKA)";
            drawText.FontAssembly = "SidusXII";
            //drawText.FontPath = null;

            return drawText;
        }

        public static T Pieces<T>(this T drawText) where T : IDrawText
        {
            drawText.FontName = "Pieces of Eight Cyrillic AA";
            drawText.FontAssembly = "SidusXII";
            //drawText.FontPath = null;

            return drawText;
        }
    }
}