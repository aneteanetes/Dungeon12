namespace Rogue.App.Utils
{
    using Avalonia.Media;
    using System.Collections.Generic;
    using System.Drawing.Text;

    public class Font
    {
        private static Dictionary<string, FontFamily> fontsCache = new Dictionary<string, FontFamily>();

        public static FontFamily Common => GetFontFamily("Triforce(RUS BY LYAJKA) Triforce", "Rogue.Resources.Fonts.Common.otf", "Rogue.Resources");

        public static FontFamily GetFontFamily(string name, string path, string asm)
        {
            if (!fontsCache.TryGetValue(name, out var fontFamily))
            {
                fontFamily = FontFamily.Parse(name, new System.Uri($"resm:{path}?assembly={asm}#{name}"));
                //new FontFamily(name, new System.Uri($"resm:{path}?assembly={asm}#{name}"));
                fontsCache[name] = fontFamily;
            }

            return fontFamily;
        }
    }
}