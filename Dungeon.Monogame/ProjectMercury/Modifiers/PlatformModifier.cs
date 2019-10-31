/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Modifiers
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines a Modifier which freezes a particle when it comes into contact with a bounding box.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.PlatformModifierTypeConverter, ProjectMercury.Design")]
#endif
    public sealed class PlatformModifier : Modifier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformModifier"/> class.
        /// </summary>
        public PlatformModifier()
        {
            this.Platforms = new List<BoundingBox>();
        }

        /// <summary>
        /// The list of platforms.
        /// </summary>
        public List<BoundingBox> Platforms { get; set; }

        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            PlatformModifier modifier = new PlatformModifier();

            modifier.Platforms.AddRange(this.Platforms);

            return modifier;
        }

        /// <summary>
        /// Processes the particles.
        /// </summary>
        /// <param name="dt">Elapsed time in whole and fractional seconds.</param>
        /// <param name="particleArray">A pointer to an array of particles.</param>
        /// <param name="count">The number of particles which need to be processed.</param>
        protected internal override unsafe void Process(float dt, Particle* particleArray, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Particle* particle = (particleArray + i);

                var position = new Vector3(particle->Position, 0);

                for (int ii = 0; ii < this.Platforms.Count; ii++)
                {
                    ContainmentType result;

                    this.Platforms[ii].Contains(ref position, out result);

                    if (result == ContainmentType.Contains)
                    {
                        particle->Momentum = Vector2.Zero;

                        return;
                    }
                }
            }
        }
    }
}
