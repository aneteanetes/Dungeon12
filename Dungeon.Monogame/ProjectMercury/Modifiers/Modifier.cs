/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Modifiers
{
    using System;

    /// <summary>
    /// Defines the base class for an object which modifies Particle values.
    /// </summary>
    public abstract class Modifier
    {
        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        public abstract Modifier DeepCopy();

        /// <summary>
        /// Processes the particles.
        /// </summary>
        /// <param name="elapsedSeconds">Elapsed time in whole and fractional seconds.</param>
        /// <param name="particleArray">A pointer to the first particle in an array of particles.</param>
        /// <param name="count">The number of particles which need to be processed.</param>
        protected internal unsafe abstract void Process(float elapsedSeconds, Particle* particle, int count);
    }
}