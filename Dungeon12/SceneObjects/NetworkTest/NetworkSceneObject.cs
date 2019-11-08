using Dungeon;
using Dungeon.Drawing;
using Dungeon.Entities.Alive;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;

namespace Dungeon12.Drawing.SceneObjects.NetworkTest
{
    public class NetworkSceneObject : EmptyHandleSceneControl
    {
        private NetworkObject _alive;

        public NetworkSceneObject(NetworkObject alive, bool controls)
        {
            _alive = alive;

            this.Width = 10;
            this.Height = 10;

            if (controls)
            {
                this.AddChild(new SmallMetallButtonControl(new DrawText("+") { Size = 40 }.Montserrat())
                {
                    Left = 10 + 6,
                    OnClick = () =>
                    {
                        _alive.HitPoints++;
                    }
                });

                this.AddChild(new SmallMetallButtonControl(new DrawText("-") { Size = 40 }.Montserrat())
                {
                    Left = 10 + 11,
                    OnClick = () =>
                    {
                        _alive.HitPoints--;
                    }
                });
            }
        }

        public override IDrawText Text
        {
            get => new DrawText($"{_alive.Name} [HP] : {_alive.HitPoints}/{_alive.MaxHitPoints}");
        }
    }

    public class NetworkObject : Alive
    {
        public override string ProxyId => Name;
    }
}