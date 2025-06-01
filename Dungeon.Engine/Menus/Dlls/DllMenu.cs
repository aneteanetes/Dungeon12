using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Engine.Menus.Dlls
{
    public class DllMenu : IEngineMenuItem
    {
        public string Text => "Библиотеки";

        public string Tag => default;

        public Action Click => default;

        public int Weight => 1;
    }
}
