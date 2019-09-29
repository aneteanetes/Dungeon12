using Rogue.Abilities.Talants;
using Rogue.Classes.Noone.Talants.Absordibng;
using System.ComponentModel.DataAnnotations;

namespace Rogue.Classes.Noone.Talants
{
    public class AbsorbingTalants : TalantTree<Noone>
    {
        public override string Name => "Поглощение";

        public override string Tileset => "";

        public Absorbing Absorbing { get; set; } = new Absorbing(0) { Level=1 };

        public AbsorbedPoison Poison { get; set; } = new AbsorbedPoison(1);

        public AbsorbingMetall Metall { get; set; } = new AbsorbingMetall(1) { Level = 1, Active=true };

        public AbsorbingElements Elements { get; set; } = new AbsorbingElements(3);

        public AbsorbingStone Stone { get; set; } = new AbsorbingStone(2) { Level = 2 };

        public AbsorbingMagic Magic { get; set; } = new AbsorbingMagic(4);
    }

    public class AbsorbingStone : Talant<Noone>
    {
        public override string Group => nameof(Absorbing);

        public override bool Activatable => true;

        public AbsorbingStone(int order) : base(order)
        {
        }

        public override string Name => "Поглощение земли";

        public override string[] DependsOn => new string[]
        {
            nameof(AbsorbingMetall)
        };

        public override int Tier => 2;

        protected override void CallApply(dynamic obj)
        {
            throw new System.NotImplementedException();
        }

        protected override bool CallCanUse(dynamic obj)
        {
            throw new System.NotImplementedException();
        }

        protected override void CallDiscard(dynamic obj)
        {
            throw new System.NotImplementedException();
        }

        protected override TalantInfo CallTalantInfo(dynamic obj)
        {
            throw new System.NotImplementedException();
        }
    }

    public class AbsorbingMagic : Talant<Noone>
    {
        public override string Group => nameof(Absorbing);
        public override bool Activatable => true;

        public AbsorbingMagic(int order) : base(order)
        {
        }

        public override string Name => "Поглощение магии";

        public override string[] DependsOn => new string[]
        {
            nameof(Absorbing)
        };

        public override int Tier => 3;

        protected override void CallApply(dynamic obj)
        {
            throw new System.NotImplementedException();
        }

        protected override bool CallCanUse(dynamic obj)
        {
            throw new System.NotImplementedException();
        }

        protected override void CallDiscard(dynamic obj)
        {
            throw new System.NotImplementedException();
        }

        protected override TalantInfo CallTalantInfo(dynamic obj)
        {
            throw new System.NotImplementedException();
        }
    }

    public class AbsorbingElements : Talant<Noone>
    {
        public override string Group => nameof(Absorbing);
        public override bool Activatable => true;
        public AbsorbingElements(int order) : base(order)
        {
        }

        public override string Name => "Поглощение элементов";

        public override string[] DependsOn => new string[]
        {
            nameof(AbsorbedPoison)
        };

        public override int Tier => 2;

        protected override void CallApply(dynamic obj)
        {
            throw new System.NotImplementedException();
        }

        protected override bool CallCanUse(dynamic obj)
        {
            throw new System.NotImplementedException();
        }

        protected override void CallDiscard(dynamic obj)
        {
            throw new System.NotImplementedException();
        }

        protected override TalantInfo CallTalantInfo(dynamic obj)
        {
            throw new System.NotImplementedException();
        }
    }

    public class AbsorbingMetall : Talant<Noone>
    {
        public override string Group => nameof(Absorbing);
        public override bool Activatable => true;
        public AbsorbingMetall(int order) : base(order)
        {
        }

        public override string Name => "Поглощение металла";

        public override string[] DependsOn => new string[]
        {
            nameof(Absorbing)
        };

        public override int Tier => 1;

        protected override void CallApply(dynamic obj)
        {
            throw new System.NotImplementedException();
        }

        protected override bool CallCanUse(dynamic obj)
        {
            throw new System.NotImplementedException();
        }

        protected override void CallDiscard(dynamic obj)
        {
            throw new System.NotImplementedException();
        }

        protected override TalantInfo CallTalantInfo(dynamic obj)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Absorbing : Talant<Noone>
    {
        public Absorbing(int order) : base(order)
        {
        }

        public override string Name => "Поглощение";

        protected override void CallApply(dynamic obj)
        {
            throw new System.NotImplementedException();
        }

        protected override bool CallCanUse(dynamic obj)
        {
            throw new System.NotImplementedException();
        }

        protected override void CallDiscard(dynamic obj)
        {
            throw new System.NotImplementedException();
        }

        protected override TalantInfo CallTalantInfo(dynamic obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
