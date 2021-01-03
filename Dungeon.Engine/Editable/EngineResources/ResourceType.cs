namespace Dungeon.Engine.Projects
{
    public enum ResourceType
    {
        [Value("Файл")]
        File = 9,

        [Value("Системное")]
        Embedded = 8,

        [Value("Изображение")]
        Image = 1,

        [Value("Шрифт")]
        Font = 2,

        [Value("3D Модель")]
        Model3D = 7,

        [Value("Музыка")]
        Music = 3,

        [Value("Звук")]
        Audio = 4,

        [Value("Частицы")]
        Particle = 5,

        [Value("Шейдер")]
        Shader = 6,

        [Value("Папка")]
        Folder = 0,
    }
}