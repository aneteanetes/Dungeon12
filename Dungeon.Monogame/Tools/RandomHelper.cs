/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace Dungeon.Monogame.Tools
{
    using System;
    using Microsoft.Xna.Framework;
    using ProjectMercury;
    using Range = ProjectMercury.Range;

    /// <summary>
    /// Defines helper methods for choosing random numbers or performing random operations.
    /// </summary>
    public static class RandomHelper
    {
        static readonly object Padlock = new object();

        /// <summary>
        /// Initializes the <see cref="RandomHelper"/> class.
        /// </summary>
        static RandomHelper()
        {
            Random = new Random();
        }

        /// <summary>
        /// Gets or sets the random number generator.
        /// </summary>
        /// <value>The random.</value>
        static private Random Random { get; set; }

        /// <summary>
        /// Returns a non-negetive random whole number.
        /// </summary>
        static public int NextInt()
        {
            lock (Padlock)
            {
                return Random.Next();
            }
        }

        /// <summary>
        /// Returns a non-negetive random whole number less than the specified maximum.
        /// </summary>
        /// <param name="max">The exclusive upper bound the random number to be generated.</param>
        static public int NextInt(int max)
        {
            lock (Padlock)
            {
                return Random.Next(max);
            }
        }

        /// <summary>
        /// Returns a random number within a specified range.
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number returned.</param>
        /// <param name="max">The exclusive upper bound of the random number returned.</param>
        static public int NextInt(int min, int max)
        {
            lock (Padlock)
            {
                return Random.Next(min, max);
            }
        }

        /// <summary>
        /// Returns a random float between 0.0 and 1.0.
        /// </summary>
        static public float NextFloat()
        {
            lock (Padlock)
            {
                return (float)Random.NextDouble();
            }
        }

        /// <summary>
        /// Returns a random float betwen 0.0 and the specified upper bound.
        /// </summary>
        /// <param name="max">The inclusive upper bound of the random number returned.</param>
        static public float NextFloat(float max)
        {
            return max * NextFloat();
        }

        /// <summary>
        /// Returns a random float within the specified range.
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number returned.</param>
        /// <param name="max">The inclusive upper bound of the random number returned.</param>
        static public float NextFloat(float min, float max)
        {
            return (max - min) * NextFloat() + min;
        }

        /// <summary>
        /// Returns a random float within the specified range.
        /// </summary>
        /// <param name="range">The range of allowable values.</param>
        static public float NextFloat(Range range)
        {
            return (range.Maximum - range.Minimum) * NextFloat() + range.Minimum;
        }

        /// <summary>
        /// Returns a random byte.
        /// </summary>
        static public byte NextByte()
        {
            return (byte)NextInt(255);
        }

        /// <summary>
        /// Returns a random boolean value.
        /// </summary>
        static public bool NextBool()
        {
            return NextInt(2) == 1;
        }

        /// <summary>
        /// Returns a random two dimensional unit vector.
        /// </summary>
        /// <returns>A random two dimensional unit vector.</returns>
        static public Vector2 NextUnitVector()
        {
            lock (Padlock)
            {
                float radians = NextFloat(-Calculator.Pi, Calculator.Pi);

                return new Vector2
                {
                    X = Calculator.Cos(radians),
                    Y = Calculator.Sin(radians)
                };
            }
        }

        /// <summary>
        /// Returns a random variation of the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="variation">The variation amount.</param>
        /// <example>A value of 10 with a variation of 5 will result in a random number between 5.0 and 15.</example>
        static public float Variation(float value, float variation)
        {
            float min = value - variation,
                  max = value + variation;

            return NextFloat(min, max);
        }

        /// <summary>
        /// Chooses a random item from the specified parameters and returns it.
        /// </summary>
        static public int ChooseOne(params int[] values)
        {
            int index = NextInt(values.Length);

            return values[index];
        }

        /// <summary>
        /// Returns a pointer to a random element in the specified array.
        /// </summary>
        /// <param name="valuesArray">A pointer to the first element in an array of integers.</param>
        /// <param name="length">The total number of elements in the array.</param>
        static public unsafe int* ChooseOne(int* valuesArray, int length)
        {
            int index = NextInt(length);

            return valuesArray + index;
        }

        /// <summary>
        /// Chooses a random item from the specified parameters and returns it.
        /// </summary>
        static public float ChooseOne(params float[] values)
        {
            int index = NextInt(values.Length);

            return values[index];
        }

        /// <summary>
        /// Returns a pointer to a random element in the specified array.
        /// </summary>
        /// <param name="valuesArray">A pointer to the first element in an array of floating point values.</param>
        /// <param name="length">The total number of elements in the array.</param>
        static public unsafe float* ChooseOne(float* valuesArray, int length)
        {
            int index = NextInt(length);

            return valuesArray + index;
        }

        /// <summary>
        /// Chooses a random item from the specified parameters and returns it.
        /// </summary>
        static public T ChooseOne<T>(params T[] values)
        {
            int index = NextInt(values.Length);

            return values[index];
        }
    }
}