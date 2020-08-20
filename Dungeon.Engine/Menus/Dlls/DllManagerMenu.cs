using Dungeon.Engine.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Engine.Menus.Dlls
{
    public class DllManagerMenu : IEngineMenuItem
    {
        public string Text => "Менеджер библиотек";

        public string Tag => nameof(DllMenu);

        public Action Click => () =>
        {
            new DllManagerForm().Show();
        }; 

        public int Weight => 0;
    }
}
