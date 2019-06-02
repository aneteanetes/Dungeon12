namespace Rogue
{
    using Microsoft.Xna.Framework;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;

    public partial class XNADrawClient : Game, IDrawClient
    {
        private readonly HashSet<Direction> CameraMovings = new HashSet<Direction>();
        public void MoveCamera(Direction direction, bool stop = false)
        {
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

        public double CameraOffsetLimitX { get; set; } = 3200000;

        public double CameraOffsetLimitY { get; set; } = 3200000;       
               
        private bool IsStop(double number, double limit) => Math.Abs(number) >= limit;

        private void CalculateCamera()
        {
            if (IsStop(CameraOffsetX, CameraOffsetLimitX))
            {
                var direction = CameraOffsetX < 0
                    ? Direction.Right
                    : Direction.Left;
                CameraMovings.Remove(direction);
            }

            if (IsStop(CameraOffsetY, CameraOffsetLimitY))
            {
                var direction = CameraOffsetY < 0
                    ? Direction.Down
                    : Direction.Up;
                CameraMovings.Remove(direction);
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
        }
    }
}