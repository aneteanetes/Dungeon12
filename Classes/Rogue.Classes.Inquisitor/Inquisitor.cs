namespace Rogue.Classes.Inquisitor
{
    using Rogue.Entites.Alive.Character;

    public class Inquisitor : Player
    {
        public override string ClassName { get => "Инквизитор"; }

        public int Lithanies { get; set; }
    }
}