using Dungeon.Drawing.SceneObjects.Map;
using Dungeon.Map;
using Dungeon.Map.Objects;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12.Drawing.SceneObjects.Effects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.SceneObjects
{
    public class Player : PlayerSceneObject
    {
        public Player(Avatar player, GameMap location, Action<ISceneObject> destroyBinding) : base(player, location, destroyBinding)
        {
            Dungeon.Global.Time
                .After(8)
                .Do(() => RemoveTorchlight())
                .Auto();
        }

        private TorchlightInHandsSceneObject torchlight;
        private bool torch = false;

        public void Torchlight()
        {
            if (!torch && (Dungeon.Global.Time.Hours > 17 || Dungeon.Global.Time.Hours < 8))
            {
                AddTorchlight();
            }
            else
            {
                RemoveTorchlight();
            }

            torch = !torch;
        }

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

        protected override void OnMoveRegistered(Direction dir)
        {
            switch (dir)
            {
                case Direction.Idle:
                    break;
                case Direction.Up:
                    if (torchlight != null)
                    {
                        torchlight.Left = 0;
                    }
                    break;
                case Direction.Down:
                    if (torchlight != null)
                    {
                        torchlight.Left = 0;
                    }
                    break;
                case Direction.Left:
                    if (torchlight != null)
                    {
                        torchlight.Left = 0.4;
                    }
                    break;
                case Direction.Right:
                    if (torchlight != null)
                    {
                        torchlight.Left = 0.2;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
