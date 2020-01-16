namespace Dungeon12.Drawing.SceneObjects.Map
{
    using Dungeon;
    using Dungeon.Control.Keys;
    using Dungeon.Control.Pointer;
    using Dungeon.Physics;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using Dungeon12.Abilities.Enums;
    using Dungeon12.Drawing.SceneObjects;
    using Dungeon12.Drawing.SceneObjects.Dialogs.Shop;
    using Dungeon12.Drawing.SceneObjects.Effects;
    using Dungeon12.Drawing.SceneObjects.UI;
    using Dungeon12.Entities;
    using Dungeon12.Entities.Alive;
    using Dungeon12.Entities.Fractions;
    using Dungeon12.Map;
    using Dungeon12.Map.Objects;
    using Dungeon12.SceneObjects;
    using Dungeon12.SceneObjects.NPC;
    using System.Collections.Generic;
    using System.Linq;

    public class NPCSceneObject : MoveableSceneObject<NPCMap>
    {
        protected override bool SilentTooltip => true;

        protected override Key AlternativeTooltipKey => Key.LeftControl;

        private NPC NPC => @object.Entity;

        public override string Cursor
        {
            get
            {
                if (@object.IsEnemy)
                    return "attackmeele";

                return @object.Merchant == null
                    ? "speak"
                    : "shop";
            }
        }

        public NPCSceneObject(PlayerSceneObject playerSceneObject, GameMap location, NPCMap mob, Rectangle defaultFramePosition)
            : base(playerSceneObject, mob, location, mob, mob.Entity, defaultFramePosition)
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

            if (mob.Entity.Idle != null)
            {
                this.SetAnimation(mob.Entity.Idle);
            }

            if (!mob.IsEnemy)
            {
                LightTrigger = Dungeon12.Global.Time
                    .After(18).Do(AddTorchlight)
                    .After(8).Do(RemoveTorchlight);
            }
            else
            {
                this.AddChild(new ObjectHpBar(mob.Entity));
            }

            attackTimer = Global.Time.Timer()
                .After(1000)
                .Recoverable()
                .Do(() => attackAvailable = true);

            this.Destroy += () => attackTimer.Dispose();
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
            if (!@object.IsEnemy)
            {
                LightTrigger.Trigger();
            }

            base.DrawLoop();
        }

        protected override void Action(MouseButton mouseButton)
        {
            if (@object.IsEnemy)
            {
                UseAbility();
            }
            else
            {
                if (!@object.NoInteract)
                {
                    playerSceneObject.StopMovings();
                    var sceneObj = Act();
                    ShowInScene?.Invoke(sceneObj.InList());
                }
                else
                {
                    Toast.Show(@object.NoInteractText, ShowInScene);
                }
            }
        }

        private void UseAbility()
        {
            if (ability == default)
                return;

            if (ability.TargetType == AbilityTargetType.Target || ability.TargetType == AbilityTargetType.TargetAndNonTarget)
            {
                var avatar = playerSceneObject.Avatar;
                ability.Target = this.@object.Entity;
                if (ability.CastAvailableCooldown(avatar))
                {
                    ability.CastCooldown(location, avatar);
                }
            }
        }

        protected override void BeforeClick()
        {
            if (NPC.Fraction != default && NPC.Fraction.Playable)
            {
                if (!Global.GameState.Character.Fractions.Any(x => x.IdentifyName == NPC.Fraction.IdentifyName))
                {
                    Global.GameState.Character.Fractions.Add(FractionView.Load(NPC.Fraction.IdentifyName).ToFraction());
                }
            }
        }

        private ISceneObject Act() => @object.Merchant == null
            ? (ISceneObject)new NPCDialogue(playerSceneObject, @object, this.DestroyBinding, this.ControlBinding, location, new MetallButtonControl("Выход"))
            : (ISceneObject)new ShopWindow(@object.Name, playerSceneObject, @object.Merchant, this.DestroyBinding, this.ControlBinding, location);

        protected override void StopAction()
        {
        }

        protected override Key[] KeyHandles => new Key[] { Key.LeftShift, AlternativeTooltipKey };

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            base.KeyDown(key, modifier, hold);
            if (@object.IsEnemy)
            {
                if (key == Key.Q || key == Key.E)
                {
                    UseAbility();
                }
            }
        }

        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
        }

        private bool attackAvailable = true;
        private TimerTrigger attackTimer;

        private bool isChasing = false;

        protected override bool OnLogic()
        {
            if (!this.@object.Entity.Aggressive)
                return true;

            if (moveDistance == 0)
            {
                isChasing = false;
            }

            if (isChasing)
                return false;

            List<MapObject> targets = new List<MapObject>();

            var query = location.MapObject.QueryContainer(this.mapObj.Vision);
            var areaCounts = query.Count;

            for (int i = 0; i < areaCounts; i++)
            {
                var e = query.ElementAtOrDefault(i);
                if (e != default)
                {
                    var nodes = e.Nodes;
                    var count = nodes.Count;
                    for (int j = 0; j < count; j++)
                    {
                        var node = nodes.ElementAtOrDefault(j);
                        if (node != default)
                        {
                            if (this.mapObj.Vision.IntersectsWith(node))
                            {
                                targets.Add(node);
                            }
                        }
                    }
                }
            }

            if (@object.IsEnemy)
            {
                targets = targets.Where(x => IsMapObjAvatar(x) || IsMapObjEnemyFractional(x)).ToList();
            }
            else
            {
                targets = targets.Where(IsMapObjEnemyFractional).ToList();
            }

            if (targets.Count() > 0)
            {
                var target = targets.First();
                if (target.IntersectsWith(@object.AttackRange))
                {
                    if (attackAvailable)
                    {
                        Attack(target);
                        attackAvailable = false;
                        attackTimer.Trigger();
                        return false;
                    }
                }

                if (NPC.ChasingPlayers && target.Is<Avatar>())
                {
                    move = Direction.Idle;
                    moveDistance = 0;
                    Chasing(target as Avatar);

                    return false;
                }
            }
            else if (isChasing)
            {
                isChasing = false;
                moveDistance = 0;
            }

            return true;
        }

        private void Chasing(MapObject target)
        {
            isChasing = true;

            var playerPos = target.Position;
            var thisPos = @object.Position;

            Direction dirX = Direction.Idle;
            Direction dirY = Direction.Idle;

            if (playerPos.X <= thisPos.X)
            {
                dirX = Direction.Left;
            }
            if (playerPos.X >= thisPos.X)
            {
                dirX = Direction.Right;
            }

            if (playerPos.Y >= thisPos.Y)
            {
                dirY = Direction.Down;
            }

            if (playerPos.Y <= thisPos.Y)
            {
                dirY = Direction.Up;
            }

            move = (Direction)((int)dirX + (int)dirY);

            moveDistance = 60;
        }

        private static bool IsMapObjAvatar(MapObject x) => typeof(Avatar).IsAssignableFrom(x.GetType());

        private bool IsMapObjEnemyFractional(MapObject x) => typeof(NPCMap).IsAssignableFrom(x.GetType()) && x!=@object && NPC.IsEnemy(x.BindedEntity);

        private void Attack(MapObject mapTarget)
        {
            var target = mapTarget.BindedEntity.As<Interactable>();

            var value = (long)RandomDungeon.Next(NPC.MinDMG*NPC.Level, NPC.MaxDMG*NPC.Level);

            target.Damage(NPC, new Damage()
            {
                Amount = value,
                Type = DamageType.Physical
            });
        }

        public override void Focus()
        {
            if (@object.IsEnemy)
            {
                playerSceneObject.TargetsInFocus.Add(@object);
            }
            base.Focus();
        }

        public override void Unfocus()
        {
            if (@object.IsEnemy)
            {
                playerSceneObject.TargetsInFocus.Remove(@object);
            }
            base.Unfocus();
        }

        protected override bool CheckActionAvailable(MouseButton mouseButton)
        {
            if(@object.IsEnemy==false)
            {
                return playerSceneObject.Avatar.IntersectsWith(FakeGrow(3));
            }

            if (mouseButton != MouseButton.None)
            {
                var range = ability.Range;
                return @object.IntersectsWith(range);
            }

            //вот тут ещё отмена Changell навыка будет

            return false;
        }

        private PhysicalObject FakeGrow(double by)
        {
            var rangeObject = new PhysicalObject()
            {
                Position = new Dungeon.Physics.PhysicalPosition
                {
                    X = @object.Position.X - ((24 * by) - 24) / 2,
                    Y = @object.Position.Y - ((24 * by) - 24) / 2
                },
                Size = new PhysicalSize()
                {
                    Height = 24,
                    Width = 24
                }
            };

            rangeObject.Size.Height *= by;
            rangeObject.Size.Width *= by;

            return rangeObject;
        }

        protected override bool CheckMoveAvailable(Direction direction)
        {
            bool inMoveRegion;
            if (@object.IsEnemy)
            {
                inMoveRegion = !location.InSafe(this.mapObj);
            }
            else
            {
                inMoveRegion = (moveable?.MoveRegion.IntersectsWith(this.mapObj) ?? false);
            }

            if (inMoveRegion && this.location.Move(this.mapObj, direction))
            {
                this.Left = this.mapObj.Location.X;
                this.Top = this.mapObj.Location.Y;
                return true;
            }
            else
            {
                lastClosedDirection = direction;
                this.mapObj.Location.X = this.Left;
                this.mapObj.Location.Y = this.Top;
                return false;
            }
        }
    }
}