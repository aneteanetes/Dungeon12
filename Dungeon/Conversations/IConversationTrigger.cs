using Dungeon.Drawing.SceneObjects.Map;
using Dungeon.Map;
using Dungeon.View.Interfaces;
using System.Collections.Generic;

namespace Dungeon.Conversations
{
    public interface IConversationTrigger
    {
        PlayerSceneObject PlayerSceneObject { get; set; }

        GameMap Gamemap { get; set; }

        IDrawText Execute(string[] args);
    }
}
