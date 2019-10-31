using Dungeon.View.Interfaces;
using System.Collections.Generic;

namespace Dungeon.Conversations
{
    public interface IConversationTrigger
    {
        object PlayerSceneObject { get; set; }

        object Gamemap { get; set; }

        IDrawText Execute(string[] args);
    }
}
