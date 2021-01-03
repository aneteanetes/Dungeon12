using Dungeon.Engine.Forms;
using System;

namespace Dungeon.Engine.Menus.Tools
{
    public class DTREditor : IEngineMenuItem
    {
        public string Text => "Редактор DTR";

        public string Tag => nameof(ToolsMainMenu);

        public Action Click => () =>
        {
            new LiteDbEditorForm().Show();
        };

        public int Weight => 2;
    }
}
