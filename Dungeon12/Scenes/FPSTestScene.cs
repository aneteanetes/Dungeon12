using Dungeon.Drawing;
using Dungeon.SceneObjects.Base;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;

namespace Dungeon12.Scenes
{
    internal class FPSTestScene : Dungeon.Scenes.GameScene
    {
        public FPSTestScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => false;

        public override void Initialize()
        {
            var main = this.CreateLayer("main");

            for (int y = 0; y < 150; y++)
            {
                for (int x = 0; x < 150; x++)
                {
                    main.AddObject(new ColoredRectangle<FPSTestScene>(this)
                    {
                        Width=10,
                        Height=10,
                        Color = DrawColor.GreenYellow,
                        Left =x*5,
                        Top=y*5,
                    });
                }
            }
        }
    }
}
