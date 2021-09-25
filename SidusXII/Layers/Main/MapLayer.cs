using Dungeon;
using Dungeon.Control;
using Dungeon.Control.Gamepad;
using Dungeon.Scenes;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using SidusXII.SceneObjects.Main.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SidusXII.Layers.Main
{
    public class MapLayer : SceneLayer
    {
        public MapLayer(Scene parentScene) : base(parentScene)
        {
            Width = 1600;
            Height = 710;
        }

        public override double Width => 1600;

        public override double Height => 710;

        bool movePossible = false;

        public override void OnMousePress(PointerArgs pointerPressedEventArgs, Point offset)
        {
            movePossible = true;
            base.OnMousePress(pointerPressedEventArgs, offset);
        }

        public override void OnMouseRelease(PointerArgs pointerPressedEventArgs, Point offset)
        {
            base.OnMouseRelease(pointerPressedEventArgs, offset);
            movePossible = false;
            prev = null;
        }

        private ISceneObject MapContainer => this.Objects.FirstOrDefault(x => x is MapObject);

        Point prev;

        public override void OnMouseMove(PointerArgs pointerPressedEventArgs, Point offset)
        {
            base.OnMouseMove(pointerPressedEventArgs, offset);

            if (movePossible)
            {
                var now = pointerPressedEventArgs.AsPoint;
                if (prev == null)
                {
                    prev = now;
                    return;
                }

                var dir = prev.DetectDirection(now, 1);
                MoveMap(dir);

                prev = now;
            }
        }

        private void MoveMap(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    MapContainer.Top-=Global.Settings.ControlSettings.MapScrollSpeed;
                    break;
                case Direction.Down:
                    MapContainer.Top += Global.Settings.ControlSettings.MapScrollSpeed;
                    break;
                case Direction.Left:
                    MapContainer.Left -= Global.Settings.ControlSettings.MapScrollSpeed;
                    break;
                case Direction.Right:
                    MapContainer.Left += Global.Settings.ControlSettings.MapScrollSpeed;
                    break;
                case Direction.UpLeft:
                    MapContainer.Top -= Global.Settings.ControlSettings.MapScrollSpeed;
                    MapContainer.Left -= Global.Settings.ControlSettings.MapScrollSpeed;
                    break;
                case Direction.UpRight:
                    MapContainer.Top -= Global.Settings.ControlSettings.MapScrollSpeed;
                    MapContainer.Left += Global.Settings.ControlSettings.MapScrollSpeed;
                    break;
                case Direction.DownLeft:
                    MapContainer.Top += Global.Settings.ControlSettings.MapScrollSpeed;
                    MapContainer.Left -= Global.Settings.ControlSettings.MapScrollSpeed;
                    break;
                case Direction.DownRight:
                    MapContainer.Top += Global.Settings.ControlSettings.MapScrollSpeed;
                    MapContainer.Left += Global.Settings.ControlSettings.MapScrollSpeed;
                    break;
                default:
                    break;
            }
        }

        public override void OnStickMove(Direction direction, GamePadStick stick)
        {
            if (stick == GamePadStick.LeftStick)
                MoveMap(direction.OppositeX());

            base.OnStickMove(direction, stick);
        }
    }
}