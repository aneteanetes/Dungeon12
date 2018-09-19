namespace Rogue.Classes.Inquisitor
{
    using Rogue.Entites.Alive.Character;

    public class Inquisitor : Player
    {
        public new string Name { get => "Инквизитор"; set { } }

        public int Lithanies { get; set; }
    }
}