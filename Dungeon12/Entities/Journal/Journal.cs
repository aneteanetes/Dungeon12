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
        public List<JournalEntry> Quests { get; set; } = new List<JournalEntry>()
        {
            new JournalEntry()
            {
                Display="Аномальная погода",
                Group="Остров веры",
                Text=
                "Собор послал вас узнать почему на" + Environment.NewLine+
                "Острове Веры такая аномальная " +Environment.NewLine+
                "погода. " +Environment.NewLine+
                "" +Environment.NewLine+
                "Возможно, это связанно с недавними" +Environment.NewLine+
                "событиями на забытом маяке, а" +Environment.NewLine+
                "так же и с налётами диких " +Environment.NewLine+
                "крысолюдей на поселение." +Environment.NewLine+
                "" +Environment.NewLine+
                "Послушники советуют обратиться к" +Environment.NewLine+
                "местному мэру, что бы узнать детали" +Environment.NewLine+
                "событий которые произошли прошлой " +Environment.NewLine+
                "ночью и двумя неделями ранее."
            },
            new JournalEntry()
            {
                Display="Поиск символа веры",
                Group="Остров веры",
                Text="example"
            },
            new JournalEntry()
            {
                Display="Старый дом",
                Group="Остров веры",
                Text="example"
            },
            new JournalEntry()
            {
                Display="Классы",
                Group="Обучение",
                Text="example"
            },
            new JournalEntry()
            {
                Display="Повышение уровня",
                Group="Обучение",
                Text="example"
            },
            new JournalEntry()
            {
                Display="Игровое время",
                Group="Обучение",
                Text="example"
            },
            new JournalEntry()
            {
                Display="Погода",
                Group="Обучение",
                Text="example"
            },
            new JournalEntry()
            {
                Display="Зоны спокойствия",
                Group="Обучение",
                Text="example"
            },
            new JournalEntry()
            {
                Display="Торговцы",
                Group="Обучение",
                Text="example"
            },
            new JournalEntry()
            {
                Display="Переправа",
                Group="Материк",
                Text="example"
            },
            new JournalEntry()
            {
                Display="Известия",
                Group="Материк",
                Text="example"
            },
        };

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
        public List<JournalEntry> World { get; set; } = new List<JournalEntry>()
        {
            new JournalEntry()
            {
                Display="Учителя обороны",
                Group="Учителя",
                Text="example"
            },
            new JournalEntry()
            {
                Display="Учителя служителей",
                Group="Учителя",
                Text="example"
            },
            new JournalEntry()
            {
                Display="Остров веры",
                Group="Маршруты",
                Text="example"
            }
        };

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