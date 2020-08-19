using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Engine.Menus.File
{
    public class FileMenu : IEngineMenuItem
    {
        public string Text => "Файл";

        public string Tag => default;

        public Action Click => default;
    }
}
