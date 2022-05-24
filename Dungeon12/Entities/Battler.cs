namespace Dungeon12.Entities
{
    internal class Battler
    {
        public MaxValue Hp { get; set; } = new MaxValue(50, 50);

        public int Level { get; private set; } = 1;

        public int Exp { get; private set; } = 0;

        public ExpTable ExpTable { get; set; } = new ExpTable();

        public MaxValue Damage { get; set; } = new MaxValue(5, 10);

        /// <summary>
        /// Attack POwer
        /// </summary>
        public Value AD { get; set; } = new Value();

        /// <summary>
        /// Ability Power
        /// </summary>
        public Value AP { get; set; } = new Value();

        public Value Speed { get; set; } = new Value(1);

        public Value Initiative { get; set; } = new Value(1);

        /// <summary>
        /// Шанс попадения
        /// </summary>
        public Value HitChance { get; set; } = new Value(95);

        public Value CritChance { get; set; } = new Value(5);

        public Value Armor { get; set; } = new Value(0);

        public int ArmorClass { get; set; } = 0;

        /// <summary>
        /// Шанс отражения магии
        /// </summary>
        public Value MagicReflectChance { get; set; } = new Value(10);

        public Value FireProtection { get; set; } = new Value();

        public Value FrostProtection { get; set; } = new Value();

        public Value MagicProtection { get; set; } = new Value();

        public Value LightProtection { get; set; } = new Value();

        public Value DarkProtection { get; set; } = new Value();

        /// <summary>
        /// Шанс уклонения
        /// </summary>
        public Value DodgeChance { get; set; } = new Value(5);

        /// <summary>
        /// Шанс физического блока
        /// </summary>
        public Value BlockChance { get; set; } = new Value(8);

        /// <summary>
        /// Проницание магии
        /// </summary>
        public Value MagicPenetration { get; set; } = new Value(5);

        /// <summary>
        /// Шанс сокрушительного удара
        /// </summary>
        public Value CrushingBlowChance { get; set; } = new Value(5);

        /// <summary>
        /// Шанс скользящего удара
        /// </summary>
        public Value GlancingBlowChance { get; set; } = new Value(10);

        /// <summary>
        /// Мультиприкатор скользящего удара
        /// </summary>
        public Value GlancingBlowMultiplier { get; set; } = new Value(.5);

        /// <summary>
        /// Шанс Паррирования
        /// </summary>
        public Value ParryChance { get; set; } = new Value(5);

        public Value EffectPhysicalChance { get; set; } = new Value(100);

        public Value EffectMagicChance { get; set; } = new Value(100);

        public Value EffectPhysicalDamage { get; set; } = new Value(100);

        public Value EffectMagicDamage { get; set; } = new Value(100);
    }
}