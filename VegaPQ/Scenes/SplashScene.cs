using Dungeon;
using Dungeon.Control;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon12.SceneObjects.UI;
using System;
using System.Collections.Generic;
using System.Text;
using VegaPQ.SceneObjects;

namespace VegaPQ.Scenes
{
    public class SplashScene : StartScene<MenuScene>
    {
        public SplashScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void FatalException()
        {
            MessageBox.Show("Произошла фатальная ошибка, требуется перезапустить игру.", () => { Dungeon.DungeonGlobal.Exit?.Invoke(); });
        }

        public override void Init()
        {
            this.AddObject(new ImageControl("back.png".AsmImgRes()));

            this.AddObject(new Game());

            base.Init();
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            
        }

        protected override void MousePress(PointerArgs pointerArgs)
        {
        }
    }
}
