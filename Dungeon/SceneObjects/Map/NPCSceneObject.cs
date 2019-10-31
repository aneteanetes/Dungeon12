namespace Dungeon.Drawing.SceneObjects.Map
{
    using Dungeon.Control.Events;
    using Dungeon.Control.Keys;
    using Dungeon.Control.Pointer;
    using Dungeon.Drawing.SceneObjects.Dialogs.NPC;
    using Dungeon.Drawing.SceneObjects.Dialogs.Shop;
    using Dungeon.Drawing.SceneObjects.Effects;
    using Dungeon.Entites.Alive;
    using Dungeon.Entites.Animations;
    using Dungeon.Map;
    using Dungeon.Map.Objects;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;

    public class NPCSceneObject : MoveableSceneObject<NPC>
    {
        public override string Cursor => @object.Merchant == null
            ? "speak"
            : "shop";

        public NPCSceneObject(PlayerSceneObject playerSceneObject, GameMap location, NPC mob, Rectangle defaultFramePosition)
            : base(playerSceneObject, mob, location, mob, mob.NPCEntity, defaultFramePosition)
        {
            this.Image = mob.Tileset;
            Left = mob.Location.X;
            Top = mob.Location.Y;
            Width = 1;
            Height = 1;

            mob.Die += () =>
            {
                this.Destroy?.Invoke();
            };

            if (mob.NPCEntity.Idle != null)
            {
                this.SetAnimation(mob.NPCEntity.Idle);
            }

            LightTrigger = Global.Time
                .After(18).Do(AddTorchlight)
                .After(8).Do(RemoveTorchlight);
        }

        private TorchlightInHandsSceneObject torchlight;

        private void AddTorchlight()
        {
            torchlight = new TorchlightInHandsSceneObject();
            this.AddChild(torchlight);
        }

        private void RemoveTorchlight()
        {
            this.RemoveChild(torchlight);
            torchlight?.Destroy?.Invoke();
        }

        private readonly TimeTrigger LightTrigger;

        protected override void DrawLoop()
        {
            LightTrigger.Trigger();
            base.DrawLoop();
        }
        
        protected override void Action(MouseButton mouseButton)
        {
            playerSceneObject.StopMovings();
            var sceneObj = Act();
            ShowEffects?.Invoke(sceneObj.InList());
        }

        private ISceneObject Act() => @object.Merchant == null
            ? (ISceneObject)new NPCDialogue(playerSceneObject, @object, this.DestroyBinding, this.ControlBinding,location)
            : (ISceneObject)new ShopWindow(@object.Name, playerSceneObject, @object.Merchant, this.DestroyBinding, this.ControlBinding, location);

        protected override void StopAction()
        {
        }

        protected override Key[] KeyHandles => new Key[] { Key.LeftShift };

        protected override Dictionary<int, (Direction dir, Vector vect, Func<Moveable, AnimationMap> anim)> DirectionMap => new Dictionary<int, (Direction, Vector, Func<Moveable, AnimationMap>)>
        {
            { 0,(Direction.Up, Vector.Minus, m=>
                {
                    if(torchlight != null)
                    {
                        torchlight.Left=0;
                    }
                    return m.MoveUp;
                })
            },
            { 1,(Direction.Down, Vector.Plus, m=>
                {
                    if(torchlight != null)
                    {
                        torchlight.Left=0;
                    }
                    return m.MoveDown;
                })
            },
            { 2,(Direction.Left, Vector.Minus, m=>
                {
                    if(torchlight != null)
                    {
                        torchlight.Left=0.4;
                    }
                    return m.MoveLeft;
                })
            },
            { 3,(Direction.Right, Vector.Plus, m=>
                {
                    if(torchlight != null)
                    {
                        torchlight.Left=0.2;
                    }
                    return m.MoveRight;
                })
            },
        };

        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
        }
    }
}