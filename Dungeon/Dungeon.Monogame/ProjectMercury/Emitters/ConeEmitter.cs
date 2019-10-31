/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Emitters
{
    using System;
    using System.ComponentModel;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines an Emitter which releases particles in a beam which gradually becomes wider.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Emitters.ConeEmitterTypeConverter, ProjectMercury.Design")]
#endif
    public class ConeEmitter : Emitter
    {
        private float _direction;

        /// <summary>
        /// The angle (in radians) that the ConeEmitters beam is facing.
        /// </summary>
        public float Direction
        {
            get { return this._direction; }
            set
            {
                this._direction = value;

                // Recalculate the cone extents...
                this.CalculateConeExtents();
            }
        }

        private float HalfConeAngle;

        /// <summary>
        /// The angle (in radians) from edge to edge of the ConeEmitters beam.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the specified value is either
        /// too small or too large.</exception>
        public float ConeAngle
        {
            get { return this.HalfConeAngle + this.HalfConeAngle; }
            set
            {
                Guard.ArgumentNotFinite("ConeAngle", value);
                Guard.ArgumentLessThan("ConeAngle", value, float.Epsilon);
                Guard.ArgumentGreaterThan("ConeAngle", value, Calculator.TwoPi);

                this.HalfConeAngle = value * 0.5f;

                // Recalculate the cone extents...
                this.CalculateConeExtents();
            }
        }

        private Vector2 ConeExtents;

        /// <summary>
        /// Calculates the extents of the cone based on the direction and cone angle.
        /// </summary>
        private void CalculateConeExtents()
        {
            this.ConeExtents = new Vector2
            {
                X = this.Direction - this.HalfConeAngle,
                Y = this.Direction + this.HalfConeAngle
            };
        }

        /// <summary>
        /// Returns an unitialised deep copy of the Emitter.
        /// </summary>
        /// <returns>A deep copy of the Emitter.</returns>
        public override Emitter DeepCopy()
        {
            Emitter copy = new ConeEmitter
            {
                ConeAngle = this.ConeAngle,
                Direction = this.Direction
            };

            base.CopyBaseFields(copy);

            return copy;
        }

        /// <summary>
        /// Generates an offset vector and force vector for a Particle when it is released.
        /// </summary>
        /// <param name="offset">The offset of the Particle from the trigger location.</param>
        /// <param name="force">A unit vector defining the initial force of the Particle.</param>
         protected override void GenerateOffsetAndForce(out Vector2 offset, out Vector2 force)
        {
            offset = Vector2.Zero;

            float radians = RandomHelper.NextFloat(this.ConeExtents.X, this.ConeExtents.Y);

            force = new Vector2
            {
                X = Calculator.Cos(radians),
                Y = Calculator.Sin(radians)
            };
        }
    }
}