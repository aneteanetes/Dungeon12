using Dungeon.Control.Keys;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;

namespace Demo.Scenes
{
    public class Start : StartScene
    {
        public override bool Destroyable => true;
        public Start(SceneManager sceneManager) : base(sceneManager) { }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if(keyPressed== Key.Escape)
            {
                Global.Exit();
            }

            base.KeyPress(keyPressed, keyModifiers, hold);
        }
    }
}
