using Dungeon;
using Dungeon.Control.Keys;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon.View.Interfaces;
using Dungeon12.SceneObjects.GUI.Main;
using Dungeon12.SceneObjects.World;
using Dungeon12.Scenes.Menus;
using Dungeon12.World;
using System;

namespace Dungeon12.Scenes.Game
{
    public class GameplayScene : GameScene<MainMenuScene>
    {
        public override bool AbsolutePositionScene => false;

        public GameplayScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => false;

        SceneLayer UILayer;
        SceneLayer MapLayer;

        public override void Initialize()
        {
            InitializeWorld();
            InitializeUI();
        }

        public GameWorld World { get; set; }

        private void InitializeWorld()
        {
            this.MapLayer = this.CreateLayer(nameof(MapLayer));

            World = new GameWorld();
            var map = World.CreateWorldMap("Island");
            this.MapLayer.AddObject(new WorldMapSceneObject(map));

            Global.Camera.SetCameraSpeed(20);
            //set position create
            //this.sceneManager.DrawClient.SetCamera(-6300, 0);
        }

        private void InitializeUI()
        {
            UILayer = this.CreateLayer(nameof(UILayer));
            UILayer.AbsoluteLayer = true;

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

            UILayer.AddObject(new Position());
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape)
                this.Switch<MainMenuScene>();

            var camera = this.sceneManager.DrawClient;

            if (keyPressed == Key.K)
                this.sceneManager.DrawClient.SetCamera(-6000, -3000);
            if (keyPressed == Key.L)
                this.sceneManager.DrawClient.SetCamera(0, 0);

            if (keyPressed == Key.Left)
                camera.MoveCamera(Dungeon.Types.Direction.Left);
            if (keyPressed == Key.Right)
                camera.MoveCamera(Dungeon.Types.Direction.Right);
            if (keyPressed == Key.Up)
                camera.MoveCamera(Dungeon.Types.Direction.Up);
            if (keyPressed == Key.Down)
                camera.MoveCamera(Dungeon.Types.Direction.Down);

            base.KeyPress(keyPressed, keyModifiers, hold);
        }

        protected override void KeyUp(Key keyPressed, KeyModifiers keyModifiers)
        {
            this.sceneManager.DrawClient.StopMoveCamera();
            base.KeyUp(keyPressed, keyModifiers);
        }

        private class Position : TextControl
        {
            public Position() : base(null) { }

            public override bool AbsolutePosition => true;

            public override bool CacheAvailable => false;

            public override IDrawText Text => new DrawText($"X:{Global.Camera.CameraOffsetX} Y:{Global.Camera.CameraOffsetY}").Montserrat();

            public override double Left => 1750;

            public override double Height => 100;

            public override double Width => this.MeasureText(Text).X / 32;
        }
    }
}