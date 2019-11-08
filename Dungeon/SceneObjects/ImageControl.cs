
namespace Dungeon.Drawing.SceneObjects
{
    using Dungeon.Drawing.SceneObjects.UI;
    using Dungeon.SceneObjects;

    public class ImageControl : EmptySceneObject
    {
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
