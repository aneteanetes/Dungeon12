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
    /// Defines an Emitter which releases Particles at a random point along a line.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Emitters.LineEmitterTypeConverter, ProjectMercury.Design")]
#endif
    public class LineEmitter : Emitter
    {
        private float HalfLength;

        /// <summary>
        /// Gets or sets the length of the line.
        /// </summary>
        public float Length
        {
            get { return this.HalfLength + this.HalfLength; }
            set
            {
                Guard.ArgumentNotFinite("Length", value);
                Guard.ArgumentLessThan("Length", value, 0f);

                this.HalfLength = value * 0.5f;
            }
        }

        /// <summary>
        /// Gets or sets the rotation of the line around its middle point.
        /// </summary>
        public float Angle
        {
            get { return Calculator.Atan2(this.AngleSin, this.AngleCos); }
            set
            {
                Guard.ArgumentNotFinite("Angle", Angle);

                this.AngleCos = Calculator.Cos(value);
                this.AngleSin = Calculator.Sin(value);
            }
        }

        private float AngleCos = Calculator.Cos(0f);
        private float AngleSin = Calculator.Sin(0f);

        /// <summary>
        /// If true, will emit particles perpendicular to the angle of the line.
        /// </summary>
        public bool Rectilinear;

        /// <summary>
        /// If true, will emit particles both ways. Only work when Rectilinear is enabled.
        /// </summary>
        public bool EmitBothWays;

        /// <summary>
        /// Returns an unitialised deep copy of the Emitter.
        /// </summary>
        /// <returns>A deep copy of the Emitter.</returns>
        public override Emitter DeepCopy()
        {
            Emitter copy = new LineEmitter
            {
                Angle        = this.Angle,
                Length       = this.Length,
                Rectilinear  = this.Rectilinear,
                EmitBothWays = this.EmitBothWays
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
            float lineOffset = RandomHelper.NextFloat(-this.HalfLength, this.HalfLength);

            offset = new Vector2
            {
                X = (lineOffset * this.AngleCos),
                Y = (lineOffset * this.AngleSin),
            };

            if (this.Rectilinear)
            {
                force = new Vector2 { X = this.AngleSin, Y = -this.AngleCos };

                if (this.EmitBothWays)
                {
                    if (RandomHelper.NextBool())
                    {
                        force.X *= -1f;
                        force.Y *= -1f;
                    }
                }
            }
            else
            {
                force = RandomHelper.NextUnitVector();
            }
        }
    }
}