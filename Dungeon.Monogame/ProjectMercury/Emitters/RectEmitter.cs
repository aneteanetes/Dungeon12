/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Emitters
{
    using System.ComponentModel;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines an Emitter which releases particles in a rectangle shape.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Emitters.RectEmitterTypeConverter, ProjectMercury.Design")]
#endif
    public class RectEmitter : Emitter
    {
        private float HalfWidth;

        /// <summary>
        /// Gets or sets the width of the rectangle.
        /// </summary>
        /// <value>The width of the rectangle.</value>
        public float Width
        {
            get { return this.HalfWidth + this.HalfWidth; }
            set
            {
                Guard.ArgumentNotFinite("Width", value);
                Guard.ArgumentLessThan("Width", value, 0f);

                this.HalfWidth = value * 0.5f;
            }
        }

        private float HalfHeight;

        /// <summary>
        /// Gets or sets the height of the rectangle..
        /// </summary>
        /// <value>The height of the rectangle.</value>
        public float Height
        {
            get { return this.HalfHeight + this.HalfHeight; }
            set
            {
                Guard.ArgumentNotFinite("Height", value);
                Guard.ArgumentLessThan("Height", value, 0f);

                this.HalfHeight = value * 0.5f;
            }
        }

        /// <summary>
        /// Gets or sets the rotation of the rectangle.
        /// </summary>
        /// <value>The rotation of the rectangle measured in radians.</value>
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

        private float AngleCos = Calculator.Cos(0f);
        private float AngleSin = Calculator.Sin(0f);

        /// <summary>
        /// True if the Particles should be released only from the edge of the rectangle, else false.
        /// </summary>
        public bool Frame;

        /// <summary>
        /// Returns an unitialised deep copy of the Emitter.
        /// </summary>
        /// <returns>A deep copy of the Emitter.</returns>
        public override Emitter DeepCopy()
        {
            Emitter copy = new RectEmitter
            {
                Frame = this.Frame,
                Height = this.Height,
                Rotation = this.Rotation,
                Width = this.Width
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
#if XBOX
            offset = new Vector2();
#endif
            if (this.Frame)
            {
                if (RandomHelper.NextBool())
                {
                    offset.X = RandomHelper.ChooseOne(-this.HalfWidth, this.HalfWidth);
                    offset.Y = RandomHelper.NextFloat(-this.HalfHeight, this.HalfHeight);
                }
                else
                {
                    offset.X = RandomHelper.NextFloat(-this.HalfWidth, this.HalfWidth);
                    offset.Y = RandomHelper.ChooseOne(-this.HalfHeight, this.HalfHeight);
                }
            }
            else
            {
                offset.X = RandomHelper.NextFloat(-this.HalfWidth, this.HalfWidth);
                offset.Y = RandomHelper.NextFloat(-this.HalfHeight, this.HalfHeight);
            }

            // Apply the Emitter rotation to the offset vector...
            Vector2 unrotatedOffset = offset;

            offset.X = ((unrotatedOffset.X * this.AngleCos) + (unrotatedOffset.Y * -this.AngleSin));
            offset.Y = ((unrotatedOffset.X * this.AngleSin) + (unrotatedOffset.Y * this.AngleCos));

            force = RandomHelper.NextUnitVector();
        }
    }
}