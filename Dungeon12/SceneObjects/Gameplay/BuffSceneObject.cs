namespace Dungeon12.Drawing.SceneObjects.Gameplay
{
    using Dungeon.Transactions;

    public class BuffSceneObject : ImageControl
    {
        public override bool CacheAvailable => false;

        public BuffSceneObject(Applicable appl)
            : base(appl.Image)
        { }
    }
}