namespace Dungeon.Drawing.Impl
{
    public static class DrawTextCapitalize
    {
        public static DrawText Capitalize(this DrawText drawText, float space)
        {
            var first = drawText.StringData[0].ToString();
            drawText.ReplaceAt(0, new DrawText(first, drawText.ForegroundColor, drawText.BackgroundColor) { Size = drawText.Size, LetterSpacing = space });

            return drawText;
        }

        public static string Capitalize(this string text)
        {
            return char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}