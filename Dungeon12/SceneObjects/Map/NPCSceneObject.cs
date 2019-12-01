namespace Dungeon12.Drawing.SceneObjects.Map
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
    using System.Linq;
    using Dungeon.Physics;
    using Dungeon.Drawing.SceneObjects.Map;
    using Dungeon12.Entities;
    using Dungeon12.Map.Objects;
    using Dungeon.Drawing.SceneObjects.UI;
    using Dungeon.Abilities.Enums;

    public class NPCSceneObject : MoveableSceneObject<NPCMap>
    {
        protected override bool SilentTooltip => true;

        private NPC NPC => @object.NPC;

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
            : base(playerSceneObject, mob, location, mob, mob.NPC, defaultFramePosition)
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

            if (mob.NPC.Idle != null)
            {
                this.SetAnimation(mob.NPC.Idle);
            }

            if (!mob.IsEnemy)
            {
                LightTrigger = Dungeon.Global.Time
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
                    MessageBox.Show(@object.NoInteractText, ShowInScene);
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

        private ISceneObject Act() => @object.Merchant == null
            ? (ISceneObject)new NPCDialogue(playerSceneObject, @object, this.DestroyBinding, this.ControlBinding,location, new MetallButtonControl("Выход"))
            : (ISceneObject)new ShopWindow(@object.Name, playerSceneObject, @object.Merchant, this.DestroyBinding, this.ControlBinding, location);

        protected override void StopAction()
        {
        }

        protected override Key[] KeyHandles => new Key[] { Key.LeftShift };

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
            if (!this.@object.NPC.Aggressive)
                return true;

            if (moveDistance == 0)
            {
                isChasing = false;
            }

            var targets = location.MapObject.Query(this.mapObj.Vision, true)
                .SelectMany(nodes => nodes.Nodes)
                .Where(node => this.mapObj.Vision.IntersectsWith(node));

            if (@object.IsEnemy)
            {
                targets = targets.Where(x => IsMapObjAvatar(x) || IsMapObjEnemyFractional(x));
            }
            else
            {
                targets = targets.Where(IsMapObjEnemyFractional);
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
                    }
                }

                if (NPC.ChasingPlayers && target.Is<Avatar>())
                {
                    moves.Clear();
                    moveDistance = 0;
                    Chasing(target as Avatar);
                }
            }

            return false;
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

            moves.Add((Direction)((int)dirX + (int)dirY));

            moveDistance = 60;
        }

        private static bool IsMapObjAvatar(MapObject x)  => typeof(Avatar).IsAssignableFrom(x.GetType());

        private bool IsMapObjEnemyFractional(MapObject x) => x.BindedEntity.IsEnemy(NPC);
        
        private void Attack(MapObject mapTarget)
        {
            var target = mapTarget.BindedEntity.As<Defensible>();

            var value = (long)RandomDungeon.Next(2, 7);

            var dmg = value - target.Defence;

            if (dmg < 0)
            {
                dmg = 0;
            }

            target.HitPoints -= dmg;

            if (target.HitPoints <= 0)
            {
                mapTarget.Die?.Invoke();
            }

            var critical = dmg > 6;

            this.ShowInScene(new List<ISceneObject>()
                {
                    new PopupString(dmg.ToString()+(critical ? "!" : ""), critical ? ConsoleColor.Red : ConsoleColor.White,mapTarget.Location,25,critical ? 14 : 12,0.06)
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
            if (@object.IsEnemy)
            {
                if (mouseButton != MouseButton.None)
                {
                    var range = ability.Range;
                    return @object.IntersectsWith(range);
                }

                //вот тут ещё отмена Changell навыка будет

                return false;
            }
            return true;
        }
    }
}