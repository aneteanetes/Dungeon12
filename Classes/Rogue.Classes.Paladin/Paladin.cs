namespace Rogue.Classes.Paladin
{
    using Rogue.Entites.Alive.Character;

    public class Paladin : Player
    {
        public override string ClassName { get => "Паладин"; }

        public long Mana { get; set; }
    }
}