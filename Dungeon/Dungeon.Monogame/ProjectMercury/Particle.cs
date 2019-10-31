/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines the data structure for a particle.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Particle
    {
        /// <summary>
        /// Gets or sets the position of the particle.
        /// </summary>
        public Vector2 Position;
        
        /// <summary>
        /// Gets or sets the scale of the particle.
        /// </summary>
        public float Scale;

        /// <summary>
        /// Gets or sets the rotation of the particle in radians.
        /// </summary>
        public float Rotation;

        /// <summary>
        /// Gets or sets the colour of the particle. The W component is opacity.
        /// </summary>
        public Vector4 Colour;

        /// <summary>
        ///  Gets or sets the current momentum of the particle.
        /// </summary>
        public Vector2 Momentum;

        /// <summary>
        /// Gets or sets the sum of the forces which are currently acting on the particle.
        /// </summary>
        public Vector2 Velocity;

        /// <summary>
        /// Gets or sets the time at which the particle was released.
        /// </summary>
        public float Inception;

        /// <summary>
        /// Gets or sets the age of the particle in the range 0-1.
        /// </summary>
        public float Age;

        /// <summary>
        /// Applies a force to the particle.
        /// </summary>
        /// <param name="force">A vector describing the force and direction.</param>
        public void ApplyForce(ref Vector2 force)
        {
            this.Velocity.X += force.X;
            this.Velocity.Y += force.Y;
        }

        /// <summary>
        /// Applies a rotation to the Particle.
        /// </summary>
        /// <param name="radians">The angle to rotate in radians.</param>
        public void Rotate(float radians)
        {
            this.Rotation += radians;

            if (this.Rotation > Calculator.Pi)
                this.Rotation -= Calculator.TwoPi;

            else if (this.Rotation < -Calculator.Pi)
                this.Rotation += Calculator.TwoPi;
        }

        /// <summary>
        /// Updates the particle.
        /// </summary>
        /// <param name="deltaSeconds">Elapsed seconds since the last update.</param>
        /// <remarks>This method has been manually inlined in the Emitter base class Update method,
        /// its implementation has been left here for reference &amp; clarity.</remarks>
        [Obsolete("No longer used!")]
        internal void Update(float deltaSeconds)
        {
            // Add velocity to momentum...
            this.Momentum.X += this.Velocity.X;
            this.Momentum.Y += this.Velocity.Y;

            // Set velocity back to zero...
            this.Velocity.X = this.Velocity.Y = 0f;

            // Calculate momentum for this time-step...
            Vector2 deltaMomentum;

            deltaMomentum.X = this.Momentum.X * deltaSeconds;
            deltaMomentum.Y = this.Momentum.Y * deltaSeconds;

            // Add momentum to the particles Position...
            this.Position.X += deltaMomentum.X;
            this.Position.Y += deltaMomentum.Y;
        }
    }
}