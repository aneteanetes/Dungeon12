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
    /// Defines a Modifier which adjusts the scale of Particles based on a sine wave.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.ScaleOscillatorTypeConverter, ProjectMercury.Design")]
#endif
    public class ScaleOscillator : Modifier
    {
        private float TotalSeconds;

        private float _frequency;

        /// <summary>
        /// Gets or sets the oscillator frequency (the number of cycles per second).
        /// </summary>
        public float Frequency
        {
            get { return this._frequency; }
            set
            {
                Guard.ArgumentNotFinite("Frequency", value);
                Guard.ArgumentLessThan("Frequency", value, float.Epsilon);

                this._frequency = value;
            }
        }

        private float _minimum;

        /// <summary>
        /// Gets or sets the minimum scale (the scale of Particles at the negetive peak of the sine wave).
        /// </summary>
        /// <value>The minimum scale.</value>
        public float MinimumScale
        {
            get { return this._minimum; }
            set
            {
                Guard.ArgumentNotFinite("MinimumScale", value);
                Guard.ArgumentLessThan("MinimumScale", value, 0f);

                this._minimum = value;
            }
        }

        private float _maximum;

        /// <summary>
        /// Gets or sets the maximum scale (the scale of Particles at the positive peak of the sine wave).
        /// </summary>
        /// <value>The maximum scale.</value>
        public float MaximumScale
        {
            get { return this._maximum; }
            set
            {
                Guard.ArgumentNotFinite("MaximumScale", value);
                Guard.ArgumentLessThan("MaximumScale", value, 0f);

                this._maximum = value;
            }
        }

        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new ScaleOscillator
            {
                Frequency = this.Frequency,
                MinimumScale = this.MinimumScale,
                MaximumScale = this.MaximumScale
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

            for (int i = 0; i < count; i++)
            {
                Particle* particle = (particleArray + i);

                float secondsAlive = this.TotalSeconds - particle->Inception;

                float sin = Calculator.Sin(secondsAlive * (this.Frequency * 3f));

                particle->Scale = ((this.MaximumScale - this.MinimumScale) * sin) + this.MinimumScale;
            }
        }
    }
}