using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.Entities;

namespace Dungeon12.SceneObjects.Base
{
    internal class ItemHint : EmptySceneObject, ISceneObjectHosted
    {
        public ISceneObject Host { get; set; }

        public ItemHint(Item item)
        {
            this.Width = 355;


        }
    }
}