using Dungeon.SceneObjects;
using Dungeon12.Entities;

namespace Dungeon12.SceneObjects.MUD
{
    internal class ChatPanel : SceneObject<GameLog>
    {
        public ChatPanel(GameLog component) : base(component)
        {
            this.Width=1120;
            this.Height=200;

            this.AddBorder();
        }
    }
}
