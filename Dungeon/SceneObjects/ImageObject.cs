namespace Dungeon.Drawing.SceneObjects
{
    using Dungeon.SceneObjects;
    using System;
    using System.Reflection;

    public class ImageSceneObject : ImageObject { public ImageSceneObject(string imagePath = null) : base(imagePath) { } }

    /// <summary>
    /// По умолчанию не реагирует на события
    /// </summary>
    public class ImageObject : EmptySceneObject
    {
        public ImageObject(string imagePath=null)
        {
            if (imagePath == null)
                return;

            imagePath = MakeImagePath(imagePath);
            Image = imagePath;
        }

        public static string MakeImagePath(string imagePath)
        {
            if (imagePath.IsEmpty())
                throw new NullReferenceException("imagePath is empty!");

            if (!imagePath.Contains(".Resources."))
            {
                if (!imagePath.StartsWith("Images"))
                    imagePath = $"Images/{imagePath}";

                return Assembly.GetEntryAssembly().GetName().Name + ".Resources." + imagePath.Embedded();
            }

            return imagePath;
        }

        public ImageObject(Func<string> imagePath)
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

        public override void Throw(Exception ex)
        {
            Parent?.Throw(ex);
        }
    }
}
