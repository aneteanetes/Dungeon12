/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury
{
    using System;

    /// <summary>
    /// Encapsulates common mathematical functions.
    /// </summary>
    public static class Calculator
    {
        /// <summary>
        /// Represents the value of pi.
        /// </summary>
        public const float Pi = 3.141593f;

        /// <summary>
        /// Represents the value of pi times two.
        /// </summary>
        public const float TwoPi = 6.283185f;

        /// <summary>
        /// Represents the value of pi divided by two.
        /// </summary>
        public const float PiOver2 = 1.570796f;

        /// <summary>
        /// Represents the value of pi divided by four.
        /// </summary>
        public const float PiOver4 = 0.7853982f;

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value. If value is less than min, min will be returned.</param>
        /// <param name="max">The maximum value. If value is greater than max, max will be returned.</param>
        /// <returns>The clamped value.</returns>
        static public float Clamp(float value, float min, float max)
        {
            value = (value > max) ? max : value;
            value = (value < min) ? min : value;

            return value;
        }

        /// <summary>
        /// Restricts a value to be within the specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="range">The range of allowable values.</param>
        /// <returns>The clamped value.</returns>
        static public float Clamp(float value, Range range)
        {
            value = (value > range.Maximum) ? range.Maximum : value;
            value = (value < range.Minimum) ? range.Minimum : value;

            return value;
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value. If value is less than min, value will be assigned min.</param>
        /// <param name="max">The maximum value. If value is greater than max, value will be assigned max.</param>
        static public void Clamp(ref float value, float min, float max)
        {
            value = (value > max) ? max : value;
            value = (value < min) ? min : value;
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="range">The range of allowable values.</param>
        static public void Clamp(ref float value, Range range)
        {
            value = (value > range.Maximum) ? range.Maximum : value;
            value = (value < range.Minimum) ? range.Minimum : value;
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value. If value is less than min, min will be returned.</param>
        /// <param name="max">The maximum value. If value is greater than max, max will be returned.</param>
        /// <returns>The clamped value.</returns>
        static public T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            value = value.CompareTo(max) > 0 ? max : value;
            value = value.CompareTo(min) < 0 ? min : value;

            return value;
        }

        /// <summary>
        /// Wraps the specified value to be within the specified range.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The wrapped value.</returns>
        static public float Wrap(float value, float min, float max)
        {
            float range = max - min;

            if (value < min)
                do
                    value += range;
                while (value < min);

            else if (value > max)
                do
                    value -= range;
                while (value > max);

            return value;
        }

        /// <summary>
        /// Wraps the specified value to be within the specified range.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <param name="range">The range of allowable values.</param>
        /// <returns>The wrapped value.</returns>
        static public float Wrap(float value, Range range)
        {
            if (value < range.Minimum)
                do
                    value += range.Size;
                while (value < range.Minimum);

            else if (value > range.Maximum)
                do
                    value -= range.Size;
                while (value > range.Maximum);

            return value;
        }

        /// <summary>
        /// Wraps the specified value to be within the specified range.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        static public void Wrap(ref float value, float min, float max)
        {
            float range = max - min;

            if (value < min)
                do
                    value += range;
                while (value < min);

            else if (value > max)
                do
                    value -= range;
                while (value > max);
        }

        /// <summary>
        /// Wraps the specified value to be within the specified range.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <param name="range">The allowable range of values.</param>
        static public void Wrap(ref float value, Range range)
        {
            if (value < range.Minimum)
                do
                    value += range.Size;
                while (value < range.Minimum);

            else if (value > range.Maximum)
                do
                    value -= range.Size;
                while (value > range.Maximum);
        }

        /// <summary>
        /// Linearly interpolates between two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        /// <returns>Interpolated value.</returns>
        static public float LinearInterpolate(float value1, float value2, float amount)
        {
            return value1 + ((value2 - value1) * amount);
        }

        /// <summary>
        /// Linearly interpolates between two values.
        /// </summary>
        /// <param name="value">The output value.</param>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        static public void LinearInterpolate(ref float value, float value1, float value2, float amount)
        {
            value = value1 + ((value2 - value1) * amount);
        }

        /// <summary>
        /// Linearly interpolates between three values, where the position of the middle value is variable.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="value2Position">The position of the second source value between 0 and 1.</param>
        /// <param name="value3">Source value.</param>
        /// <param name="amount">Value between 0 and 1 indicating the position in the curve to evaluate.</param>
        /// <returns>Interpolated value.</returns>
        static public float LinearInterpolate(float value1, float value2, float value2Position, float value3, float amount)
        {
            if (amount < value2Position)
                return LinearInterpolate(value1, value2, amount / value2Position);

            else
                return LinearInterpolate(value2, value3, (amount - value2Position) / (1f - value2Position));
        }

        /// <summary>
        /// Interpolates between two values using a cubic equation.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Weighting value.</param>
        /// <returns>Interpolated value.</returns>
        static public float CubicInterpolate(float value1, float value2, float amount)
        {
            Calculator.Clamp(ref amount, 0f, 1f);

            return Calculator.LinearInterpolate(value1, value2, (amount * amount) * (3f - (2f * amount)));
        }

        /// <summary>
        /// Interpolates between two values using a cubic equation.
        /// </summary>
        /// <param name="value">The output value.</param>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Weighting value.</param>
        static public void CubicInterpolate(ref float value, float value1, float value2, float amount)
        {
            Calculator.Clamp(ref amount, 0f, 1f);

            Calculator.LinearInterpolate(ref value, value1, value2, (amount * amount) * (3f - (2f * amount)));
        }

        /// <summary>
        /// Returns the greater of two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <returns>The greater value.</returns>
        static public float Max(float value1, float value2)
        {
            return value1 >= value2 ? value1 : value2;
        }

        /// <summary>
        /// Returns the greater of three values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="value3">Source value.</param>
        /// <returns>The greater value.</returns>
        static public float Max(float value1, float value2, float value3)
        {
            return value2 >= value3 ? (value1 >= value2 ? value1 : value2) : value1 >= value3 ? value1 : value3;
        }

        /// <summary>
        /// Sets value to be the greater of two values.
        /// </summary>
        /// <param name="value">The output value.</param>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        static public void Max(ref float value, float value1, float value2)
        {
            value = value1 >= value2 ? value1 : value2;
        }

        /// <summary>
        /// Sets value to the the greater of three values.
        /// </summary>
        /// <param name="value">The output value.</param>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="value3">Source value.</param>
        static public void Max(ref float value, float value1, float value2, float value3)
        {
            value = value2 >= value3 ? (value1 >= value2 ? value1 : value2) : value1 >= value3 ? value1 : value3;
        }

        /// <summary>
        /// Returns the greater of two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <returns>The greater value, or value1 if the values are equal.</returns>
        static public T Max<T>(T value1, T value2) where T : IComparable<T>
        {
            return value1.CompareTo(value2) >= 0 ? value1 : value2;
        }

        /// <summary>
        /// Returns the greater of three values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="value3">Source value.</param>
        /// <returns>The greater value, or value1 if the values are equal.</returns>
        static public T Max<T>(T value1, T value2, T value3) where T : IComparable<T>
        {
            return value2.CompareTo(value3) >= 0 ? (value1.CompareTo(value2) >= 0 ? value1 : value2) : value1.CompareTo(value3) >= 0 ? value1 : value3;
        }

        /// <summary>
        /// Returns the lesser of two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <returns>The lesser value.</returns>
        static public float Min(float value1, float value2)
        {
            return value1 <= value2 ? value1 : value2;
        }

        /// <summary>
        /// Returns the lesser of three values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="value3">Source value.</param>
        /// <returns>The lesser value.</returns>
        static public float Min(float value1, float value2, float value3)
        {
            return value2 <= value3 ? (value1 <= value2 ? value1 : value2) : value1 <= value3 ? value1 : value3;
        }

        /// <summary>
        /// Sets value to be the lesser of two values.
        /// </summary>
        /// <param name="value">The output value.</param>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        static public void Min(ref float value, float value1, float value2)
        {
            value = value1 <= value2 ? value1 : value2;
        }

        /// <summary>
        /// Sets value to be the lesser of three values.
        /// </summary>
        /// <param name="value">The output value.</param>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="value3">Source value.</param>
        static public void Min(ref float value, float value1, float value2, float value3)
        {
            value = value2 <= value3 ? (value1 <= value2 ? value1 : value2) : value1 <= value3 ? value1 : value3;
        }

        /// <summary>
        /// Returns the lesser of two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <returns>The lesser value, or value1 if the values are equal.</returns>
        static public T Min<T>(T value1, T value2) where T : IComparable<T>
        {
            return value1.CompareTo(value2) <= 0 ? value1 : value2;
        }

        /// <summary>
        /// Returns the lesser of three values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="value3">Source value.</param>
        /// <returns>The lesser value, or value1 if the values are equal.</returns>
        static public T Min<T>(T value1, T value2, T value3) where T : IComparable<T>
        {
            return value2.CompareTo(value3) <= 0 ? (value1.CompareTo(value2) <= 0 ? value1 : value2) : value1.CompareTo(value3) <= 0 ? value1 : value3;
        }

        /// <summary>
        /// Returns the absolute value of a single precision floating point number.
        /// </summary>
        /// <param name="value">Source value.</param>
        /// <returns>The absolute vaue of the source value.</returns>
        static public float Abs(float value)
        {
            return value >= 0 ? value : -value;
        }

        /// <summary>
        /// Assigns the absolute value of a single precision floating point number.
        /// </summary>
        /// <param name="value">Source value.</param>
        static public void Abs(ref float value)
        {
            value = value >= 0 ? value : -value;
        }

        /// <summary>
        /// Returns the angle whose cosine is the specified value.
        /// </summary>
        /// <param name="value">A number representing a cosine.</param>
        /// <returns>The angle whose cosine is the specified value.</returns>
        static public float Acos(float value)
        {
            return (float)Math.Acos((double)value);
        }

        /// <summary>
        /// Returns the angle whose sine is the specified value.
        /// </summary>
        /// <param name="value">A number representing a sine.</param>
        /// <returns>The angle whose sine is the specified value.</returns>
        static public float Asin(float value)
        {
            return (float)Math.Asin((double)value);
        }

        /// <summary>
        /// Returns the angle whos tangent is the speicified number.
        /// </summary>
        /// <param name="value">A number representing a tangent.</param>
        /// <returns>The angle whos tangent is the speicified number.</returns>
        static public float Atan(float value)
        {
            return (float)Math.Atan((double)value);
        }

        /// <summary>
        /// Returns the angle whose tangent is the quotient of the two specified numbers.
        /// </summary>
        /// <param name="y">The y coordinate of a point.</param>
        /// <param name="x">The x coordinate of a point.</param>
        /// <returns>The angle whose tangent is the quotient of the two specified numbers.</returns>
        static public float Atan2(float y, float x)
        {
            return (float)Math.Atan2((double)y, (double)x);
        }

        /// <summary>
        /// Returns the sine of the specified angle.
        /// </summary>
        /// <param name="value">An angle specified in radians.</param>
        /// <returns>The sine of the specified angle.</returns>
        static public float Sin(float value)
        {
            return (float)Math.Sin((double)value);
        }

        /// <summary>
        /// Returns the hyperbolic sine of the specified angle.
        /// </summary>
        /// <param name="value">An angle specified in radians.</param>
        /// <returns>The hyperbolic sine of the specified angle.</returns>
        static public float Sinh(float value)
        {
            return (float)Math.Sinh((double)value);
        }

        /// <summary>
        /// Returns the cosine of the specified angle.
        /// </summary>
        /// <param name="value">An angle specified in radians.</param>
        /// <returns>The cosine of the specified angle.</returns>
        static public float Cos(float value)
        {
            return (float)Math.Cos((double)value);
        }

        /// <summary>
        /// Returns the hyperbolic cosine of the specified angle.
        /// </summary>
        /// <param name="value">An angle specified in radians.</param>
        /// <returns>The hyperbolic cosine of the specified angle.</returns>
        static public float Cosh(float value)
        {
            return (float)Math.Cosh((double)value);
        }

        /// <summary>
        /// Returns the tangent of the specified angle.
        /// </summary>
        /// <param name="value">An angle specified in radians.</param>
        /// <returns>The tangent of the specified angle.</returns>
        static public float Tan(float value)
        {
            return (float)Math.Tan((double)value);
        }

        /// <summary>
        /// Returns the hyperbolic tangent of the specified angle.
        /// </summary>
        /// <param name="value">An angle specified in radians.</param>
        /// <returns>The hyperbolic tangent of the specified angle.</returns>
        static public float Tanh(float value)
        {
            return (float)Math.Tanh((double)value);
        }

        /// <summary>
        /// Returns the natural (base e) logarithm of the specified value.
        /// </summary>
        /// <param name="value">A number whose logarithm is to be found.</param>
        /// <returns>The natural (base e) logarithm of the specified value.</returns>
        static public float Log(float value)
        {
            return (float)Math.Log((double)value);
        }

        /// <summary>
        /// Returns the specified value raised to the specified power.
        /// </summary>
        /// <param name="value">Source value.</param>
        /// <param name="power">A single precision floating point number that specifies a power.</param>
        /// <returns>The specified value raised to the specified power.</returns>
        static public float Pow(float value, float power)
        {
            return (float)Math.Pow((double)value, (double)power);
        }

        /// <summary>
        /// Returns the square root of the specified value.
        /// </summary>
        /// <param name="value">Source value.</param>
        /// <returns>The square root of the specified value.</returns>
        static public float Sqrt(float value)
        {
            return (float)Math.Sqrt((double)value);
        }
    }
}