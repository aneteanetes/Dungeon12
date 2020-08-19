using Dungeon.Control;
using Dungeon.Control.Keys;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon12.SceneObjects.UI;
using Dungeon12.Scenes.Menus;

namespace Dungeon12.Scenes
{
    public class SplashScene : StartScene<Start>
    {
        public SplashScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        //public override void FatalException()
        //{
        //    MessageBox.Show("Произошла фатальная ошибка, требуется перезапустить игру.", () => { Global.Exit?.Invoke(); });
        //}

        AnimatedSplash Splash;

        public override void Init()
        {
            Splash = new AnimatedSplash();
            this.AddObject(Splash);

            base.Init();
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if(Splash.CanSwitch)
            {
                this.Switch<Start>();
            }
        }

        protected override void MousePress(PointerArgs pointerArgs)
        {
            if (Splash.CanSwitch)
            {
                this.Switch<Start>();
            }
        }
    }
}