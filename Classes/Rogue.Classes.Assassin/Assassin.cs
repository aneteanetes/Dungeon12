namespace Rogue.Classes.Assassin
{
    using Rogue.Entites.Alive.Character;

    public class Assassin : Player
    {
        public override string ClassName { get => "Убийца"; }

        public long Poisons { get; set; }
    }
}