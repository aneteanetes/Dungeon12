namespace Rogue.Classes.BloodMage
{
    using Rogue.Entites.Alive.Character;

    public class BloodMage : Player
    {
        public new string Name { get => "Маг крови"; set { } }

        public long Blood { get; set; }
    }
}