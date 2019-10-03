
namespace Rogue.Drawing.SceneObjects
{
    using Rogue.Drawing.SceneObjects.UI;

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
