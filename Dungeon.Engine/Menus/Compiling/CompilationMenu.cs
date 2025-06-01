using Dungeon.Engine.Events;
using Dungeon.Engine.Projects;
using Dungeon.Engine.Utils;
using Dungeon.Resources;
using System;
using System.IO;
using System.Resources;

namespace Dungeon.Engine.Menus.File
{
    public class CompilationMenu : IEngineMenuItem
    {
        public string Text => "Компиляция";

        public string Tag => default;

        public Action Click => default;

        public int Weight => 2;
    }
}
