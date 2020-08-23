using Dungeon.Engine.Events;
using Dungeon.Engine.Projects;
using Dungeon.Resources;
using System;
using System.IO;
using System.Windows;

namespace Dungeon.Engine.Menus.File
{
    public class ComplieMenu : IEngineMenuItem
    {
        public string Text => "Запуск";

        public string Tag => nameof(CompilationMenu);

        public Action Click => () =>
        {
            var proj = App.Container.Resolve<DungeonEngineProject>();
            if (proj == default)
            {
                MessageBox.Show("Project is not loaded!");
                return;
            }

            DungeonGlobal.Events.Raise(new FreezeAllEvent());


            var res = ResourceLoader.Load($"Templates.Projects.{proj.Type}Project.csproj".AsmRes());
            var projFile = res.Stream.AsString();

            System.IO.File.WriteAllText(Path.Combine(proj.Path, proj.Name, $"{proj.Name}.csproj"), projFile);


            DungeonGlobal.Events.Raise(new StatusChangeEvent("Построение завершилось успешно"));
            DungeonGlobal.Events.Raise(new UnfreezeAllEvent());
        };

        public int Weight => 1;
    }
}
