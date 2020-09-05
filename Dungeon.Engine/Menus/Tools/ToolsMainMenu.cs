using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Engine.Menus.Tools
{
    public class ToolsMainMenu : IEngineMenuItem
    {
        public string Text => "Инструменты";

        public string Tag => default;

        public Action Click => default;

        public int Weight => 4;
    }
}
