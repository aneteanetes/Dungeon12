using Dungeon;
using Dungeon.SceneObjects;
using System.Reflection;

namespace Dungeon12
{
    internal class ImageControl : EmptySceneControl
    {
        public ImageControl(string imagePath = null)
        {
            if (imagePath == null)
                return;

            if (!imagePath.Contains(".Resources.Images."))
            {
                imagePath = Assembly.GetCallingAssembly().GetName().Name + ".Resources.Images." + imagePath.Embedded();
            }

            Image = imagePath;
        }

        public ImageControl(Func<string> imagePath)
        {
            imageDelegate = imagePath;
        }

        private Func<string> imageDelegate;

        public override string Image
        {
            get
            {
                if (imageDelegate != null)
                    return imageDelegate();
                return base.Image;
            }
        }
    }
}
