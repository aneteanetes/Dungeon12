using Dungeon.Engine.Events;
using Dungeon.Engine.Forms;
using System;

namespace Dungeon.Engine.Menus.Tools
{
    public class ResMenu : IEngineMenuItem
    {
        public string Text => "Ресурсы";

        public string Tag => nameof(ToolsMainMenu);

        public Action Click => () =>
        {
            new AddResourceForm().Show();
        };

        public int Weight => 1;
    }
}
