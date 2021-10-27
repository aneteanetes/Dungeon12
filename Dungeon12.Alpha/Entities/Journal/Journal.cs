using Dungeon;
using Dungeon.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon12.Entites.Journal
{
    public class Journal : NetObject
    {
        [Value("Задания")]
        [Icon("quests")]
        public List<JournalEntry> Quests { get; set; } = new List<JournalEntry>();

        [Value("Детали")]
        [Icon("details")]
        public List<JournalEntry> Details { get; set; } = new List<JournalEntry>();

        [Value("Мир")]
        [Icon("world")]
        public List<JournalEntry> World { get; set; } = new List<JournalEntry>();

        [Value("Выполнено")]
        [Icon("qdone")]
        public List<JournalEntry> QuestsDone { get; set; } = new List<JournalEntry>();

        public Journal()
        {
            JournalCategories = new string[] { nameof(Quests), nameof(Details), nameof(World), nameof(QuestsDone) }.Select(ToCategory).ToList();
        }

        public List<JournalCategory> JournalCategories { get; }

        private JournalCategory ToCategory(string name)
        {
            var prop = _Type.GetMembers().FirstOrDefault(p => p.Name == name);

            return new JournalCategory()
            {
                Name = prop.ValueAttribute().ToString(),
                Icon = $"Journal/{prop.ValueAttribute<IconAttribute>().ToString()}.png".AsmImg(),
                Content = _Type[this, name] as List<JournalEntry>
            };
        }
    }
}