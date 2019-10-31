/*  
 Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Modifiers
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines a modifier which maintains a bounding rectangle around an Emitter.
    /// </summary>
    public sealed class BoundingRectModifier : Modifier
    {
        /// <summary>
        /// Gets a bounding rectangle which encompasses each of the particles of an emitter.
        /// </summary>
        public BoundingRect BoundingRect { get; private set; }

        /// <summary>
        /// Gets or sets a padding value to add to the bounding rectangle.
        /// </summary>
        public float Padding { get; set; }

        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new BoundingRectModifier
            {
                Padding = this.Padding
            };
        }

        /// <summary>
        /// Processes the particles.
        /// </summary>
        /// <param name="elapsedSeconds">Elapsed time in whole and fractional seconds.</param>
        /// <param name="particle">A pointer to the first particle in an array of particles.</param>
        /// <param name="count">The number of particles which need to be processed.</param>
        protected internal override unsafe void Process(float elapsedSeconds, Particle* particle, int count)
        {
            float minX = 0f, minY = 0f;
            float maxX = 0f, maxY = 0f;

            for (int i = 0; i < count; i++)
            {
                minX = particle->Position.X < minX ? particle->Position.X : minX;
                maxX = particle->Position.X > maxX ? particle->Position.X : maxX;
                minY = particle->Position.Y < minY ? particle->Position.Y : minY;
                maxY = particle->Position.Y > maxY ? particle->Position.Y : maxY;

                particle++;
            }

            this.BoundingRect = new BoundingRect
            {
                Min = new Vector2 { X = minX - this.Padding, Y = minY - this.Padding },
                Max = new Vector2 { X = maxX + this.Padding, Y = maxY + this.Padding }
            };
        }
    }
}