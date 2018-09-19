namespace Rogue.Classes.FireMage
{
    using Rogue.Entites.Alive.Character;

    public class FireMage : Player
    {
        public new string Name { get => "Маг огня"; set { } }

        public long Mana { get; set; }
    }
}