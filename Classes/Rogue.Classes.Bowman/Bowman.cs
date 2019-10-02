using Rogue.Abilities;
using Rogue.Abilities.Talants.TalantTrees;
using Rogue.Classes.Bowman.Abilities;
using System;

namespace Rogue.Classes.Bowman
{
    public class Bowman : BaseCharacterTileset
    {
        public override string Tileset => "Rogue.Classes.Bowman.Images.sprite.png";

        public SpeedShot SpeedShot { get; set; } = new SpeedShot();

        public MightShot MightShot { get; set; } = new MightShot();

        public RainOfArrows RainOfArrows { get; set; } = new RainOfArrows();

        public JumpOff JumpOff { get; set; } = new JumpOff();

        public override T[] PropertiesOfType<T>()
        {
            switch (typeof(T))
            {
                case Type t when t.IsAssignableFrom(typeof(Ability)):
                    return new T[]
                    {
                        SpeedShot as T,
                        MightShot as T,
                        RainOfArrows as T,
                        JumpOff as T
                    };
                case Type t when t.IsAssignableFrom(typeof(TalantTree)):
                    return new T[] { };
                default: return default;
            }
        }
    }
}