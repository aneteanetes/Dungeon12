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
    /// Defines a Modifier which merges the scale of particles towards a single scale over their lifetime. Works best
    /// when Particles are being released with random scale, where you require the particles to have a uniform scale
    /// at the end of their lifetime.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.ScaleMergeModifierTypeConverter, ProjectMercury.Design")]
#endif
    public sealed class ScaleMergeModifier : Modifier
    {
        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new ScaleMergeModifier
            {
                MergeScale = this.MergeScale
            };
        }

        private float _mergeScale;

        /// <summary>
        /// Gets or sets the final scale of Particles when they are retired.
        /// </summary>
        /// <value>The merge scale.</value>
        public float MergeScale
        {
            get { return this._mergeScale; }
            set
            {
                Guard.ArgumentNotFinite("MergeScale", value);
                Guard.ArgumentLessThan("MergeScale", value, 0f);

                this._mergeScale = value;
            }
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

                float a = particle->Age * 0.07f,
                      aInv = 1f - a;

                particle->Scale = (particle->Scale * aInv) + (this.MergeScale * a);
            }
        }
    }
}