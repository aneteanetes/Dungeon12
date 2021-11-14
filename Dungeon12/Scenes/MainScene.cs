using Dungeon;
using Dungeon.Control;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Resources;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon.Types;
using Dungeon.View;
using Dungeon12.Components;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.Entities;
using Dungeon12.SceneObjects.UserInterface.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

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


            string[] heroes_str = new string[] { "Protector", "Assassin", "Priest", "Necromancer" };

            List<Hero> heroes = new List<Hero>();

            heroes_str.ForEach(h =>
            {
                var spritesheet = ResourceLoader.Load($"Images/Dolls/{h}/spritesheet.sf".AsmRes());
                var hero = new Hero()
                {
                    PhysicalObject = new Entities.MapRelated.MapObject()
                    {
                        Size = new Dungeon.Physics.PhysicalSize()
                        {
                            Height = 8,
                            Width = 8
                        }
                    },
                    WalkSpriteSheet = SpriteSheet.Load(spritesheet.Stream.AsString())
                };
                hero.WalkSpriteSheet.DefaultFramePosition = Point.Zero.Copy();
                hero.WalkSpriteSheet.Image = $"Dolls/{h}/spritesheet.png".AsmImg();
                heroes.Add(hero);
            });


            var x = 1352.5;
            var y = 1442.5;

            heroes[0].PhysicalObject.Position = new Dungeon.Physics.PhysicalPosition()
            {
                X = x,
                Y = y
            };

            heroes[1].PhysicalObject.Position = new Dungeon.Physics.PhysicalPosition()
            {
                X = x-24,
                Y = y-16
            };
            heroes[2].PhysicalObject.Position = new Dungeon.Physics.PhysicalPosition()
            {
                X = x + 24,
                Y = y - 16
            };
            heroes[3].PhysicalObject.Position = new Dungeon.Physics.PhysicalPosition()
            {
                X = x,
                Y = y - 24
            };

            var party = new Party()
            {
                Hero1 = heroes[0],
                Hero2 = heroes[1],
                Hero3 = heroes[2],
                Hero4 = heroes[3],
            };

            objs.AddObject(new PartySceneObject(party));

            //var party = new Party
            //{
            //    ,
            //    FrameAnimated = FrameAnimated.FromTileset("Classes/Warrior/sprite.png".AsmImg(), 32, 32)
            //};

            //var partyscobj = new PlayerSceneObject(party);
            //objs.AddObject(partyscobj);

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