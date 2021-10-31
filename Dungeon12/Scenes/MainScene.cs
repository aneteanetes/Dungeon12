using Dungeon;
using Dungeon.Control;
using Dungeon.Control.Gamepad;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon.Tiled;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12.Entities;
using Dungeon12.SceneObjects.Map;
using Dungeon12.SceneObjects.UserInterface.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.Scenes
{
    public class MainScene : GameScene<StartScene>
    {
        public MainScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => false;

        public override bool AbsolutePositionScene => false;

        public override void Initialize()
        {
            back = this.CreateLayer("map");
            back.AddObject(new ImageObject("Maps/FaithIsland.png".AsmImg()));

            var objs = this.CreateLayer("obj");
            var party = new Party
            {
                Size=new Dungeon.Physics.PhysicalSize()
                {
                    Height = 32,
                    Width = 32
                },
                Position = new Dungeon.Physics.PhysicalPosition()
                {
                    X = 1352.5,
                    Y = 1442.5
                },
                Tileset = "Classes/Warrior/sprite.png".AsmImg(),
                TileSetRegion = new Rectangle()
                {
                    X = 0,
                    Y = 0,
                    Height = 32,
                    Width = 32
                }
            };

            var partyscobj = new PartySceneObject(party);
            objs.AddObject(partyscobj);

            Global.Camera.SetCamera(-627.5, -1027.5);
        }

        SceneLayer back;

        protected override void MousePress(PointerArgs pointerArgs)
        {
            back.AddObject(new PopupText($"{pointerArgs.X}, {pointerArgs.Y} ({pointerArgs.GameCoordinates.X}, {pointerArgs.GameCoordinates.Y})".AsDrawText(), pointerArgs.AsPoint, speed: 0.5)
            {
                Time = TimeSpan.FromSeconds(0.7),
            });

            base.MousePress(pointerArgs);
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Right)
            {
                Global.Camera.MoveCamera(Direction.Right);
            }
            if (keyPressed == Key.Left)
            {
                Global.Camera.MoveCamera(Direction.Left);
            }
            if (keyPressed == Key.Down)
            {
                Global.Camera.MoveCamera(Direction.Down);
            }
            if (keyPressed == Key.Up)
            {
                Global.Camera.MoveCamera(Direction.Up);
            }

            if (keyPressed == Key.Escape)
                this.Switch<StartScene>();

            base.KeyPress(keyPressed, keyModifiers, hold);
        }

        protected override void KeyUp(Key keyPressed, KeyModifiers keyModifiers)
        {
            Global.Camera.StopMoveCamera();
            base.KeyUp(keyPressed, keyModifiers);
        }
    }
}