using Dungeon.SceneObjects;
using Dungeon12.Entities;

namespace Dungeon12.SceneObjects.Map
{
    public class PartySceneObject : SceneControl<Party>
    {
        public PartySceneObject(Party component) : base(component, true)
        {

        }
    }
}
