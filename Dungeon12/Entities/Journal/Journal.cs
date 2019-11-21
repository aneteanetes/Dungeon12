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
        //{
        //    new JournalEntry()
        //    {
        //        Display="Аномальная погода",
        //        Group="Остров веры",
        //        Text=
        //        "Собор послал вас узнать почему на Острове Веры такая аномальная погода. Возможно, это связанно с недавними событиями на забытом маяке, а так же и с налётами диких крысолюдей на поселение."
        //         +Environment.NewLine+
        //        "Послушники советуют обратиться к местному мэру, что бы узнать детали событий которые произошли прошлой ночью и двумя неделями ранее."
        //    },
        //    new JournalEntry()
        //    {
        //        Display="Поиск символа веры",
        //        Group="Остров веры",
        //        Text="example"
        //    },
        //    new JournalEntry()
        //    {
        //        Display="Старый дом",
        //        Group="Остров веры",
        //        Text="example"
        //    }
        //};

        [Value("Детали")]
        [Icon("details")]
        public List<JournalEntry> Details { get; set; } = new List<JournalEntry>()
        {
            new JournalEntry()
            {
                Display="Магические жидкости",
                Group="Объекты",
                Text="example"
            },
            new JournalEntry()
            {
                Display="Алтари",
                Group="Объекты",
                Text="example"
            },
            new JournalEntry()
            {
                Display="Пещеры",
                Group="Регионы",
                Text="example"
            },
            new JournalEntry()
            {
                Display="Маяки",
                Group="Регионы",
                Text="example"
            }
        };

        [Value("Мир")]
        [Icon("world")]
        public List<JournalEntry> World { get; set; } = new List<JournalEntry>();

        [Value("Выполнено")]
        [Icon("qdone")]
        public List<JournalEntry> QuestsDone { get; set; } = new List<JournalEntry>()
        {
            new JournalEntry()
            {
                Display="Таланты",
                Group="Обучение",
                Text="example"
            }
        };

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
                Icon = $"Journal/{prop.ValueAttribute<IconAttribute>().ToString()}.png".AsmImgRes(),
                Content = _Type[this, name] as List<JournalEntry>
            };
        }
    }
}