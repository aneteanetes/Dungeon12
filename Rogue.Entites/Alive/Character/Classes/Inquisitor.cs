using System;
using Rogue.Entites.Abilities;
using Rogue.Entites.Enums;

namespace Rogue.Entites.Alive.Character.Classes
{
    public class Inquisitor : Player
    {
        public Inquisitor(Race race) : base(race)
        {
        }

        public override Class Class => Class.Inquisitor;

        public override Ability Q => throw new NotImplementedException();

        public override Ability W => throw new NotImplementedException();

        public override Ability E => throw new NotImplementedException();

        public override Ability R => throw new NotImplementedException();
    }
}
