using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon.Monogame
{
    public class PixelTexture
    {
        private GraphicsDevice _graphicsDevice;
        private Texture2D _pixel;

        public PixelTexture(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _pixel = new Texture2D(_graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            _pixel.SetData(new[] { Color.White });
        }

        public Texture2D Get() => _pixel;
    }
}
