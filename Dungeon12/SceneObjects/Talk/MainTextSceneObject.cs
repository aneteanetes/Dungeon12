using Dungeon;
using Dungeon.SceneObjects;

namespace Dungeon12.SceneObjects.Talk
{
    public class MainTextSceneObject : EmptySceneObject
    {
        public MainTextSceneObject()
        {
            this.Width = 962;
            this.Height = 637;
            this.Image = @"Talk/text.png".AsmImg();
        }

        private class Replica : EmptySceneObject
        {

        }
    }
}