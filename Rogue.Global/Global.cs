namespace Rogue
{
    using Rogue.View.Interfaces;

    public static class Global
    {
        public static IDrawClient DrawClient;

        public static object FreezeWorld = null;

        public static object FreezeControls = null;

        public static object Freezed => FreezeWorld ?? FreezeControls;
    }
}