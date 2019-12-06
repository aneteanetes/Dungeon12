using Dungeon.Drawing.SceneObjects.Map;
using Dungeon.Map;
using Dungeon.View.Interfaces;

namespace Dungeon.Conversations
{
    public interface IConversationTrigger : ITrigger<IDrawText,PlayerSceneObject,GameMap,string[],Replica>
    {
        bool Storable { get; }
    }
}
