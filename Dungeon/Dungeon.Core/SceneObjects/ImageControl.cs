
namespace Dungeon.Drawing.SceneObjects
{
    using Dungeon.Drawing.SceneObjects.UI;

    public class ImageControl : SceneObject
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
