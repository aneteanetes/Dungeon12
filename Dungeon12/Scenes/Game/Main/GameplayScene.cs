using Dungeon;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon12.SceneObjects.GUI.Main;
using Dungeon12.Scenes.Menus;
using System;

namespace Dungeon12.Scenes.Game
{
    public class GameplayScene : GameScene<MainMenuScene>
    {
        public GameplayScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => false;

        SceneLayer UILayer;
        SceneLayer MapLayer;

        public override void Initialize()
        {
            MapLayer = this.AddLayer(nameof(MapLayer));
            MapLayer.AddObject(new ImageControl("Splash/Island_Part.png".AsmImg()));

            InitializeUI();
        }

        private void InitializeUI()
        {
            UILayer = this.AddLayer(nameof(UILayer));

            UILayer.AddObject(new Minimap()
            {
                Left = 1597
            });

            // left panel
            var left = 30;
            for (int i = 0; i < 4; i++)
            {
                UILayer.AddObject(new GameplaySquareButton("")
                {
                    Left = left,
                    Top = 975
                });

                left += 75;
            }

            // right panel
            left = 1590;
            for (int i = 0; i < 4; i++)
            {
                UILayer.AddObject(new GameplaySquareButton("")
                {
                    Left = left,
                    Top = 975
                });

                left += 75;
            }

            // characters

            UILayer.AddObject(new CharacterPanel()
            {
                Left = 706,
                Top = 840
            });

            UILayer.AddObject(new CharacterPanel()
            {
                Left = 706,
                Top = 959
            });

            UILayer.AddObject(new CharacterPanel()
            {
                Left = 970,
                Top = 840
            });

            UILayer.AddObject(new CharacterPanel()
            {
                Left = 970,
                Top = 959
            });
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape)
                this.Switch<MainMenuScene>();

            base.KeyPress(keyPressed, keyModifiers, hold);
        }
    }
}