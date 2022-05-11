using Dungeon;
using Dungeon.Localization;

namespace Dungeon12.Localization
{
    public class GameStrings : LocalizationStringDictionary<GameStrings>
    {
        public string ByProperty(string nameof)
        {
            return this.GetPropertyExpr<string>(nameof);
        }

        public string FreePoints { get; set; }

        public string IgnoreTargetPhysicalDefence { get; set; } = "Игнорирует физическую защиту цели";

        public string Damage { get; set; } = "Урон";

        public string Heal { get; set; } = "Исцеление";

        public string Incoming { get; set; } = "Входящий";

        public string Active { get; set; } = "Действует";

        public string Turn { get; set; } = "Ход";

        public string Turns { get; set; } = "Ходов";

        public string Attack { get; set; } = "Атака";

        public string Defence { get; set; } = "Защита";

        public string Barrier { get; set; } = "Барьер";

        public string Type { get; set; } = "Тип";

        public string Range { get; set; } = "Дальность";

        public string StunChanse { get; set; } = "Шанс оглушить";

        public string Health { get; set; } = "Здоровье";

        public string HealEffective { get; set; } = "Эффективность исцеления";

        public string Sacrifice { get; set; } = "Умирает вместо призвавшего";

        public string Taunt { get; set; } = "Провоцирует";

        public string NotAffectSummoned { get; set; } = "Не действует на призванных существ";

        public string HideAttack { get; set; } = "Скрытая";

        public string EachHandAttack { get; set; } = "Атака оружием в каждой руке";

        public string RightHandAttack { get; set; } = "Атака оружием в правой руке";

        public string LeftHandAttack { get; set; } = "Атака оружием в левой руке";

        public string RangeClose { get; set; } = "Ближний бой";

        public string RangeFar { get; set; } = "Дальний бой";

        public string RangeAny { get; set; } = "Любой";

        public string RangeFriendly { get; set; } = "Дружественная цель";

        public string Summon { get; set; } = "Призыв";

        public string RangeWeaponDepends { get; set; } = "Зависит от оружия";

        public string Healing { get; set; } = "Исцеление";

        public string ElemenentalUnitHis { get; set; } = "Элементаля";

        public string Area { get; set; } = "Область";

        public string Cooldown { get; set; } = "Восстановление";

        public string EnterCharacterName { get; set; } = "Введите имя персонажа";

        public string Abilities { get; set; } = "Способности";

        public string Apples { get; set; } = "Яблоки";

        public string Skills { get; set; } = "Навыки";

        public string LeftMouseButton { get; set; } = "ЛКМ";

        public string Info { get; set; } = "Информация";

        public string Warrior { get; set; } = "Воин";
        public string Mage { get; set; } = "Маг";
        public string Thief { get; set; } = "Вор";
        public string Priest { get; set; } = "Священник";

        public string Next { get; set; } = "Далее";

        public string Prev { get; set; } = "Назад";

        public string Cancel { get; set; } = "Отмена";

        public string CreateParty { get; set; } = "Создание отряда";

        public string NewGame { get; set; } = "Новая игра";

        public string Save { get; set; } = "Сохранить";

        public string Load { get; set; } = "Загрузить";

        public string Settings { get; set; } = "Настройки";

        public string Credits { get; set; } = "Авторы";

        public string FastGame { get; set; } = "ККИ";

        public string ExitGame { get; set; } = "Выйти";

        public override string ___RelativeLocalizationFilesPath => "locale";

        public override string ___DefaultLanguageCode => "ru";

        //fractions
        public string Friendly{ get; set; } = "Арбитры";
        public string Neutral{ get; set; } = "Нейтралитет";
        public string Vanguard{ get; set; } = "Авангард";
        public string MageGuild{ get; set; } = "Гильдия магов";
        public string Mercenary{ get; set; } = "Наёмники";
        public string Exarch{ get; set; } = "Экзархат";
        public string Cult{ get; set; } = "Культ проклятых";


        //skills

        public string Landscape { get; set; } = "Ландшафт";
        public string Eating { get; set; } = "Расход еды";
        public string Repair { get; set; } = "Ремонт";
        public string Weaponcraft { get; set; } = "Оружейничество";
        public string Portals { get; set; } = "Порталы";
        public string Attension { get; set; } = "Внимание";
        public string Spiritism { get; set; } = "Спиритизм";
        public string Alchemy { get; set; } = "Алхимия";
        public string Traps { get; set; } = "Ловушки";
        public string Lockpicking { get; set; } = "Взлом";
        public string Stealing { get; set; } = "Воровство";
        public string Leatherwork { get; set; } = "Кожевничество";
        public string Prayers { get; set; } = "Молитвы";
        public string FoodStoring { get; set; } = "Хранение еды";
        public string Trade { get; set; } = "Торговля";
        public string Tailoring { get; set; } = "Шитьё";


