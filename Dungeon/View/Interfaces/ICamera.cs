namespace Dungeon.View.Interfaces
{
    using Dungeon.Types;

    public interface ICamera
    {
        void MoveCamera(Direction direction, bool stop = false, bool once = false);

        void StopMoveCamera();

        void SetCamera(double x, double y);

        void ResetCamera();

        double CameraOffsetX { get; }

        double CameraOffsetY { get; }

        /// <summary>
        /// 0 - invisible
        /// 0.5 - half
        /// 2 - double size
        /// </summary>
        double CameraOffsetZ { get; }

        double CameraOffsetLimitX { get; }

        double CameraOffsetLimitY { get; }

        double CameraOffsetLimitZ { get; }

        void SetCameraSpeed(double speed);

        bool InCamera(ISceneObject sceneObject);
    }
}
