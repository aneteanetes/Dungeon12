namespace Rogue
{
    /// <summary>
    /// хм, а нахуя я заполифилил рандом?
    /// <para>
    /// ааа, для того что бы всегда был один инстанс что бы не тупило
    /// </para>
    /// </summary>
    public static class Random
    {
        private static System.Random SystemRandom = new System.Random();

        public static int Next() => SystemRandom.Next();

        public static int Next(int maxValue) => SystemRandom.Next(maxValue);

        public static int Next(int minValue, int maxValue) => SystemRandom.Next(minValue, maxValue);

        public static int Range(int start, int end) => Next(start, end+1);
    }
}