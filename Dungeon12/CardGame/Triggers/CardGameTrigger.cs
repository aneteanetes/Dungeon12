using Dungeon;
using Dungeon.Conversations;
using Dungeon.Drawing.SceneObjects.Map;
using Dungeon.Map;
using Dungeon.Scenes.Manager;
using Dungeon.View.Interfaces;
using Dungeon12.CardGame.Engine;
using Dungeon12.CardGame.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon12
{
    public class CardGameTrigger : IConversationTrigger
    {
        public PlayerSceneObject PlayerSceneObject { get; set; }

        public GameMap Gamemap { get; set; }

        public IDrawText Execute(string[] args)
        {
            Global.SceneManager.Change<CardGameScene>(args);
            return "Игра началась...".AsDrawText().Montserrat();
        }
    }
}
