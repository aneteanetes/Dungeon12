using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon12.Scenes.Game;
using Dungeon12.Scenes.Menus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Scenes.SaveLoad
{
    public class SaveLoadScene : GameScene<Main, Start>
    {
        public SaveLoadScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Init()
        {
            base.Init();
        }
    }
}
