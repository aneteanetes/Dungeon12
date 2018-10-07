namespace Rogue.Classes.BloodMage
{
    using System;
    using Rogue.Entites.Alive.Character;
    using Rogue.Types;

    public class BloodMage : Player
    {
        public override string ClassName { get => "Маг крови"; }

        public override string Resource => this.Blood.ToString();

        public override string ResourceName => "Кровь";

        public override ConsoleColor ResourceColor => ConsoleColor.Red;

        public override ConsoleColor ClassColor => ConsoleColor.Red;

        public long Blood { get; set; }

        public override string Tileset => "Rogue.Classes.BloodMage.Images.Dolls.Character.png";

        public override Rectangle TileSetRegion => new Rectangle
        {
            X = 64,
            Y = 0,
            Height = 32,
            Width = 32
        };
    }
}