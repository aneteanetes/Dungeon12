/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Defines helper methods for validating arguments passed into methods.
    /// </summary>
    static internal class Guard
    {
        /// <summary>
        /// Performs a check against an argument, and throws a ArgumentNullException if it is null.
        /// </summary>
        /// <param name="parameter">The name of the method parameter.</param>
        /// <param name="argument">The value being passed as an argument.</param>
        [Conditional("DEBUG")]
        static public void ArgumentNull(string parameter, object argument)
        {
            if (argument == null)
                throw new ArgumentNullException(parameter);
        }

        /// <summary>
        /// Performs a check against a string argument, and throws an ArgumentNullException if it is
        /// null or empty.
        /// </summary>
        /// <param name="parameter">The name of the method parameter.</param>
        /// <param name="argument">The value being passed as an argument.</param>
        [Conditional("DEBUG")]
        static public void ArgumentNullOrEmpty(string parameter, string argument)
        {
            if (String.IsNullOrEmpty(argument))
                throw new ArgumentNullException(parameter);
        }

        /// <summary>
        /// Performs a check against a method argument, and throws an ArgumentOutOfRangeException if it is
        /// less than the specified threshold.
        /// </summary>
        /// <typeparam name="T">The type of argument being checked.</typeparam>
        /// <param name="parameter">The name of the method parameter.</param>
        /// <param name="argument">The value being passed as an argument.</param>
        /// <param name="threshold">The threshold value that the argument must be equal to or greater than
        /// to pass the test.</param>
        [Conditional("DEBUG")]
        static public void ArgumentLessThan<T>(string parameter, T argument, T threshold) where T : IComparable<T>
        {
            if (argument.CompareTo(threshold) < 0)
                throw new ArgumentOutOfRangeException(parameter);
        }

        /// <summary>
        /// Performs a check against a method argument, and throws an ArgumentOutOfRangeException if it is
        /// greater than the specified threshold.
        /// </summary>
        /// <typeparam name="T">The type of argument being checked.</typeparam>
        /// <param name="parameter">The name of the method parameter.</param>
        /// <param name="argument">The value being passed as an argument.</param>
        /// <param name="threshold">The threshold value that the argument must be equal to or less than
        /// to pass the test.</param>
        [Conditional("DEBUG")]
        static public void ArgumentGreaterThan<T>(string parameter, T argument, T threshold) where T : IComparable<T>
        {
            if (argument.CompareTo(threshold) > 0)
                throw new ArgumentOutOfRangeException(parameter);
        }

        /// <summary>
        /// Performs a check against a method argument, and throws an ArgumentOutOfRangeException if it is
        /// greater than the specified maximum value, or less than the specified minimum value.
        /// </summary>
        /// <typeparam name="T">The type of argument being checked.</typeparam>
        /// <param name="parameter">The name of the method parameter.</param>
        /// <param name="argument">The value being passed as an argument.</param>
        /// <param name="min">The minimum allowed value (inclusive).</param>
        /// <param name="max">The maximum allowed value (inclusive).</param>
        [Conditional("DEBUG")]
        static public void ArgumentOutOfRange<T>(string parameter, T argument, T min, T max) where T : IComparable<T>
        {
            if ((argument.CompareTo(min) < 0) || (argument.CompareTo(max) > 0))
                throw new ArgumentOutOfRangeException(parameter);
        }

        /// <summary>
        /// Performs a check against a method argument, and throws a NotFiniteNumberException if it is not a
        /// finite number eg NaN, PositiveInfinity or NegetiveInfinity.
        /// </summary>
        /// <param name="parameter">The name of the method parameter.</param>
        /// <param name="argument">The value being passed as an argument.</param>
        [Conditional("DEBUG")]
        static public void ArgumentNotFinite(string parameter, float argument)
        {
            if (float.IsNaN(argument) || float.IsNegativeInfinity(argument) || float.IsPositiveInfinity(argument))
                throw new NotFiniteNumberException((double)argument);
        }

        /// <summary>
        /// Throws an InvalidOperationException if the specified expression evaluates to true.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <param name="message">The error message.</param>
        [Conditional("DEBUG")]
        static public void IsTrue(bool expression, string message)
        {
            if (expression == true)
                throw new InvalidOperationException(message);
        }

        /// <summary>
        /// Throws an InvalidOperationException if the specified expression evaluates to false.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <param name="message">The error message.</param>
        [Conditional("DEBUG")]
        static public void IsFalse(bool expression, string message)
        {
            if (expression == false)
                throw new InvalidOperationException(message);
        }
    }
}