namespace Dungeon.Drawing.SceneObjects
{
    using Dungeon.SceneObjects;

    /// <summary>
    /// По умолчанию не реагирует на события
    /// </summary>
    public class ImageControl : EmptySceneObject
    {
        public override bool Events => false;

        public ImageControl(string imagePath)
        {
            Image = imagePath;
        }

        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
        }
    }
}
