namespace Dungeon
{
    /// <summary>
    /// хм, а нахуя я заполифилил рандом?
    /// <para>
    /// ааа, для того что бы всегда был один инстанс что бы не тупило
    /// </para>
    /// </summary>
    public static class RandomRogue
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

        public static int Range(int start, int end)
        {
            lock (syncLock)
            {
                return Next(start, end + 1);
            }
        }
    }
}