using Dungeon.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon.Entites.Journal
{
    public class Journal : NetObject
    {
        [Value("Задания")]
        public List<JournalEntry> Quests { get; set; }

        [Value("Детали")]
        public List<JournalEntry> Details { get; set; }

        [Value("Мир")]
        public List<JournalEntry> World { get; set; }

        [Value("Выполнено")]
        public List<JournalEntry> QuestsDone { get; set; }

        public Journal()
        {
            JournalCategories = new string[] { nameof(Quests), nameof(Details), nameof(World), nameof(QuestsDone) }.Select(ToCategory).ToList();
        }

        public List<JournalCategory> JournalCategories { get; }

        private JournalCategory ToCategory(string name)
        {
            var prop = _Type.GetMembers().FirstOrDefault(p => p.Name == name);

            var valueAttr = (ValueAttribute)prop.GetAttribute(typeof(ValueAttribute),false);

            return new JournalCategory()
            {
                Name = valueAttr.Value.ToString(),
                Content = _Type[this, name] as List<JournalEntry>
            };
        }
    }
}