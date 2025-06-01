using Dungeon.Engine.Forms;
using Dungeon.Engine.Projects;
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
            var proj = App.Container.Resolve<EngineProject>();
            new DllManagerForm(proj).Show();
        }; 

        public int Weight => 0;
    }
}
