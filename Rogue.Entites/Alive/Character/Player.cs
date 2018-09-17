using Rogue.Entites.Enums;

namespace Rogue.Entites.Alive.Character
{
    /// <summary>
    /// Абстрактный класс персонажа
    /// </summary>
    public class Player : Modified
    {
        public Race Race { get; set; }

        public long EXP { get; set; }

        public int Gold { get; set; }
    }
}