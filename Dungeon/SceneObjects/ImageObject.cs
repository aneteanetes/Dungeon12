namespace Dungeon.Drawing.SceneObjects
{
    using Dungeon.SceneObjects;

    /// <summary>
    /// По умолчанию не реагирует на события
    /// </summary>
    public class ImageObject : EmptySceneObject
    {
        public override bool Events => false;

        public ImageObject(string imagePath)
        {
            Image = imagePath;
        }

        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
        }
    }
}
