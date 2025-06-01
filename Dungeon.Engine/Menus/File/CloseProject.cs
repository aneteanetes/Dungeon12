using Dungeon.Engine.Events;
using System;

namespace Dungeon.Engine.Menus.File
{
    public class CloseProject : IEngineMenuItem
    {
        public string Text => "Закрыть";

        public string Tag => nameof(FileMenu);

        public Action Click => () =>
        {
            DungeonGlobal.Events.Raise(new ProjectInitializeEvent(default));
        };
        public int Weight => 2;
    }
}
