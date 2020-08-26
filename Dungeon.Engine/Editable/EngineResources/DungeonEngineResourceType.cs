using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Dungeon.Engine.Projects
{
    public enum DungeonEngineResourceType
    {
        [Value("Изображение")]
        Image,
        [Value("Шрифт")]
        Font,
        [Value("3D Модель")]
        Model3D,
        [Value("Музыка")]
        Music,
        [Value("Звук")]
        Audio,
        [Value("Частицы")]
        Particle,
        [Value("Шейдер")]
        Shader
    }
}
