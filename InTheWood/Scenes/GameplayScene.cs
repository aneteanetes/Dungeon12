using Dungeon;
using Dungeon.Control;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Force.DeepCloner;
using InTheWood.Entities.MapScreen;
using InTheWood.SceneObjects.MapObjects;
using InTheWood.Shaders.Bloom;
using System;

namespace InTheWood.Scenes
{
    public class GameplayScene : StartScene
    {
        public override bool AbsolutePositionScene => false;

        public override bool Destroyable => true;

        public GameplayScene(SceneManager sceneManager) : base(sceneManager) { }

        public override void Init()
        {
            var map = new Map();
            var centerSector = new Sector();
            map.AddSector(centerSector);

            var leftSector = new Sector();
            leftSector.Status = MapStatus.Friendly;
            map.AddSector(leftSector, new SectorConnection(centerSector, leftSector)
            {
                ConnectDirection = Dungeon.Types.SimpleDirection.Left,
                Position = 2,
                Offset = 1
            });

            var rightSector = new Sector();
            rightSector.Status = MapStatus.Hostile;
            map.AddSector(rightSector, new SectorConnection(centerSector, rightSector)
            {
                ConnectDirection = Dungeon.Types.SimpleDirection.Right,
                Position = 2,
                Offset = 1
            });

            var mapObj = new MapSceneObject(map)
            {
                Left = 450,
                Top = 100,
            };
            mapObj.Scale = .5;

            this.AddObject(mapObj);
            var bloomFilter = new BloomFilter
            {
                AfterLoad = bf => bf.BloomPreset = BloomFilter.BloomPresets.SuperWide
            };
            this.AddGlobalEffect(bloomFilter);
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape)
            {
                Global.Exit();
            }

            base.KeyPress(keyPressed, keyModifiers, hold);
        }

        bool moveCamera = false;

        protected override void MousePress(PointerArgs pointerArgs)
        {
            if(pointerArgs.MouseButton== Dungeon.Control.Pointer.MouseButton.Left)
            {
                moveCamera = true;
            }
        }

        protected override void MouseRelease(PointerArgs pointerArgs)
        {
            if (pointerArgs.MouseButton == Dungeon.Control.Pointer.MouseButton.Left)
            {
                moveCamera = false;
            }
        }

        private double prevX;
        private double prevY;

        public double MouseSensitivity { get; set; } = 5;

        protected override void MouseMove(PointerArgs pointerArgs)
        {
            var camera = DungeonGlobal.Camera;
            if (moveCamera)
            {
                camera.SetCameraSpeed(3.5);

                var xSensitivity = Math.Abs(pointerArgs.X - prevX) >= MouseSensitivity;
                var ySensitivity = Math.Abs(pointerArgs.Y - prevY) >= MouseSensitivity;

                if (pointerArgs.X >= prevX && xSensitivity)
                {
                    camera.MoveCamera(Dungeon.Types.Direction.Left,once:true);
                }
                else if (pointerArgs.X <= prevX && xSensitivity)
                {
                    camera.MoveCamera(Dungeon.Types.Direction.Right, once: true);
                }

                if (pointerArgs.Y > prevY && ySensitivity)
                {
                    camera.MoveCamera(Dungeon.Types.Direction.Up, once: true);
                }
                else if (pointerArgs.Y < prevY && ySensitivity)
                {
                    camera.MoveCamera(Dungeon.Types.Direction.Down, once: true);
                }
            }
            else
            {
                camera.SetCameraSpeed(2.5);
            }

            prevX = pointerArgs.X;
            prevY = pointerArgs.Y;

            base.MouseMove(pointerArgs);
        }
    }
}