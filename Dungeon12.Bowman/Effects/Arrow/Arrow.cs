using Dungeon.Control.Pointer;
using Dungeon.Drawing.Impl;
using Dungeon.Physics;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon12.Drawing.SceneObjects;
using Dungeon12.Entities.Alive;
using Dungeon12.Map;
using Dungeon12.Map.Objects;
using System;
using System.Collections.Generic;

namespace Dungeon12.Bowman.Effects
{
    public class Arrow : AnimatedSceneObject<ArrowObject>
    {
        private GameMap _gameMap;

        private Point destination;
        private MapObject destinationArea;
        private MapObject thisArea;
        private Bowman bowman;

        public Arrow(Bowman bowman, GameMap gameMap, ArrowObject arrow, Direction dir, Point from, bool effect = false) : base(null, arrow, "", new Rectangle()
        {
            Height = 32,
            Width = 32,
            X = 0,
            Y = dir == Direction.Down ? 0
                : (dir == Direction.Left ? 32
                    : (dir == Direction.Right ? 64
                        : dir == Direction.Up ? 96 : 0))
        })
        {
            this.bowman = bowman;
            arrow.Size = new PhysicalSize() { Height = 16, Width = 16 };
            _gameMap = gameMap;
            destination = Global.PointerLocation.GameCoordinates;

            destinationArea = new MapObject()
            {
                Location = new Point(destination),
                Size = new PhysicalSize() { Height = 32, Width = 32 }
            };

            this.Width = 1;
            this.Height = 1;

            this.Left = from.X;
            this.Top = from.Y;

            thisArea = new MapObject()
            {
                Location = new Point(this.Left,this.Top),
                Size = arrow.Size.Copy()
            };

            this.Image = @object.Image;
            SetAnimation(@object.Animation);
            SetAngle(dir, from);

            CalculatePath();

            if (effect)
            {
                this.AddChild(new Might(dir == Direction.Left || dir == Direction.Right, dir == Direction.Left, dir == Direction.Right));
            }
        }



        private class Might : EmptySceneObject
        {
            public Might(bool offsetTop, bool offsetLeft,bool offsetRight)
            {
                this.Width = 1;
                this.Height = 1;

                if (offsetTop)
                {
                    this.Top -= 0.25;
                }

                if (offsetLeft)
                {
                    this.Left -= 0.25;
                }

                if (offsetRight)
                {
                    this.Left += 0.25;
                }

                this.Effects = new List<Dungeon.View.Interfaces.IEffect>()
                {
                    new ParticleEffect()
                    {
                        Name="PowerArrow",
                        Scale = 1
                    }
                };
            }
        }

        /// <summary>
        /// Сложный метод сделанный на ощупь, хуйзнает почему, но работает как надо
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="from"></param>
        private void SetAngle(Direction dir, Point from)
        {
            this.Angle = from.Angle(Global.PointerLocation.GameCoordinates);

            if (dir == Direction.Left)
            {
                this.Angle -= 180 * Math.PI / 180;
                this.Angle *= -1;
            }

            if (dir == Direction.Right)
            {
                this.Angle *= -1;
            }

            if (dir == Direction.Up)
            {
                this.Angle -= 90 * Math.PI / 180;
                this.Angle *= -1;
            }

            if (dir == Direction.Down)
            {
                this.Angle += 90 * Math.PI / 180;
                this.Angle *= -1;
            }
        }

        private Queue<Point> Trajectory = new Queue<Point>();

        private void CalculatePath()
        {
            var xDiff = destination.X - this.Left;
            var yDiff = destination.Y - this.Top;

            VectorDir xVector = xDiff < 0 ? VectorDir.Minus : VectorDir.Plus;
            VectorDir yVector = yDiff < 0 ? VectorDir.Minus : VectorDir.Plus;

            xDiff = Math.Abs(xDiff);
            yDiff = Math.Abs(yDiff);

            if (xDiff > @object.Range)
            {
                xDiff = @object.Range;
            }
            if (yDiff > @object.Range)
            {
                yDiff = @object.Range;
            }

            var xSteps = xDiff / @object.Speed;
            var ySteps = yDiff / @object.Speed;

            var countPaths = xSteps > ySteps ? xSteps : ySteps;
            var distance = xSteps > ySteps ? xDiff : yDiff;

            var xStepSpeed = @object.Speed;
            var yStepSpeed = @object.Speed;

            if (xSteps > ySteps)
            {
                var moreDiff = ySteps / xSteps; //больше в N раз
                yStepSpeed *= moreDiff;
            }

            if (ySteps > xSteps)
            {
                var moreDiff = xSteps / ySteps; //больше в N разz
                xStepSpeed *= moreDiff;
            }

            for (double i = 0; i < distance; i += @object.Speed)
            {
                Trajectory.Enqueue(new Point(xStepSpeed, yStepSpeed)
                {
                    VectorX = xVector,
                    VectorY = yVector
                });
            }
        }

        public override bool Clickable => false;

        protected override void Action(MouseButton mouseButton)
        {
        }

        public override Rectangle ImageRegion => base.ImageRegion;
        
        protected override void AnimationLoop()
        {
            var rangeObject = new MapObject
            {
                Position = new Dungeon.Physics.PhysicalPosition
                {
                    X = (this.Left * 32) + 10,
                    Y = (this.Top * 32) + 10
                },
                Size = new PhysicalSize()
                {
                    Height = 16,
                    Width = 16
                }
            };

            var target = _gameMap.One<NPCMap>(rangeObject,n=>n.IsEnemy);
            if (target != default)
            {
                target.Entity.Damage(this.bowman,new Damage()
                {
                    Amount=@object.Damage,
                    Type=DamageType.Kenetic                    
                });
                RequestStop();
            }

            var obstruct = _gameMap.One<Obstruct>(rangeObject);
            if(obstruct!=default)
            {
                RequestStop();
            }
        }

        protected override void OnAnimationStop()
        {
            this.Destroy?.Invoke();
        }

        protected override void DrawLoop()
        {
            if (Trajectory.Count == 0)
            {
                RequestStop();
                return;
            }

            if (destinationArea.IntersectsWithOrContains(thisArea))
            {
                Trajectory.Clear();
                RequestStop();
                return;
            }

            var step = Trajectory.Dequeue();

            if (step.VectorY == VectorDir.Plus)
            {
                this.Top += step.Y;
                thisArea.Location.Y += step.Y;
            }
            else
            {
                this.Top -= step.Y;
                thisArea.Location.Y -= step.Y;
            }

            if (step.VectorX == VectorDir.Plus)
            {
                this.Left += step.X;
                thisArea.Location.X += step.X;
            }
            else
            {
                this.Left -= step.X;
                thisArea.Location.X -= step.X;
            }
        }
    }
}
