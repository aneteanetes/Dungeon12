using Dungeon;
using Dungeon.SceneObjects;

namespace Dungeon12.SceneObjects.Base
{
    internal class Title : EmptySceneObject
    {
        public Title(string title, double width=830,double height=50)
        {
            this.Width=width;
            this.Height=height;

            this.AddTextCenter(title.Gabriela().InSize(26).InColor(Global.CommonColorLight));
        }
    }
}