namespace Rogue
{
    public static class Random
    {
        private static System.Random SystemRandom = new System.Random();

        public static int Next() => SystemRandom.Next();

        public static int Next(int maxValue) => SystemRandom.Next(maxValue);

        public static int Next(int minValue, int maxValue) => SystemRandom.Next(minValue, maxValue);
    }
}