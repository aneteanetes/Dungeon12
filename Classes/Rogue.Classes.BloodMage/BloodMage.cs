namespace Rogue.Classes.BloodMage
{
    using System;
    using Rogue.Entites.Alive.Character;

    public class BloodMage : Player
    {
        public override string ClassName { get => "Маг крови"; }

        public override string Resource => this.Blood.ToString();

        public override string ResourceName => "Кровь";

        public override ConsoleColor ResourceColor => ConsoleColor.Red;

        public override ConsoleColor ClassColor => ConsoleColor.Red;

        public long Blood { get; set; }
    }
}