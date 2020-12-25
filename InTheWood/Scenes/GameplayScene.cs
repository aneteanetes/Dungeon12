using Dungeon;
using Dungeon.Control;
using Dungeon.Control.Keys;
using Dungeon.Control.Pointer;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Monogame.Effects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using InTheWood.Entities.MapScreen;
using InTheWood.SceneObjects.MapObjects;
using System;
using System.Runtime.InteropServices;

namespace InTheWood.Scenes
{
    public class GameplayScene : StartScene
    {
        public override bool AbsolutePositionScene => false;

        public override bool Destroyable => true;

        public GameplayScene(SceneManager sceneManager) : base(sceneManager) { }

        MapSceneObject mapObj;

        SceneLayer mapLayer;
        SceneLayer backLayer;

        public override void Init()
        {
            backLayer = this.AddLayer(nameof(backLayer));
            backLayer.AddObject(new DarkRectangle()
            {
                Opacity = 1,
                Height = 720,
                Width = 1280,
                Color = ConsoleColor.Yellow
            });
            backLayer.AddObject(new ImageControl("Images/Levels/1.png".AsmRes())
            {
                Width = 1280,
                Height = 720
            });

            mapLayer = this.AddLayer(nameof(mapLayer));
            mapLayer.AddGlobalEffect(new Light2D());
            mapLayer.Top = 50;
            mapLayer.Left = 300;

            var map = new Map();
            map.SetMap(9, 7);

            mapObj = new MapSceneObject(map);
            mapObj.Scale = .8;

            mapLayer.AddObject(mapObj);
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

        protected override void MouseWheel(MouseWheelEnum mouseWheel)
        {
            return;
            if (mouseWheel == MouseWheelEnum.Down)
            {
                mapObj.Scale -= 0.1;
            }
            else
            {
                mapObj.Scale += 0.1;
            }


            base.MouseWheel(mouseWheel);
        }

        private double prevX;
        private double prevY;

        public double MouseSensitivity { get; set; } = 3;

        protected override void MouseMove(PointerArgs pointerArgs)
        {
            return;
            var camera = DungeonGlobal.Camera;
            if (moveCamera)
            {
                camera.SetCameraSpeed(5.5);

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