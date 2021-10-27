using Dungeon;
using Dungeon.Control;
using Dungeon.Control.Gamepad;
using Dungeon.Control.Keys;
using Dungeon.Drawing;
using Dungeon.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon.View.Interfaces;
using Dungeon12.GameObjects.Party;
using Dungeon12.SceneObjects.GUI.Main;
using Dungeon12.Scenes.Menus;
using Dungeon12.World;
using Dungeon12.World.Map;
using System.Linq;

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
        SceneLayer ObjLayer;

        public override void Initialize()
        {
            InitializeWorld();
            InitializeWorldObjects();
            InitializeUI();
        }

        public GameWorld World { get; set; }

        private void InitializeWorldObjects()
        {
            ObjLayer = this.CreateLayer(nameof(ObjLayer));

            var party = World.CreateParty();            
            ObjLayer.AddObject(new PartySceneObject(party));

            party.SceneObject.Top = 4000;
            party.SceneObject.Left = 6200;
        }

        private void InitializeWorld()
        {
            this.MapLayer = this.CreateLayer(nameof(MapLayer));

            World = new GameWorld();
            var map = World.CreateWorldMap("Island");
            this.MapLayer.AddObject(new WorldMapSceneObject(map));

            Global.Camera.SetCameraSpeed(20);
            this.sceneManager.DrawClient.SetCamera(-6040, -3880);
            //X6900 Y6580
        }

        private void InitializeUI()
        {
            UILayer = this.CreateLayer(nameof(UILayer));
            UILayer.AbsoluteLayer = true;

            UILayer.AddObject(new Minimap()
            {
                Left = 1654,
                Top = 8
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

            //camera coords
            UILayer.AddObject(new Position(1750));
            UILayer.AddObject(cursorPos = new Position(DungeonGlobal.Resolution.Width / 2));
        }

        Position cursorPos;

        protected override void MouseMove(PointerArgs pointerArgs)
        {
            cursorPos.X = (Global.Camera.CameraOffsetX * -1) + pointerArgs.X;
            cursorPos.Y = (Global.Camera.CameraOffsetX * -1) + pointerArgs.Y;
            base.MouseMove(pointerArgs);
        }

        protected override void GamePadButtonPress(GamePadButton[] btns)
        {
            if(btns.Contains(GamePadButton.Start))
            {
                this.Switch<MainMenuScene>("InGame");
            }
            base.GamePadButtonPress(btns);
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape)
                this.Switch<MainMenuScene>("InGame");

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
            private double initialLeft;
            public Position(double left) : base(null)
            {
                this.Left = initialLeft = left;
            }

            public override bool AbsolutePosition => true;

            public override bool CacheAvailable => false;

            public override IDrawText Text => new DrawText($"X:{this.X ?? Global.Camera.CameraOffsetX} Y:{this.Y ?? Global.Camera.CameraOffsetY}").Montserrat();

            public double? X { get; set; }

            public double? Y { get; set; }

            public override double Height => 100;

            public override double Width
            {
                get
                {
                    var w = this.MeasureText(Text).X / 32;
                    Left = initialLeft - w / 2;
                    return w;
                }
            }
        }
    }
}