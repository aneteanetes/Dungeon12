using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace VegaPQ.Scenes
{
    public class MenuScene : GameScene
    {
        public MenuScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => throw new NotImplementedException();
    }
}
