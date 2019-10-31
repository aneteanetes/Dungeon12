namespace Rogue.Classes.Inquisitor
{
    using System;
    using Rogue.Entites.Alive.Character;

    public class Inquisitor : Player
    {
        public override string ClassName { get => "Инквизитор"; }

        public override string Resource => this.Lithanies.ToString();

        public override string ResourceName => "Литании";

        public override ConsoleColor ResourceColor => ConsoleColor.Cyan;

        public override ConsoleColor ClassColor => ConsoleColor.Cyan;

        public int Lithanies { get; set; }
    }
}