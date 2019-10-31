/*  
 Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury
{
    using System.Collections.Generic;
    using ProjectMercury.Renderers;
    using ProjectMercury.Threading;

    public class ParticleEffectManager : List<ParticleEffect>
    {
        /// <summary>
        /// Gets or sets the renderer which is used to render the particle effects.
        /// </summary>
        public Renderer Renderer { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffectManager"/> class.
        /// </summary>
        /// <param name="renderer">The renderer which will be used to render particles.</param>
        public ParticleEffectManager(Renderer renderer) : base(20)
        {
            Guard.ArgumentNull("renderer", renderer);

            this.Renderer = renderer;
        }

        /// <summary>
        /// Updates all the particle effects which are managed by the manager.
        /// </summary>
        /// <param name="deltaSeconds">The elapsed time in whole and fractional seconds.</param>
        /// <param name="multithreaded">True to update particle effects asynchronously.</param>
        public void Update(float deltaSeconds, bool multithreaded)
        {
            if (!multithreaded)
            {
                for (int i = 0; i < this.Count; i++)
                    this[i].Update(deltaSeconds);
            }
            else
            {
                if (this.Count > 0)
                {
                    Parallel.For(0, this.Count, delegate(int i)
                    {
                        this[i].Update(deltaSeconds);
                    });
                }
            }
        }

        /// <summary>
        /// Gets the number of active particles in each particle effect.
        /// </summary>
        public int ActiveParticlesCount
        {
            get
            {
                int count = 0;
                
                for (int i = 0; i < this.Count; i++)
                    count += this[i].ActiveParticlesCount;
                
                return count;
            }
        }

        /// <summary>
        /// Draws each particle effect.
        /// </summary>
        public void Draw()
        {
            for (int i = 0; i < this.Count; i++)
                this.Renderer.RenderEffect(this[i]);
        }
    }
}