        public string LandscapeDesc { get; set; } = "Благодаря грубой физической силе войны могут изменять ландшафт подземелий - прорубать проходы в зарослях и копать траншеи. Для того что бы изменить ландшафт подземелья потребуется 1 ед. провизии. Каждое очко навыка позволяет изменять ландшафт всё более сложного типа, вплоть до разрушения минералов. При уничтожении ландшафта есть шанс получить ресурсы.";
        public string EatingDesc { get; set; } = "Военная подготовка позволяет войнам более качественно распределять провизию, что позволяет восстанавливать больше здоровья за одну ед. провизии. Каждое очко навыка увеличивает восстановления здоровья на 2.5%. Навык расхода еды не суммируется и всегда выбирается наиболее высокий.";
        public string RepairDesc { get; set; } = "Навык ремонта позволяет чинить предметы прямо во время странствий. Каждое очко навыка уменьшает расход прочности предметов на 1 ед. Кроме этого, при переходе который тратит провизию на привал есть шанс 2.5% за 1 ед. навыка починить один предмет полностью.";
        public string WeaponcraftDesc { get; set; } = "Оружейничество позволяет создавать оружие. Войны могут создавать любые типы оружия из дерева, камня, и металла. Для создания тяжелых доспехов потребуется использовать кузню которая может быть для этого оборудована.";
        public string PortalsDesc { get; set; } = "Маги способны создавать порталы для перемещения на большие дистанции и между городами регионов. Для создания портала магу потребуется побывать у городского портала. Каждая 1 ед. навыка позволяет преодолеть расстояние в 4 клетки. Для перемещения между регионами используется формула: каждые 4 ед. навыка позволяют телепортироваться через 1 регион.";
        public string AttensionDesc { get; set; } = "Внимание позволяет находить скрытые двери, дополнительные предметы для заданий, замечать ловушки на дверях, переходах и сундуках. Каждая 1 ед. навыка сравнивается с уровнем внимания нужным для проверки навыка. Кроме этого, внимание позволяет заранее знать кого будет атаковать враг, а так же определять его хар-ки по формуле: Внимание>Ур.цели+1";
        public string SpiritismDesc { get; set; } = "Спиритизм это навык общения с мёртвыми. Общение с мёртвыми может помочь выяснить детали задания при проверке Ур.навыка>Ур.духа. Так же спиритизм потребуется для обучения некромантии - чем выше уровень спиритизма тем более сильных существ может призвать маг.";
        public string AlchemyDesc { get; set; } = "Алхимия позволяет создавать зелья увеличивающие характеристики и дающие дополнительные эффекты как в атаке, так и в защите. Алхимики могут создавать катализаторы для увеличения силы зелий. 1 ед. навыка увеличивает силу катализаторов на 5%, а после 10 уровня позволяет создавать легенадрные зелья.";
        public string TrapsDesc { get; set; } = "Воры могут изготавливать и разоружать ловушки. Изготовление ловушек доступно из всех материалов, а использовать их можно заранее на поле боя. Так же, навык ловушек позволяет ворам обезоруживать вражеские ловушки, для разоружения уровень навыка должен быть больше либо равен уровню установленной ловушки. Изготавливаемые ловушки равны по уровню навыку.";
        public string LockpickingDesc { get; set; } = "Взлом позволяет взламывать замки на сундуках и дверях. Для успешного взлома замков требуется уровень навыка больше либо равный уровню замка. При уровне навыка больше уровне замка на 10 вы получаете дополнительные ресурсы для изготовления ловушек. При уровне навыка меньше уровня замка на 10 замок блокируется и его больше невозможно открыть.";
        public string StealingDesc { get; set; } = "Навык воровства позволяет получить дополнительные предметы для заданий, а так же шанс обворовать цель атаки. Каждая 1ед. навыка даёт 5% шанс украсть один предмет у каждого противника. При успешной краже у одной из целей, следующий процент кражи уменьшается в половину. (Если первая успешная кража была с 15%, то следующий шанс равен 7.5%)";
        public string LeatherworkDesc { get; set; } = "Кожевничество позволяет находить и собирать шкуры с убитых врагов, а после этого использовать для изготовления кожаных доспехов.";
        public string PrayersDesc { get; set; } = "При нападении на вас даёт шанс получить благословление всему отряду. При нападении на врага даёт шанс навлечь кару на весь отряд противника. 1ед. навыка даёт 5% шанс навлечь кару или получить благословление. За каждые 5ед. навыка кол-во благословлений или кары увеличивается на 1. (15ед. навыка позволяет получить/навлечь 3 кары/благословления)";
        public string FoodStoringDesc { get; set; } = "Долгие годы служения в храмах и аббатах открыли священникам тайны хранения еды. 1 ед. навыка позволяет увеличить кол-во запасов еды которые можно хранить в походе на 1. За каждые 2ед. навыка можно хранить новый тип еды, каждый новый тип еды позволяет восстанавливать больше очков здоровья на привале.";
        public string TradeDesc { get; set; } = "Торговля позволяет использовать торговые гильдии в поселениях, с помощью которых можно налаживать торговые поставки между регионами. Каждая успешная цепочка поставки даёт пассивный доход раз в сутки. От уровня навыка зависит возможность дальности цепочек поставки, а при максимальном уровне можно перевозить товары через море.";
        public string TailoringDesc { get; set; } = "Навык портняжного дела позволяет создавать тканевую одежду которая может увеличивать все характеристики кроме физической защиты. Такая одежда может давать различные магические эффекты не занимая при этом ячейки брони.";

