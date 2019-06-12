namespace Rogue.Map.Objects
{
    using Rogue.Conversations;
    using Rogue.Map.Infrastructure;
    using System.Collections.Generic;

    [Template("H")]
    public class Home : Сonversational
    {
        public override string Icon { get => "N"; set { } }

        protected override MapObject Self => this;
        
        public override bool Obstruction => true;
    }
}