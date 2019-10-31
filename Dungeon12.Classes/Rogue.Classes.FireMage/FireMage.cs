namespace Rogue.Classes.FireMage
{
    using System;
    using Rogue.Entites.Alive.Character;

    public class FireMage : Player
    {
        public override string ClassName { get => "Маг огня"; }

        public override ConsoleColor ClassColor => ConsoleColor.DarkRed;

        public long Mana { get; set; }
    }
}