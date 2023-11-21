using Dungeon;
using Dungeon.Control;
using Dungeon.Control.Pointer;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Types;
using Dungeon.Varying;

namespace Dungeon12.SceneObjects.Map
{
    internal class WorldSceneObject : SceneControl<Entities.Map.World>
    {
        WorldTerrainSceneObject terrain;

        public WorldSceneObject(Entities.Map.World component) : base(component)
        {
            Width = Global.Resolution.Width;
            Height = Global.Resolution.Height;

            terrain = this.AddChild(new WorldTerrainSceneObject(component));
        }

        public void SetCoords(int x, int y)
        {
            terrain.Move(x, y);
        }

        protected override ControlEventType[] Handles => new ControlEventType[]
        {
            ControlEventType.Click,
            ControlEventType.GlobalClickRelease,
            ControlEventType.GlobalMouseMove,
            ControlEventType.MouseWheel
        };

        private bool hold = false;

        public override void Click(PointerArgs args)
        {
            if (args.MouseButton == Dungeon.Control.Pointer.MouseButton.Right)
                hold = true;

            base.Click(args);
        }

        public override void MouseWheel(MouseWheelEnum mouseWheelEnum)
        {
            if(mouseWheelEnum == MouseWheelEnum.Down)
            {
                if (terrain.Scale > -1)
                    terrain.Scale -= 0.1;
            }
            if (mouseWheelEnum == MouseWheelEnum.Up)
            {
                if (terrain.Scale < 1)
                    terrain.Scale += 0.1;
            }

            base.MouseWheel(mouseWheelEnum);
        }

        Dot prev = new();

        public override void GlobalMouseMove(PointerArgs args)
        {
            if (hold)
            {
                var now = args.AsDot();
                if (prev == default)
                {
                    prev = now;
                    return;
                }

                if (prev != now)
                {
                    var dir = prev.DetectDirection(now, Variables.Get<double>("MapDetectDirectionAccuracy",1));
                    terrain.Move(dir, Variables.Get<double>("MapMoving",3));
                }

                prev = now;
            }
        }

        private HashSet<Direction> moving=new HashSet<Direction>();

        public override void Update(GameTimeLoop gameTime)
        {
            base.Update(gameTime);
        }

        public override void GlobalClickRelease(PointerArgs args)
        {
            if (args.MouseButton == Dungeon.Control.Pointer.MouseButton.Right)
                hold = false;

            base.GlobalClickRelease(args);
        }
    }
}