namespace Rogue.Classes.Paladin
{
    using Rogue.Entites.Alive.Character;

    public class Paladin : Player
    {
        public new string Name { get => "Паладин"; set { } }

        public long Mana { get; set; }
    }
}