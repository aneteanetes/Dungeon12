namespace Rogue.App.Utils
{
    using Avalonia.Media;

    public class Font
    {
        private static FontFamily triforceFamilty;
        public static FontFamily Common
        {
            get
            {
                if (triforceFamilty == null)
                {
                    triforceFamilty = new FontFamily(
                        "Triforce(RUS BY LYAJKA) Triforce",
                        new System.Uri("resm:Rogue.Resources.Fonts.Common.otf?assembly=Rogue.Resources#Triforce(RUS BY LYAJKA) Triforce"));
                }

                return triforceFamilty;
            }
        }

    }
}