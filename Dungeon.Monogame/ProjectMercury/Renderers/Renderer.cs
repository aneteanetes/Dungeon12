/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Renderers
{
    using System;
    using Emitters;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines the abstract base class for a Renderer.
    /// </summary>
    public abstract class Renderer : IDisposable
    {
        /// <summary>
        /// Hold a reference to the games GraphicsDeviceService.
        /// </summary>
        public IGraphicsDeviceService GraphicsDeviceService;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing) { }

        /// <summary>
        /// Disposes any unmanaged resources being used by this instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="Renderer"/> is reclaimed by garbage collection.
        /// </summary>
        ~Renderer()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Loads any content needed by the Renderer.
        /// </summary>
        public virtual void LoadContent(ContentManager content) { }

        /// <summary>
        /// Renders the specified Emitter.
        /// </summary>
        public virtual void RenderEmitter(Emitter emitter)
        {
            Guard.ArgumentNull("emitter", emitter);

            Matrix ident = Matrix.Identity;

            this.RenderEmitter(emitter, ref ident);
        }

        /// <summary>
        /// Renders the specified Emitter, applying the specified transformation offset.
        /// </summary>
        public abstract void RenderEmitter(Emitter emitter, ref Matrix transform);

        /// <summary>
        /// Renders the specified ParticleEffect.
        /// </summary>
        public virtual void RenderEffect(ParticleEffect effect)
        {
            Guard.ArgumentNull("effect", effect);

            Matrix ident = Matrix.Identity;

            this.RenderEffect(effect, ref ident);
        }

        /// <summary>
        /// Renders the specified ParticleEffect, applying the specified transformation offset.
        /// </summary>
        public virtual void RenderEffect(ParticleEffect effect, ref Matrix transform)
        {
            Guard.ArgumentNull("effect", effect);

            for (int i = 0; i < effect.Count; i++)
                this.RenderEmitter(effect[i], ref transform);
        }
    }
}