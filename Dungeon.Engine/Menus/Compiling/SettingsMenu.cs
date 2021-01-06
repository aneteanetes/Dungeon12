﻿using Dungeon.Engine.Events;
using Dungeon.Engine.Forms;
using Dungeon.Engine.Projects;
using System;
using System.Windows;

namespace Dungeon.Engine.Menus.File
{
    public class SettingsMenu : IEngineMenuItem
    {
        public string Text => "Настройки";

        public string Tag => nameof(CompilationMenu);

        public Action Click => () =>
        {
            var proj = App.Container.Resolve<EngineProject>();
            if (proj == default)
            {
                Message.Show("Project is not loaded!");
                return;
            }

            if (proj.CompileSettings == default)
            {
                proj.CompileSettings = new ProjectSettings();
            }

            DungeonGlobal.Events.Raise(new PropGridFillEvent(proj.CompileSettings));
        };
        public int Weight => 0;
    }
}