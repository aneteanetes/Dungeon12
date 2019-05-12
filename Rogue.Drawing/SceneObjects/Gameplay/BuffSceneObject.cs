namespace Rogue.Drawing.SceneObjects.Gameplay
{
    using Rogue.Transactions;

    public class BuffSceneObject : ImageControl
    {
        public override bool CacheAvailable => false;

        public BuffSceneObject(Applicable appl)
            : base(appl.Image)
        { }
    }
}