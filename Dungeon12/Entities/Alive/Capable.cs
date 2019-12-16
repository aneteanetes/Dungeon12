namespace Dungeon12.Entities.Alive
{
    /// <summary>
    /// Может использовать модификаторы атаки
    /// </summary>
    public class Capable : Attackable
    {
        public long AttackDamage { get; set; }

        public long AbilityPower { get; set; }
    }
}