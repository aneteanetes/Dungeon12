namespace Dungeon12.SceneObjects.World
{
    public static class WorldSettings
    {
        public static int _width = 21;
        public static int _height = 15;

        public static int _widthBorder = 4;
        public static int _heightBorder = 3;

        public static int _widthOffset => ((_width-1)-(_widthBorder*2))/2;
        public static int _heightOffset => ((_height-1)-(_heightBorder*2))/2;

        public static double cellSize = 64;
    }
}