        //abils

        public string WarriorAttack { get; set; } = "Сильный удар";
        public string WarriorThrow { get; set; } = "Бросок оружия";
        public string WarriorStand { get; set; } = "Защитная стойка";
        public string WarriorWarcry { get; set; } = "Боевой клич";

        public string MageArrowAttack { get; set; } = "Магическая стрела";
        public string MageAoe { get; set; } = "Сфера огня";
        public string MageShield { get; set; } = "Каменная кожа";
        public string MageSummon { get; set; } = "Призыв элементаля";

        public string ThiefAttack { get; set; } = "Скрытая атака";
        public string ThiefShadow { get; set; } = "Удар тьмы";
        public string ThiefMark { get; set; } = "Метка смерти";
        public string ThiefStep { get; set; } = "Быстрый шаг";

        public string PriestAttack { get; set; } = "Ошеломляющий удар";
        public string PriestHeal { get; set; } = "Исцеление";
        public string PriestHolyNova { get; set; } = "Волна света";
        public string PriestAngel { get; set; } = "Помощь ангела";

        //abils descr default

        public string WarriorAttackDescT1 { get; set; } = "Наносит удар тупой стороной оружия по одной цели игнорируя физическую броню.";
        public string WarriorThrowDescT1 { get; set; } = "Бросает оружие в цель во втором ряду. В следующий ход враг будет атаковать война.";
        public string WarriorStandDescT1 { get; set; } = "Увеличивает физическую защиту уменьшая урон от способностей.";
        public string WarriorWarcryDescT1 { get; set; } = "Заставляет всех врагов атаковать война в следующий ход.";

        public string MageArrowAttackDescT1 { get; set; } = "Выпускает магическую стрелу способную нанести урон в любом радиусе.";
        public string MageAoeDescT1 { get; set; } = "Выпускают сферу огня наносящую урон в небольшом радиусе.";
        public string MageShieldDescT1 { get; set; } = "Увеличивает физическую и магическую защиту дружественной цели.";
        public string MageSummonDescT1 { get; set; } = "Призывает элементаля сражающегося на вашей стороне со случайной целью.";

        public string ThiefAttackDescT1 { get; set; } = "Скрытая атака выполняется после любой следующей атаки дружественного существа.";
        public string ThiefShadowDescT1 { get; set; } = "Атака наносящая урон от магии тьмы двум рядом стоящим целям.";
        public string ThiefMarkDescT1 { get; set; } = "Удар наносящий небольшой урон но оставляющий метку на цели увеличивающую повреждения.";
        public string ThiefStepDescT1 { get; set; } = "После активации позволяет совершить два действия подряд в этом ходу.";

        public string PriestAttackDescT1 { get; set; } = "Атака ближнего боя с шансом оглушить врага на один ход.";
        public string PriestHealDescT1 { get; set; } = "Исцеляет выбранного члена отряда. Не работает на призванных существ.";
        public string PriestHolyNovaDescT1 { get; set; } = "Волна света исцеляющая всех членов отряда.";
        public string PriestAngelDescT1 { get; set; } = "Призывает ангела который увеличивает исходящее исцеление и может пожертвовать собой ради жреца.";

        // standard names
        public string WarriorFemale { get; set; } = "Аехани";
        public string WarriorMale { get; set; } = "Мадаор";
        public string MageMale { get; set; } = "Ташин";
        public string MageFemale { get; set; } = "Фикора";
        public string ThiefMale { get; set; } = "Клиф";
        public string ThiefFemale { get; set; } = "Гиссель";
        public string PriestMale { get; set; } = "Дженс";
        public string PriestFemale { get; set; } = "Селуа";

        // elements

        public string ElementMental { get; set; } = "Ментальный";
        public string ElementPhysical { get; set; } = "Физический";
        public string ElementFire { get; set; } = "Огонь";
        public string ElementFrost { get; set; } = "Холод";
        public string ElementMagical { get; set; } = "Магия";
        public string ElementSpirit { get; set; } = "Дух";
        public string ElementHoly { get; set; } = "Свет";
        public string ElementDark { get; set; } = "Тьма";

        public string PartyInventory { get; set; } = "Инвентарь отряда";
    }
}