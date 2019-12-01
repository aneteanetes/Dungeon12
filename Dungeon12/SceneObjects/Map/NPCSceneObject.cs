namespace Dungeon.Drawing.SceneObjects.Map
{
    using Dungeon.Control.Keys;
    using Dungeon.Control.Pointer;
    using Dungeon.Entities.Alive;
    using Dungeon.Entities.Animations;
    using Dungeon.Map;
    using Dungeon.Map.Objects;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using Dungeon12.Drawing.SceneObjects.Dialogs.Shop;
    using Dungeon12.Drawing.SceneObjects.Effects;
    using System;
    using Dungeon;
    using System.Collections.Generic;
    using Dungeon12.Drawing.SceneObjects;
    using Dungeon12.SceneObjects.NPC;
    using Dungeon.SceneObjects;

    public class NPCSceneObject : MoveableSceneObject<NPC>
    {
        public override string Cursor => @object.Merchant == null
            ? "speak"
            : "shop";

        protected override bool SilentTooltip => true;

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

            LightTrigger = Dungeon.Global.Time
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
            if (!@object.NoInteract)
            {
                playerSceneObject.StopMovings();
                var sceneObj = Act();
                ShowInScene?.Invoke(sceneObj.InList());
            }
            else
            {
                MessageBox.Show(@object.NoInteractText, ShowInScene);
            }
        }

        private ISceneObject Act() => @object.Merchant == null
            ? (ISceneObject)new NPCDialogue(playerSceneObject, @object, this.DestroyBinding, this.ControlBinding,location, new MetallButtonControl("Выход"))
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