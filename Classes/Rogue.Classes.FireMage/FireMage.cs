namespace Rogue.Classes.FireMage
{
    using Rogue.Entites.Alive.Character;

    public class FireMage : Player
    {
        public override string ClassName { get => "Маг огня"; }

        public long Mana { get; set; }
    }
}