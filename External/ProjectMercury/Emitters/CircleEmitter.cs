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
    /// Defines an Emitter which releases Particles in a circle or ring shape.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Emitters.CircleEmitterTypeConverter, ProjectMercury.Design")]
#endif
    public class CircleEmitter : Emitter
    {
        private float _radius;

        /// <summary>
        /// Defines the radius of the circle.
        /// </summary>
        public float Radius
        {
            get { return this._radius; }
            set
            {
                Guard.ArgumentNotFinite("Radius", value);
                Guard.ArgumentLessThan("Radius", value, float.Epsilon);

                this._radius = value;
            }
        }

        /// <summary>
        /// True if particles should be spawned only on the edge of the circle, else false.
        /// </summary>
        public bool Ring;

        /// <summary>
        /// True if particles should radiate away from the center of the circle, else false.
        /// </summary>
        public bool Radiate;

        /// <summary>
        /// Returns an unitialised deep copy of the Emitter.
        /// </summary>
        /// <returns>A deep copy of the Emitter.</returns>
        public override Emitter DeepCopy()
        {
            CircleEmitter copy = new CircleEmitter
            {
                Radius = this.Radius,
                Radiate = this.Radiate,
                Ring = this.Ring
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
            Vector2 angle = RandomHelper.NextUnitVector();

            float radiusMultiplier = this.Ring ? this.Radius : this.Radius * RandomHelper.NextFloat();

            offset = new Vector2
            {
                X = angle.X * radiusMultiplier,
                Y = angle.Y * radiusMultiplier,
            };

            force = this.Radiate ? angle : RandomHelper.NextUnitVector();
        }
    }
}