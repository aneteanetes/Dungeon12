using Rogue.View.Interfaces;
using System.Collections.Generic;

namespace Rogue.Conversations
{
    public interface IConversationTrigger
    {
        object PlayerSceneObject { get; set; }

        object Gamemap { get; set; }

        IDrawText Execute(string[] args);
    }
}
