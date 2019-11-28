using System;

namespace Dungeon
{
    /// <summary>
    /// хм, а нахуя я заполифилил рандом?
    /// <para>
    /// ааа, для того что бы всегда был один инстанс что бы не тупило
    /// </para>
    /// </summary>
    public static class RandomDungeon
    {
        private static System.Random SystemRandom = new System.Random();
        private static readonly object syncLock = new object();
        public static int Next()
        {
            lock (syncLock)
            {
                return SystemRandom.Next();
            }
        }

        public static int Next(int maxValue)
        {
            lock (syncLock)
            {
                return SystemRandom.Next(maxValue);
            }
        }

        public static int Next(int minValue, int maxValue)
        {
            lock (syncLock)
            {
                return SystemRandom.Next(minValue, maxValue);
            }
        }

        /// <summary>
        /// Возвращает случайное число между двумя включая оба
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static int Range(int start, int end)
        {
            lock (syncLock)
            {
                return Next(start, end + 1);
            }
        }

        /// <summary>
        /// Возвращает случайное число между двумя включая оба
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static int Range(double start, double end) => Range((int)Math.Ceiling(start), (int)Math.Ceiling(end));

        public static bool Chance(int percentForSucess)
        {
            var i = Range(1, 100);
            return i <= percentForSucess;
        }

        public static bool Chance(long percentForSucess)
        {
            var i = Range(1, 100);
            return i <= percentForSucess;
        }
    }
}