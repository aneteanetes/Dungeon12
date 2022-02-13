using Dungeon;
using Dungeon.SceneObjects;
using Dungeon12.Entities.Talks;

namespace Dungeon12.SceneObjects.Talk
{
    public class ReplySceneObject : SceneObject<Replica>
    {
        public ReplySceneObject(Replica component) : base(component)
        {
            this.Width = 961;
            this.Height = 283;
            this.Image = @"Talk/replicsback.png".AsmImg();
        }
    }
}