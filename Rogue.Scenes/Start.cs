namespace Rogue.Scenes
{
    using System;
    using Rogue.Scenes.Scenes;

    public class Start : GameScene
    {
        public Start(SceneManager @null) : base(@null)
        {
        }

        public override bool Destroyable => true;

        public override void Render()
        {
        }
    }
}
