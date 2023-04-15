using FontStashSharp.Interfaces;
using FontStashSharp.Rasterizers.StbTrueTypeSharp;

namespace Dungeon.Monogame.Fonts
{
    public class StbFontSourceLoader : IFontLoader
    {
        private readonly StbTrueTypeSharpSettings _settings;

        public StbFontSourceLoader(StbTrueTypeSharpSettings settings)
        {
            _settings = settings;
        }

        public IFontSource Load(byte[] data)
        {
            return new StbFontSource(data, _settings);
        }
    }
}
