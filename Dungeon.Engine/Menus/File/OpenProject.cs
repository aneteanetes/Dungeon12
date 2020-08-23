using Dungeon.Engine.Events;
using Dungeon.Engine.Forms;
using Dungeon.Engine.Projects;
using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace Dungeon.Engine.Menus.File
{
    public class OpenProject : IEngineMenuItem
    {
        public string Text => "Открыть";

        public string Tag => nameof(FileMenu);

        public Action Click => () =>
        {
            using var dialog = new OpenFileDialog();
            dialog.Filter = "Dungeon Engine Project (.deproj)|*.deproj";
            var result = dialog.ShowDialog();

            var path = dialog.FileName;
            if(string.IsNullOrEmpty(path))
            {
                return;
            }

            using var db = new LiteDatabase(path);
            var dirName = Path.GetFileNameWithoutExtension(path);
            var proj = db.GetCollection<DungeonEngineProject>().FindOne(x => x.Name == dirName);

            if (proj == default)
            {
                System.Windows.MessageBox.Show("Directory does not contains DungeonEngine project!");
                return;
            }

            DungeonGlobal.Events.Raise(new ProjectInitializeEvent(proj));
        };
        public int Weight => 1;
    }
}
