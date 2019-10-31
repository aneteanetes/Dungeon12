/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Modifiers
{
    using System;
    using System.ComponentModel;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;

    /// <summary>
    /// Defines a Modifier which applies a sine wave force to a Particle over the course of its lifetime.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.SineForceModifierTypeConverter, ProjectMercury.Design")]
#endif
    public sealed class SineForceModifier : Modifier
    {
        private float TotalSeconds;

        /// <summary>
        /// Gets or sets the frequency of the sine wave.
        /// </summary>
        public float Frequency;

        /// <summary>
        /// Gets or sets the amplitude of the sine wave.
        /// </summary>
        public float Amplitude;

        /// <summary>
        /// Gets or sets the rotation of the sine force.
        /// </summary>
        /// <value>The rotation angle in radians.</value>
        public float Rotation
        {
            get { return Calculator.Atan2(this.AngleSin, this.AngleCos); }
            set
            {
                Guard.ArgumentNotFinite("Rotation", value);

                this.AngleCos = Calculator.Cos(value);
                this.AngleSin = Calculator.Sin(value);
            }
        }

        private float AngleCos;
        private float AngleSin;

        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new SineForceModifier
            {
                Amplitude = this.Amplitude,
                Frequency = this.Frequency,
                Rotation = this.Rotation
            };
        }

        /// <summary>
        /// Processes the particles.
        /// </summary>
        /// <param name="dt">Elapsed time in whole and fractional seconds.</param>
        /// <param name="particleArray">A pointer to an array of particles.</param>
        /// <param name="count">The number of particles which need to be processed.</param>
        protected internal override unsafe void Process(float dt, Particle* particleArray, int count)
        {
            this.TotalSeconds += dt;

            float deltaAmp = this.Amplitude * dt;

            for (int i = 0; i < count; i++)
            {
                Particle* particle = (particleArray + i);

                float secondsAlive = this.TotalSeconds - particle->Inception;

                float sin = Calculator.Cos(secondsAlive * this.Frequency);

                Vector2 force = new Vector2(sin, 0f);

                force.X = ((force.X * this.AngleCos) + (force.Y * -this.AngleSin));
                force.Y = ((force.X * this.AngleSin) + (force.Y * this.AngleCos));

                force.X *= deltaAmp;
                force.Y *= deltaAmp;

                particle->Velocity.X += force.X;
                particle->Velocity.Y += force.Y;
            }
        }
    }
}
