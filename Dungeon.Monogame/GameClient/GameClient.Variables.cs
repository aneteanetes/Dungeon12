using Dungeon.Monogame.Settings;
using Dungeon.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using ProjectMercury.Renderers;

namespace Dungeon.Monogame
{
    public partial class GameClient
    {
        private PenumbraComponent penumbra;
        internal DrawClient DrawClient;
        private Effect GlobalImageFilter = null;

        private GameTime gameTime;
        private Callback сallback;

        private bool loaded = false;
        private bool drawCicled = false;
        private bool skipCallback = false;
        public bool isFatal;
        private bool blockControls = false;
        private float audioVolume = 0;

        private ParticleRenderer ParticleRenderer;

        public ImageLoader ImageLoader;

        internal GraphicsDeviceManager graphics;

        internal SpriteBatchKnowed DefaultSpriteBatch;
        internal SpriteBatchKnowed LayerSpriteBatch;

        public Matrix ResolutionScale;
        private Dot originSize;

        private GameSettings _settings;
        public ContentResolver contentResolver;
    }
}
