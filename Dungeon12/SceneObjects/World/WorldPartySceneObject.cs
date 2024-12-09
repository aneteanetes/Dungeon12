using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon12.Entities;

namespace Dungeon12.SceneObjects.World
{
    internal class WorldPartySceneObject : SceneObject<Party>
    {
        private WorldHeroSceneObject h1,h2,h3,h4;

        internal WorldPartySceneObject(Party component, bool bindView = true) : base(component, bindView)
        {
            Width = WorldSettings.cellSize;
            Height = WorldSettings.cellSize;

            h1 = this.AddChild(new WorldHeroSceneObject(component.Second)); // mage
            h1.SetSlot(Compass.North);

            //component.Hero1.IsSelected = true;
            h2 = this.AddChild(new WorldHeroSceneObject(component.First)); // warrior
            h2.SetSlot(Compass.West);

            h3 =this.AddChild(new WorldHeroSceneObject(component.Fourth)); // priest
            h3.SetSlot(Compass.East);

            h4 = this.AddChild(new WorldHeroSceneObject(component.Third)); // thief
            h4.SetSlot(Compass.South);
        }

        public void ChangeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Idle:
                    break;
                case Direction.Up:
                    break;
                case Direction.Down:
                    break;
                case Direction.Left:
                    break;
                case Direction.Right:
                    break;
                default:
                    break;
            }
        }
    }
}