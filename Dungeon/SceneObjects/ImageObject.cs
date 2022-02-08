namespace Dungeon.Drawing.SceneObjects
{
    using Dungeon.SceneObjects;
    using System.Reflection;

    /// <summary>
    /// По умолчанию не реагирует на события
    /// </summary>
    public class ImageObject : EmptySceneObject
    {
        public override bool Events => false;

        public ImageObject(string imagePath)
        {
            if (!imagePath.Contains(".Resources.Images."))
            {
                imagePath = Assembly.GetCallingAssembly().GetName().Name + ".Resources.Images." + imagePath.Embedded();
            }

            Image = imagePath;
        }

        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
        }
    }
}
