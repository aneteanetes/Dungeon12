using Dungeon.Resources;
using Dungeon.View.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Dungeon.Monogame
{
    public class SpriteBatchManager
    {
        private ContentManager _contentManager;
        private GraphicsDevice _graphicsDevice;
        private Matrix? _resolutionMatrix;
        private static readonly RasterizerState antialise = new() { MultiSampleAntiAlias = true };
        private bool isBegined = false;
        public const string NoEffectNameConstant = "#@$NO_%EFF3CT^";

        private Dictionary<string, SpriteBatchKnowed> SpriteBatches = new Dictionary<string, SpriteBatchKnowed>();

        public SpriteBatchManager(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            _graphicsDevice = graphicsDevice;
            _contentManager = contentManager;
        }

        public void Begin(Matrix? resolutionMatrix)
        {
            _resolutionMatrix = resolutionMatrix;
            isBegined = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="samplerState"></param>
        /// <param name="alphaBlend"></param>
        /// <param name="isTransformMatrix"></param>
        /// <param name="effect">if null then <see cref="NoEffectNameConstant"/> constant</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public SpriteBatchKnowed GetSpriteBatch(SamplerState samplerState = null, bool alphaBlend = false, bool isTransformMatrix = true, IEffect effect = default)
        {
            if (!isBegined)
                throw new System.Exception("SpriteBatchManager needs to be Begin() for work with actual resolution matrix!");

            if (samplerState==null)
                samplerState = SamplerState.PointWrap;

            var effectName = effect == null ? NoEffectNameConstant : effect.Name;

            var key = $"{samplerState}{alphaBlend}{isTransformMatrix}{effectName}";
            if (!SpriteBatches.TryGetValue(key, out var spriteBatch))
            {
                SpriteBatches[key] = spriteBatch = new SpriteBatchKnowed(_graphicsDevice);
            }

            if (!spriteBatch.IsOpened)
            {
                spriteBatch.Begin(
                    transformMatrix: _resolutionMatrix,
                    samplerState: samplerState,
                    blendState: alphaBlend ? BlendState.AlphaBlend : BlendState.NonPremultiplied, /*GetBlendState(useLight, alphaBlend, colorInvert),*/
                    effect: GetEffect(effectName),
                    depthStencilState: spriteBatch.DepthStencilState,
                    rasterizerState: antialise);
            }

            return spriteBatch;
        }

        public void End()
        {
            foreach (var kv in SpriteBatches)
            {
                if (kv.Value.IsOpened)
                    kv.Value.End();
            }
            isBegined = false;
        }

        private Effect GetEffect(string effectName)
        {
            if (effectName == NoEffectNameConstant)
                return null;

            if (!XnaEffectsLoaded.TryGetValue(effectName, out var xnaeff))
            {
                var effectres = ResourceLoader.Load($"Shaders/{effectName}.xnb".AsmRes(), @throw: false);
                if (effectres != null)
                {
                    xnaeff = _contentManager.Load<Effect>($"{effectName}", effectres.Stream);
                }

                XnaEffectsLoaded[effectName] = xnaeff;
            }

            return xnaeff;
        }

        private static Dictionary<string, Effect> XnaEffectsLoaded = new Dictionary<string, Effect>();
    }
}
