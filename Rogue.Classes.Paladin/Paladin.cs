namespace Rogue.Classes.Paladin
{
    using System;
    using Rogue.Classes.Paladin.Perks;
    using Rogue.Entites.Alive.Character;

    public class Paladin : Player
    {
        public Paladin()
        {
            this.HitPoints = this.MaxHitPoints = 100;
            this.Level = 1;
            this.Mana = this.MaxMana = 100;
            this.MinDMG = 1;
            this.MaxDMG = 2;
            
            this.Add<PaladinPerk>();
        }

        public override string ClassName { get => "Паладин"; }

        public override ConsoleColor ClassColor => ConsoleColor.Yellow;

        public override string Resource => $"{Mana}/{MaxMana}";

        public long Mana { get; set; }

        public long MaxMana { get; set; }
    }
}