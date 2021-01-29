namespace Dungeon.Monogame
{
    using Microsoft.Xna.Framework;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using Rec = Dungeon.Types.Rectangle;

    public partial class XNADrawClient : Game, IDrawClient
    {
        private readonly HashSet<Direction> CameraMovings = new HashSet<Direction>();

        private readonly HashSet<Direction> OnceCameraMovings = new HashSet<Direction>();

        public void MoveCamera(Direction direction, bool stop = false, bool once = false)
        {
            if (once)
            {
                OnceCameraMovings.Add(direction);
                return;
            }

            if (!stop)
            {
                CameraMovings.Add(direction);
            }
            else
            {
                CameraMovings.Remove(direction);
            }
        }

        public void ResetCamera()
        {
            this.CameraMovings.Clear();
            this.CameraOffsetX = 0;
            this.CameraOffsetY = 0;
        }

        public void SetCamera(double x, double y)
        {
            this.CameraOffsetX = x;
            this.CameraOffsetY = y;
        }

        public void SetCameraSpeed(double speed) => cameraSpeed = speed;

        private double cameraSpeed = 2.5;

        public double CameraOffsetX { get; set; }

        public double CameraOffsetY { get; set; }

        public double CameraOffsetZ { get; set; }

        public double CameraOffsetLimitX { get; set; } = 3200000;

        public double CameraOffsetLimitY { get; set; } = 3200000;

        public double CameraOffsetLimitZ { get; set; } = 3200000;

        private bool IsStop(double number, double limit) => Math.Abs(number) >= limit;

        private void CalculateCamera()
        {
            if (IsStop(CameraOffsetX, CameraOffsetLimitX))
            {
                var direction = CameraOffsetX < 0
                    ? Direction.Right
                    : Direction.Left;
                CameraMovings.Remove(direction);
                OnceCameraMovings.Remove(direction);
            }

            if (IsStop(CameraOffsetY, CameraOffsetLimitY))
            {
                var direction = CameraOffsetY < 0
                    ? Direction.Down
                    : Direction.Up;
                CameraMovings.Remove(direction);
                OnceCameraMovings.Remove(direction);
            }

            if (CameraMovings.Contains(Direction.Right))
            {
                CameraOffsetX -= cameraSpeed;
            }
            if (CameraMovings.Contains(Direction.Down))
            {
                CameraOffsetY -= cameraSpeed;
            }
            if (CameraMovings.Contains(Direction.Left))
            {
                CameraOffsetX += cameraSpeed;
            }
            if (CameraMovings.Contains(Direction.Up))
            {
                CameraOffsetY += cameraSpeed;
            }

            if (OnceCameraMovings.Contains(Direction.Right))
            {
                CameraOffsetX -= cameraSpeed;
                OnceCameraMovings.Remove(Direction.Right);
            }
            if (OnceCameraMovings.Contains(Direction.Down))
            {
                CameraOffsetY -= cameraSpeed;
                OnceCameraMovings.Remove(Direction.Down);
            }
            if (OnceCameraMovings.Contains(Direction.Left))
            {
                CameraOffsetX += cameraSpeed;
                OnceCameraMovings.Remove(Direction.Left);
            }
            if (OnceCameraMovings.Contains(Direction.Up))
            {
                CameraOffsetY += cameraSpeed;
                OnceCameraMovings.Remove(Direction.Up);
            }
        }

        public void StopMoveCamera()
        {
            this.CameraMovings.Clear();
        }

        private readonly Rec _cameraViewObject = new Rec();
        public Rec CameraView
        {
            get
            {
                _cameraViewObject.X = CameraOffsetX * -1;
                _cameraViewObject.Y = CameraOffsetY * -1;
                _cameraViewObject.Width = DungeonGlobal.Resolution.Width;
                _cameraViewObject.Height = DungeonGlobal.Resolution.Height;

                return _cameraViewObject;
            }
        }


        public bool InCamera(ISceneObject sceneObject)
        {
            var pos = sceneObject.ComputedPosition;
            if (sceneObject.AbsolutePosition)
            {
                var byX = pos.X >= 0 && pos.X <= DungeonGlobal.Resolution.Width;
                var byY = pos.Y >= 0 && pos.Y <= DungeonGlobal.Resolution.Height;
                return byX && byY;
            }

            var w = sceneObject.ComputedPosition.Width == default ? 0.1 : sceneObject.ComputedPosition.Width;
            var h = sceneObject.ComputedPosition.Height == default ? 0.1 : sceneObject.ComputedPosition.Height;

            var cameraIn = IntersectsWith_WithoutAllocation(
                CameraOffsetX * -1, CameraOffsetY * -1, DungeonGlobal.Resolution.Width, DungeonGlobal.Resolution.Height,
                sceneObject.ComputedPosition.X, sceneObject.ComputedPosition.Y, w, h);

            var objIn = IntersectsWith_WithoutAllocation(
                sceneObject.ComputedPosition.X, sceneObject.ComputedPosition.Y, w, h,
                CameraOffsetX, CameraOffsetY, DungeonGlobal.Resolution.Width, DungeonGlobal.Resolution.Height);

            return cameraIn || objIn;
        }

        public bool InCamera(ITile tile)=> IntersectsWith_WithoutAllocation(
                tile.Left, tile.Top, tile.Width, tile.Height,
                CameraOffsetX*-1, CameraOffsetY*-1, DungeonGlobal.Resolution.Width, DungeonGlobal.Resolution.Height);

        private static bool IntersectsWith_WithoutAllocation(
            double x1, double y1, double w1, double h1,
            double x2, double y2, double w2, double h2)
        {
            var xsum1 = Math.Max(x1, x2);
            var xsum2 = Math.Min(x1 + w1, x2 + w2);
            var ysum1 = Math.Max(y1, y2);
            var ysum2 = Math.Min(y1 + h1, y2 + h2);

            if (xsum2 >= xsum1 && ysum2 >= ysum1)
            {
                return true;
            }

            return false;
        }
    }
}