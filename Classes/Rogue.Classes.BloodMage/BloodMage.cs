namespace Rogue.Classes.BloodMage
{
    using Rogue.Entites.Alive.Character;

    public class BloodMage : Player
    {
        public override string ClassName { get => "Маг крови"; }

        public long Blood { get; set; }
    }
}