using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon12.ECS.Components;
using System.Reflection;

namespace Dungeon12
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
