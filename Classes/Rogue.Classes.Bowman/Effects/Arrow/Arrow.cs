using Rogue.Control.Pointer;
using Rogue.Drawing.SceneObjects;
using Rogue.Drawing.SceneObjects.Map;
using Rogue.Map;
using Rogue.Map.Objects;
using Rogue.Physics;
using Rogue.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Classes.Bowman.Effects
{
    public class Arrow : AnimatedSceneObject<ArrowObject>
    {
        private double _fly;
        private GameMap _gameMap;

        public Arrow(GameMap gameMap, ArrowObject arrow, Direction dir,Point from) : base(null,arrow, "", new Rectangle()
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
            _gameMap = gameMap;

            this.Width = 1;
            this.Height = 1;

            this.Left = from.X;
            this.Top = from.Y;

            this.Image = @object.Image;
            SetAnimation(@object.Animation);

            this.Angle = 90*Math.PI/180;
            CalculatePath();
        }

        private Queue<Point> Trajectory = new Queue<Point>();

        private void CalculatePath()
        {
            var dest = Global.PointerLocation.GameCoordinates;
            var xDiff =dest.X - this.Left;
            var yDiff = dest.Y - this.Top;
            
            VectorDir xVector = xDiff < 0 ? VectorDir.Minus : VectorDir.Plus;
            VectorDir yVector = yDiff < 0 ? VectorDir.Minus : VectorDir.Plus;

            xDiff = Math.Abs(xDiff);
            yDiff = Math.Abs(yDiff);

            var xSteps = xDiff / @object.Speed;
            var ySteps = yDiff / @object.Speed;

            var countPaths = xSteps > ySteps ? xSteps : ySteps;

            var xStepSpeed = @object.Speed;
            var yStepSpeed = @object.Speed;

            if (xSteps > ySteps)
            {
                var moreDiff= ySteps / xSteps; //больше в N раз
                yStepSpeed *= moreDiff;
            }

            if (ySteps > xSteps)
            {
                var moreDiff = xSteps / ySteps; //больше в N разz
                xStepSpeed *= moreDiff;
            }

            countPaths /= 24;

            for (double i = 0; i < countPaths; i += @object.Speed)
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

        protected override void DrawLoop()
        {
            if (Trajectory.Count == 0)
            {
                RequestStop();
                return;
            }

            var step = Trajectory.Dequeue();

            if (step.VectorY == VectorDir.Plus)
            {
                this.Top += step.Y;
            }
            else
            {
                this.Top -= step.Y;
            }

            if (step.VectorX == VectorDir.Plus)
            {
                this.Left += step.X;
            }
            else
            {
                this.Left -= step.X;
            }
        }

        protected override void AnimationLoop()
        {
            var rangeObject = new MapObject
            {
                Position = new Physics.PhysicalPosition
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

            var target = _gameMap.One<Mob>(rangeObject);
            if (target != default)
            {
                target.Flow(t => t.Damage(true), new { @object.Damage });
                RequestStop();
            }
        }

        protected override void OnAnimationStop()
        {
            this.Destroy?.Invoke();
        }
    }
}
