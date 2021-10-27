using Dungeon12.Classes;
using Dungeon.Data;
using Dungeon12.Map;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Dungeon12.SceneObjects.UI;

namespace Dungeon12.Entities.Quests
{
    public interface IQuest : IDrawable, IVisual, IPersist
    {
        long Progress { get; set; }

        long MaxProgress { get; set; }

        string Description { get; set; }

        Reward Reward { get; set; }

        Character Character { get; }

        void Bind(Character @class, GameMap gameMap);

        bool IsCompleted();

        void Complete();

        /// <summary>
        /// Флаг отслеживания задания
        /// </summary>
        bool Discover { get; set; }

        QuestDescoverSceneObject VisualDescover(bool questBook = true);
    }
}