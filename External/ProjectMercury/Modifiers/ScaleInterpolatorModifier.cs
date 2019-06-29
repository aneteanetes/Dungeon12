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
    /// Defines a modifier which changes the scale of particles based on a linear interpolation over three values.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.ScaleInterpolatorModifierTypeConverter, ProjectMercury.Design")]
#endif
    public class ScaleInterpolatorModifier : Modifier
    {
        private float _initialScale;

        /// <summary>
        /// Gets or sets the initial scale.
        /// </summary>
        public float InitialScale
        {
            get { return this._initialScale; }
            set
            {
                Guard.ArgumentLessThan("InitialScale", value, 0f);

                this._initialScale = value;
            }
        }

        private float _middleScale;

        /// <summary>
        /// Gets or sets the middle scale.
        /// </summary>
        public float MiddleScale
        {
            get { return this._middleScale; }
            set
            {
                Guard.ArgumentLessThan("MiddleScale", value, 0f);

                this._middleScale = value;
            }
        }

        private float _middlePosition;

        /// <summary>
        /// Gets or sets the middle scale position.
        /// </summary>
        public float MiddlePosition
        {
            get { return this._middlePosition; }
            set
            {
                Guard.ArgumentOutOfRange("MiddlePosition", value, 0f, 1f);

                this._middlePosition = value;
            }
        }

        private float _finalScale;

        /// <summary>
        /// Gets or sets the final scale.
        /// </summary>
        public float FinalScale
        {
            get { return this._finalScale; }
            set
            {
                Guard.ArgumentLessThan("FinalScale", value, 0f);

                this._finalScale = value;
            }
        }

        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new ScaleInterpolatorModifier
            {
                InitialScale = this.InitialScale,
                MiddleScale = this.MiddleScale,
                MiddlePosition = this.MiddlePosition,
                FinalScale = this.FinalScale
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
            for (int i = 0; i < count; i++)
            {
                Particle* particle = (particleArray + i);

                Particle* previousParticle = (particle - 1);

                if (particle->Age == previousParticle->Age)
                {
                    particle->Scale = previousParticle->Scale;

                    continue;
                }

                if (particle->Age < this.MiddlePosition)
                    particle->Scale = this.InitialScale + ((this.MiddleScale - this.InitialScale) * (particle->Age / this.MiddlePosition));

                else
                    particle->Scale = this.MiddleScale + ((this.FinalScale - this.MiddleScale) * ((particle->Age - this.MiddlePosition) / (1f - this.MiddlePosition)));
            }
        }
    }
}