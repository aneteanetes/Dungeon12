/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Emitters
{
    using System;

    /// <summary>
    /// Contains extension methods for the Emitter class which aid backwards compatibility
    /// with previous releases of the engine.
    /// </summary>
    public static class EmitterCompatibilityExtensions
    {
        /// <summary>
        /// Initializes the Emitter.
        /// </summary>
        /// <remarks>The 'Initialize' method was renamed to 'Initialise' in revision 49307 (21st April 2009).</remarks>
        [Obsolete("Use 'Initialise' method instead.", false)]
        static public void Initialize(this Emitter emitter)
        {
            emitter.Initialise();
        }

        /// <summary>
        /// Updates the Emitter.
        /// </summary>
        /// <param name="emitter">The extended instance.</param>
        /// <param name="totalSeconds">Total game time in whole and fractional seconds.</param>
        /// <param name="deltaSeconds">Elapsed game time in whole and fractional seconds.</param>
        [Obsolete("Use Update(deltaSeconds) method instead.", false)]
        static public void Update(this Emitter emitter, float totalSeconds, float deltaSeconds)
        {
            emitter.Update(deltaSeconds);
        }
    }
}