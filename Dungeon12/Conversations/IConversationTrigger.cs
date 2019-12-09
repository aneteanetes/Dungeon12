using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.Map;
using Dungeon.View.Interfaces;
using Dungeon;

namespace Dungeon12.Conversations
{
    public interface IConversationTrigger : ITrigger<IDrawText,PlayerSceneObject,GameMap,string[],Replica>
    {
        bool Storable { get; }
    }
}
