/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Modifiers
{
    using System.ComponentModel;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines a Modifier which adjusts the scale of a Particle over its lifetime.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.ScaleModifierTypeConverter, ProjectMercury.Design")]
#endif
    public class ScaleModifier : Modifier
    {
        /// <summary>
        /// The initial scale of the Particle in pixels.
        /// </summary>
        public float InitialScale;

        /// <summary>
        /// The ultimate scale of the Particle in pixels.
        /// </summary>
        public float UltimateScale;

        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new ScaleModifier
            {
                InitialScale = this.InitialScale,
                UltimateScale = this.UltimateScale
            };
        }

        /// <summary>
        /// Processes the particles.
        /// </summary>
        /// <param name="dt">Elapsed time in whole and fractional seconds.</param>
        /// <param name="particle">A pointer to an array of particles.</param>
        /// <param name="count">The number of particles which need to be processed.</param>
        protected internal override unsafe void Process(float dt, Particle* particle, int count)
        {
            for (int i = 0; i < count; i++)
            {
                particle->Scale = (this.InitialScale + ((this.UltimateScale - this.InitialScale) * particle->Age));

                particle++;
            }
        }
    }
}