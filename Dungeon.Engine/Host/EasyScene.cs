using Dungeon.Drawing.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Engine.Host
{
    public class EasyScene : StartScene
    {
        public override bool Destroyable => true;

        public EasyScene(SceneManager sceneManager) : base(sceneManager) { }

        public override void Init()
        {
            base.Init();
        }
    }
}
