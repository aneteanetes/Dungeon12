namespace Dungeon.Entities.Alive
{
    /// <summary>
    /// Может использовать модификаторы атаки
    /// </summary>
    public class Capable : Attackable
    {
        public long AttackPower { get; set; }

        public long AbilityPower { get; set; }
    }
}