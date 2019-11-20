using Dungeon.Map;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities.Quests
{
    public interface IQuest : IDrawable, IVisual
    {
        long Progress { get; set; }

        long MaxProgress { get; set; }

        string Description { get; set; }

        Reward Reward { get; set; }

        Dungeon12Class Character { get; }

        QuestProgress QuestProgress { get; }

        void Bind(Dungeon12Class @class, GameMap gameMap);

        void Complete();
    }
}