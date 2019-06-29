/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Renderers
{
    using System;
    using System.ComponentModel;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Emitters;

    /// <summary>
    /// Defines a Renderer which uses the standard XNA SpriteBatch class to render Particles.
    /// </summary>
    public sealed class SpriteBatchRenderer : Renderer
    {
        private SpriteBatch Batch;

        /// <summary>
        /// A BlendState for non premultiplied additive blending.
        /// </summary>
        private BlendState NonPremultipliedAdditive { get; set; }

        /// <summary>
        /// Disposes any unmanaged resources being used by the Renderer.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                if (this.Batch != null)
                    this.Batch.Dispose();

            base.Dispose(disposing);
        }

        /// <summary>
        /// Loads any content required by the renderer.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if the GraphicsDeviceManager has not been set.</exception>
        public override void LoadContent(ContentManager content)
        {
            Guard.IsTrue(base.GraphicsDeviceService == null, "GraphicsDeviceService property has not been initialised with a valid value.");

            if (this.Batch == null)
                this.Batch = new SpriteBatch(base.GraphicsDeviceService.GraphicsDevice);

            if (this.NonPremultipliedAdditive == null)
                this.NonPremultipliedAdditive = new BlendState
                {
                    AlphaBlendFunction = BlendFunction.Add,
                    AlphaDestinationBlend = Blend.One,
                    AlphaSourceBlend = Blend.SourceAlpha,
                    ColorBlendFunction = BlendFunction.Add,
                    ColorDestinationBlend = Blend.One,
                    ColorSourceBlend = Blend.SourceAlpha,
                };
        }

        /// <summary>
        /// Renders the specified Emitter, applying the specified transformation offset.
        /// </summary>
        public override void RenderEmitter(Emitter emitter, ref Matrix transform)
        {
            Guard.ArgumentNull("emitter", emitter);
            Guard.IsTrue(this.Batch == null, "SpriteBatchRenderer is not ready! Did you forget to LoadContent?");

            if (emitter.ParticleTexture != null && emitter.ActiveParticlesCount > 0)
            {
                // Bail if the emitter blend mode is "None"...
                if (emitter.BlendMode == EmitterBlendMode.None)
                    return;

                // Calculate the source rectangle and origin offset of the Particle texture...
                Rectangle source = new Rectangle(0, 0, emitter.ParticleTexture.Width, emitter.ParticleTexture.Height);
                Vector2 origin = new Vector2(source.Width / 2f, source.Height / 2f);

                BlendState blendState = this.GetBlendState(emitter.BlendMode);

                this.Batch.Begin(SpriteSortMode.Deferred, blendState, null, null, null, null, transform);

                for (int i = 0; i < emitter.ActiveParticlesCount; i++)
                {
                    Particle particle = emitter.Particles[i];

                    float scale = particle.Scale / emitter.ParticleTexture.Width;

                    this.Batch.Draw(emitter.ParticleTexture, particle.Position, source, new Color(particle.Colour), particle.Rotation, origin, scale, SpriteEffects.None, 0f);
                }

                this.Batch.End();
            }
        }

        /// <summary>
        /// Renders the specified ParticleEffect.
        /// </summary>
        public void RenderEffect(ParticleEffect effect, SpriteBatch spriteBatch)
        {
            Guard.ArgumentNull("effect", effect);
            Guard.ArgumentNull("spriteBatch", spriteBatch);

            for (int i = 0; i < effect.Count; i++)
                this.RenderEmitter(effect[i], spriteBatch);
        }

        /// <summary>
        /// Renders the specified Emitter.
        /// </summary>
        public unsafe void RenderEmitter(Emitter emitter, SpriteBatch spriteBatch)
        {
            Guard.ArgumentNull("emitter", emitter);
            Guard.ArgumentNull("spriteBatch", spriteBatch);

            if (emitter.ParticleTexture != null && emitter.ActiveParticlesCount > 0)
            {
                // Calculate the source rectangle and origin offset of the Particle texture...
                Rectangle source = new Rectangle(0, 0, emitter.ParticleTexture.Width, emitter.ParticleTexture.Height);
                Vector2   origin = new Vector2(source.Width / 2f, source.Height / 2f);

                fixed (Particle* particleArray = emitter.Particles)
                {
                    for (int i = 0; i < emitter.ActiveParticlesCount; i++)
                    {
                        Particle* particle = (particleArray + i);

                        float scale = particle->Scale / emitter.ParticleTexture.Width;

                        spriteBatch.Draw(emitter.ParticleTexture, particle->Position, source, new Color(particle->Colour), particle->Rotation, origin, scale, SpriteEffects.None, 0f);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the BlendState object corresponding to the specified EmitterBlendMode value.
        /// </summary>
        /// <param name="emitterBlendMode">The EmitterBlendMode value.</param>
        /// <returns>A BlendState object.</returns>
        private BlendState GetBlendState(EmitterBlendMode emitterBlendMode)
        {
            switch (emitterBlendMode)
            {
                case EmitterBlendMode.Alpha:
                    {
                        return BlendState.NonPremultiplied;
                    }
                case EmitterBlendMode.Add:
                    {
                        return this.NonPremultipliedAdditive;
                    }
                default:
                    {
                        throw new InvalidEnumArgumentException("emitterBlendMode", (int)emitterBlendMode, typeof(EmitterBlendMode));
                    }
            }
        }
    }
}