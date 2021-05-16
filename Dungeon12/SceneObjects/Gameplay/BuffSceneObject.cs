namespace Dungeon12.Drawing.SceneObjects.Gameplay
{
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.Transactions;

    public class BuffSceneObject : Dungeon.Drawing.SceneObjects.ImageObject
    {
        public override bool CacheAvailable => false;

        public BuffSceneObject(Applicable appl)
            : base(appl.Image)
        { }
    }
}