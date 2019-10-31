namespace Rogue.Classes.Assassin
{
    using System;
    using Rogue.Entites.Alive.Character;

    public class Assassin : Player
    {
        public override string ClassName { get => "Убийца"; }

        public override string Resource => this.Poisons.ToString();

        public override string ResourceName => "Яды";

        public override ConsoleColor ResourceColor => ConsoleColor.Green;

        public override ConsoleColor ClassColor => ConsoleColor.Green;

        public long Poisons { get; set; }
    }
}