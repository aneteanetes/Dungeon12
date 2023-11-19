using Dungeon;
using Dungeon.Control;
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

        protected override ControlEventType[] Handles => new ControlEventType[]
        {
            ControlEventType.Click,
            ControlEventType.GlobalClickRelease,
            ControlEventType.GlobalMouseMove
        };

        private bool hold = false;

        public override void Click(PointerArgs args)
        {
            if (args.MouseButton == Dungeon.Control.Pointer.MouseButton.Right)
                hold = true;

            base.Click(args);
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