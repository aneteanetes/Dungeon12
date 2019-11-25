using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon12.Drawing.SceneObjects;
using Dungeon12.SceneObjects.SaveLoad;
using Dungeon12.Scenes.Game;
using Dungeon12.Scenes.Menus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Dungeon.Control.Keys;

namespace Dungeon12.Scenes.SaveLoad
{
    public class SaveLoadScene : GameScene<Main, Start>
    {
        public SaveLoadScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        private bool isGame;

        public override void Init()
        {
            isGame = Args.ElementAtOrDefault(0) != default;
            var isSave = Args.ElementAtOrDefault(1) != default;

            this.AddObject(new Background());
            this.AddObject(new SaveLoadWindow(isSave, () => this.Switch<Main>())
            {
                Left = 8,
                Top = 2.5
            });
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape)
            {
                this.Switch<Start>(isGame ? new string[1] : default);
            }
        }
    }
}
