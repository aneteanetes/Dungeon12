using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Nabunassar.ECS.Components;
using System.Reflection;

namespace Nabunassar
{
    internal class ImageControl : EmptySceneControl, ITooltiped
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        public ImageControl(string imagePath = null)
        {
            Image = ImageObject.MakeImagePath(imagePath);
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

        public string TooltipText { get; set; }
    }
}
