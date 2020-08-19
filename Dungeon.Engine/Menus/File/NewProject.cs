using Dungeon.Engine.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Dungeon.Engine.Menus.File
{
    public class NewProject : IEngineMenuItem
    {
        public string Text => "Новый проект";

        public string Tag => nameof(FileMenu);

        public Action Click => () =>
        {
            new ProjectForm().Show();
        };
    }
}
