using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon.Monogame
{
    public class SpriteBatchKnowed : SpriteBatch
    {
        public bool IsOpened { get; private set; }

        public SpriteBatchKnowed(GraphicsDevice graphicsDevice) : base(graphicsDevice) { }

        public DepthStencilState DepthStencilState;

        public SpriteSortMode? spriteSortMode;

        public new void Begin(SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null, Matrix? transformMatrix = null)
        {
            IsOpened = true;
            base.Begin(spriteSortMode ?? sortMode, blendState, samplerState, depthStencilState ?? DepthStencilState, rasterizerState, effect, transformMatrix);
        }

        public new void End()
        {
            IsOpened = false;
            base.End();
        }

        /// <summary>
        /// End then Begin with  params
        /// </summary>
        public void Flush()
        {
        }
    }
}
