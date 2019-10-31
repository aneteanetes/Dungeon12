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

    /// <summary>
    /// Defines a Modifier which constrains &amp; deflects particles inside a rectangle.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.RectangleConstraintDeflectorTypeConverter, ProjectMercury.Design")]
#endif
    public sealed class RectangleConstraintDeflector : Modifier
    {
        /// <summary>
        /// Defines the position of the rectangle boundary constraint.
        /// </summary>
        public Vector2 Position;

        private float _width;

        /// <summary>
        /// Gets or sets the width of the rectangle deflector.
        /// </summary>
        /// <value>The width of the rectangle deflector.</value>
        public float Width
        {
            get { return this._width; }
            set
            {
                Guard.ArgumentNotFinite("Width", value);
                Guard.ArgumentLessThan("Width", value, 0f);

                this._width = value;
            }
        }

        private float _height;

        /// <summary>
        /// Gets or sets the height of the rectangle.
        /// </summary>
        /// <value>The height of the rectangle.</value>
        public float Height
        {
            get { return this._height; }
            set
            {
                Guard.ArgumentNotFinite("Height", value);
                Guard.ArgumentLessThan("Height", value, 0f);

                this._height = value;
            }
        }

        private VariableFloat _restitutionCoefficient;

        /// <summary>
        /// Gets or sets the restitution coefficient (bounce factor) of Particles when the hit the deflector.
        /// </summary>
        public VariableFloat RestitutionCoefficient
        {
            get { return this._restitutionCoefficient; }
            set { this._restitutionCoefficient = value; }
        }

        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new RectangleConstraintDeflector
            {
                Height = this.Height,
                Position = this.Position,
                RestitutionCoefficient = this.RestitutionCoefficient,
                Width = this.Width
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
            float left = this.Position.X;
            float right = this.Position.X + this.Width;
            float top = this.Position.Y;
            float bottom = this.Position.Y + this.Height;

            for (int i = 0; i < count; i++)
            {
                Particle* particle = (particleArray + i);

                float halfScale = particle->Scale * 0.5f;

                if (particle->Position.X < left)
                {
                    particle->Position.X = left;

                    float momentumDelta = this.RestitutionCoefficient.Sample();
                    
                    //momentumDelta = momentumDelta > 1f ? 1f : momentumDelta;
                    //momentumDelta = momentumDelta < 0f ? 0f : momentumDelta;

                    particle->Momentum.X *= -momentumDelta;
                }
                else if (particle->Position.X > right)
                {
                    particle->Position.X = right;

                    float momentumDelta = this.RestitutionCoefficient.Sample();
                    
                    //momentumDelta = momentumDelta > 1f ? 1f : momentumDelta;
                    //momentumDelta = momentumDelta < 0f ? 0f : momentumDelta;

                    particle->Momentum.X *= -momentumDelta;
                }

                if (particle->Position.Y < top)
                {
                    particle->Position.Y = top;

                    float momentumDelta = this.RestitutionCoefficient.Sample();
                    
                    //momentumDelta = momentumDelta > 1f ? 1f : momentumDelta;
                    //momentumDelta = momentumDelta < 0f ? 0f : momentumDelta;

                    particle->Momentum.Y *= -momentumDelta;
                }
                else if (particle->Position.Y > bottom)
                {
                    particle->Position.Y = bottom;

                    float momentumDelta = this.RestitutionCoefficient.Sample();
                    
                    //momentumDelta = momentumDelta > 1f ? 1f : momentumDelta;
                    //momentumDelta = momentumDelta < 0f ? 0f : momentumDelta;

                    particle->Momentum.Y *= -momentumDelta;
                }
            }
        }
    }
}