using System;
using System.Collections.Generic;

namespace Dungeon12.Entities.Quests
{
    internal class QuestBook
    {
        public List<Quest> Quests { get; set; } = new List<Quest>();

        public Action<Quest> Add { get; set; }

        public Action<Quest> Done { get; set; }

        public QuestBook()
        {
            Quests.Add(new Quest()
            {
                Name = "Дипломатическая миссия",
                Goals = new List<Goal>()
                {
                    new Goal(){ Text = "Переместиться на корабль"},
                    new Goal(){ Text = "Забрать вещи из сундука"},
                    new Goal(){ Text = "Поговорить с сопровождающим"},
                    new Goal(){ Text = "Выйти на дорогу к посёлку"},
                }
            });
        }
    }
}
