using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon.Monogame
{
    public class SpiteBatchKnowed : SpriteBatch
    {
        public bool Opened { get; private set; }

        public SpiteBatchKnowed(GraphicsDevice graphicsDevice) : base(graphicsDevice) { }

        public new void Begin(SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null, Matrix? transformMatrix = null)
        {
            Opened = true;
            base.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
        }

        public new void End()
        {
            Opened = false;
            base.End();
        }
    }
}
