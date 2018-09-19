namespace Rogue.Classes.Assassin
{
    using Rogue.Entites.Alive.Character;

    public class Assassin : Player
    {
        public new string Name { get => "Убийца"; set { } }

        public long Poisons { get; set; }
    }
}