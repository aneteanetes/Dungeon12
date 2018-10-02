namespace Rogue.Scenes.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Rogue.Control.Keys;
    using Rogue.DataAccess;
    using Rogue.Drawing.Data;
    using Rogue.Drawing.GUI;
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.Labirinth;
    using Rogue.Map;
    using Rogue.Scenes.Menus;
    using Rogue.Scenes.Scenes;
    using Rogue.Settings;

    public class MainScene : GameScene<MainMenuScene>
    {
        private readonly DrawingSize DrawingSize = new DrawingSize();

        public MainScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => false;

        public override void Draw()
        {
            this.FillCommands();

            if (this.Location == null)
                this.InitMap();

            Drawing.Draw.Session<GUIBorderDrawSession>()
                .Then<LabirinthDrawSession>(x => x.Location = this.Location)
                .Then<CharMapDrawSession>(x => x.Commands = this.Commands.Select(c => $"[{c.Key}] - {c.Name}").ToArray())
                .Then<CharacterDataDrawSession>(x => x.Player = this.Player)
                .Then<MessageDrawSession>(x => x.Message = new DrawText($"{DateTime.Now.ToShortTimeString()}: Вы прибываете в столицу", ConsoleColor.DarkGray))
                .Publish();
        }

        private void FillCommands()
        {
            this.Commands.Add(new Control.Commands.Command { Key = Key.E, Name = "Действие" });
            this.Commands.Add(new Control.Commands.Command { Key = Key.F, Name = "Подобрать" });
            this.Commands.Add(new Control.Commands.Command { Key = Key.C, Name = "Персонаж" });
            this.Commands.Add(new Control.Commands.Command { Key = Key.I, Name = "Инвентарь" });
            this.Commands.Add(new Control.Commands.Command { Key = Key.Q, Name = "Атаковать" });
            this.Commands.Add(new Control.Commands.Command { Key = Key.Z, Name = "Осмотреться" });
            this.Commands.Add(new Control.Commands.Command { Key = Key.R, Name = "Способности" });
            this.Commands.Add(new Control.Commands.Command { Key = Key.Escape, Name = "Меню" });
        }

        private void InitMap()
        {
            this.Location = new Location
            {
                Biom = ConsoleColor.DarkGray
            };

            var persistMap = Database.Entity<Data.Maps.Map>(x => x.Identity == "Capital").First();

            this.Location.Name = persistMap.Name;

            foreach (var line in persistMap.Template.Split(Environment.NewLine))
            {
                var listLine = new List<Map.MapObject>();

                foreach (var @char in line)
                {
                    listLine.Add(MapObject.Create(@char.ToString()));
                }

                this.Location.Map.Add(listLine);
            }
        }

        public override void KeyPress(Key keyPressed, KeyModifiers keyModifiers)
        {
            if (keyPressed == Key.Escape)
                this.Switch<MainMenuScene>();
        }
    }
}
