using Dungeon.Drawing.SceneObjects;
using Dungeon.Transactions;

namespace Dungeon12.SceneObjects
{

    public class BuffSceneObject : Dungeon.Drawing.SceneObjects.ImageControl
    {
        public override bool CacheAvailable => false;

        public BuffSceneObject(Applicable appl)
            : base(appl.Image)
        { }
    }
}