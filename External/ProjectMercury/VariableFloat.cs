/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    /// <summary>
    /// Defines a floating point object which has a definable random variation.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.VariableFloatTypeConverter, ProjectMercury.Design")]
#endif
    public struct VariableFloat : IEquatable<VariableFloat>
    {
        /// <summary>
        /// The base value for the VariableFloat.
        /// </summary>
        public float Value;

        /// <summary>
        /// The range of the random variation around the base value.
        /// </summary>
        public float Variation;

        /// <summary>
        /// Samples the VariableFloat.
        /// </summary>
        /// <returns>A randomised float value.</returns>
        public float Sample()
        {
            if (Calculator.Abs(this.Variation) <= float.Epsilon)
                return this.Value;

            return RandomHelper.NextFloat(this.Value - this.Variation, this.Value + this.Variation);
        }

        /// <summary>
        /// Samples the VariableFloat, and clamps the result to be within the specified range.
        /// </summary>
        /// <param name="clampRange">The range of allowable values.</param>
        /// <returns>A randomised float value.</returns>
        public float Sample(Range clampRange)
        {
            if (Calculator.Abs(this.Variation) <= float.Epsilon)
                return Calculator.Clamp(this.Value, clampRange);

            float value = RandomHelper.NextFloat(this.Value - this.Variation, this.Value + this.Variation);

            return Calculator.Clamp(value, clampRange);
        }

        /// <summary>
        /// Implicit cast operator from float to VariableFloat.
        /// </summary>
        static public implicit operator VariableFloat(float value)
        {
            return new VariableFloat
            {
                Value     = value,
                Variation = 0f
            };
        }

        /// <summary>
        /// Implicit cast operation from VariableFloat to float.
        /// </summary>
        static public implicit operator float(VariableFloat value)
        {
            return value.Sample();
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is VariableFloat)
                return this.Equals((VariableFloat)obj);

            return false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(VariableFloat other)
        {
            return this.Value == other.Value &&
                this.Variation == other.Variation;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode() + this.Variation.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> representation of the object.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> representation of the object.
        /// </returns>
        public override string ToString()
        {
            CultureInfo culture = CultureInfo.CurrentCulture;

            return String.Format("{{Value:{0}, Variation:{1}}}", this.Value.ToString(culture), this.Variation.ToString(culture));
        }
    }
}