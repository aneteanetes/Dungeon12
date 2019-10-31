/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Modifiers
{
    using System.ComponentModel;

    /// <summary>
    /// Defines a modifier which changes the rotation rate of particles over their lifetime.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.RotationRateModifierTypeConverter, ProjectMercury.Design")]
#endif
    public sealed class RotationRateModifier : Modifier
    {
        /// <summary>
        /// Gets or sets the initial rotation rate in radians per second.
        /// </summary>
        public float InitialRate;

        /// <summary>
        /// Gets or sets the final rotation rate in radians per second.
        /// </summary>
        public float FinalRate;

        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new RotationRateModifier
            {
                InitialRate = this.InitialRate,
                FinalRate = this.FinalRate,
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
            float initialRateDelta = this.InitialRate * dt;
            float finalRateDelta = this.FinalRate * dt;

            for (int i = 0; i < count; i++)
            {
                float rate = initialRateDelta + ((finalRateDelta - initialRateDelta) * particle->Age);

                particle->Rotate(rate);

                particle++;
            }
        }
    }
}