namespace Rogue.Races.Perks
{
    using System;
    using Rogue.Perks;
    using Rogue.View.Interfaces;

    public class RacePerk : Perk
    {
        public override string Icon => throw new NotImplementedException();

        public override string Name => throw new NotImplementedException();

        public override IDrawColor BackgroundColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IDrawColor ForegroundColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override string Description => throw new NotImplementedException();

        protected override void CallApply(dynamic obj)
        {
            throw new NotImplementedException();
        }

        protected override void CallDiscard(dynamic obj)
        {
            throw new NotImplementedException();
        }
    }
}