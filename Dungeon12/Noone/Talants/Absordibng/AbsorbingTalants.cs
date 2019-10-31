using Dungeon.Abilities.Talants;
using Dungeon.Classes.Noone.Abilities;
using Dungeon.Classes.Noone.Talants.Absordibng;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dungeon12.Classes.Noone.Talants
{
    public class AbsorbingTalants : TalantTree<Noone>
    {
        public override string Name => "Поглощение";

        public override string Tileset => "";

        public Absorbing Absorbing { get; set; } = new Absorbing(0) { Level=1 };

        public AbsorbedPoison Poison { get; set; } = new AbsorbedPoison(1) { Level = 2 };

        public AbsorbingMetall Metall { get; set; } = new AbsorbingMetall(1) { Level = 1, Active=true };

        public AbsorbingElements Elements { get; set; } = new AbsorbingElements(3);

        public AbsorbingStone Stone { get; set; } = new AbsorbingStone(2);

        public AbsorbingMagic Magic { get; set; } = new AbsorbingMagic(4);
    }

    public class AbsorbingStone : Talant<Noone>
    {
        public override string Group => Absorbing.GroupName;

        public override bool Activatable => true;

        public AbsorbingStone(int order) : base(order)
        {
        }

        public override string Description => $"Поглощение металла позволяет увеличивать физическую при активации.";

        public override string Name => "Поглощение земли";

        public override string[] DependsOn => new string[]
        {
            nameof(AbsorbingMetall)
        };

        public override int Tier => 2;

        protected override void CallApply(dynamic obj)
        {
            return;
        }

        protected override bool CallCanUse(dynamic obj)
        {
            return true;
        }

        protected override void CallDiscard(dynamic obj)
        {
            return;
        }

        protected override TalantInfo CallTalantInfo(dynamic obj)
        {
            throw new System.NotImplementedException();
        }
    }

    public class AbsorbingMagic : Talant<Noone>
    {
        public override string Group => Absorbing.GroupName;
        public override bool Activatable => true;

        public AbsorbingMagic(int order) : base(order)
        {
        }
        public override string Description => $"Поглощение металла позволяет увеличивать защиту от магии при активации.";

        public override string Name => "Поглощение магии";

        public override string[] DependsOn => new string[]
        {
            nameof(Absorbing)
        };

        public override int Tier => 3;

        protected override void CallApply(dynamic obj)
        {
            return;
        }

        protected override bool CallCanUse(dynamic obj)
        {
            return true;
        }

        protected override void CallDiscard(dynamic obj)
        {
            return;
        }

        protected override TalantInfo CallTalantInfo(dynamic obj)
        {
            throw new System.NotImplementedException();
        }
    }

    public class AbsorbingElements : Talant<Noone>
    {
        public override string Group => Absorbing.GroupName;
        public override bool Activatable => true;
        public AbsorbingElements(int order) : base(order)
        {
        }

        public override string Name => "Поглощение элементов";

        public override string Description => $"Поглощение металла позволяет увеличивать защиту от элементов при активации.";

        public override string[] DependsOn => new string[]
        {
            nameof(AbsorbedPoison)
        };

        public override int Tier => 2;

        protected override void CallApply(dynamic obj)
        {
            return;
        }

        protected override bool CallCanUse(dynamic obj)
        {
            return true;
        }

        protected override void CallDiscard(dynamic obj)
        {
            return;
        }

        protected override TalantInfo CallTalantInfo(dynamic obj)
        {
            throw new System.NotImplementedException();
        }
    }

    public class AbsorbingMetall : Talant<Noone>
    {
        public override string Group => Absorbing.GroupName;
        public override bool Activatable => true;
        public AbsorbingMetall(int order) : base(order)
        {
        }

        public override string Name => "Поглощение металла";

        public override string Description => $"Поглощение металла позволяет увеличивать{Environment.NewLine} физическую защиту при активации.";

        public override string[] DependsOn => new string[]
        {
            nameof(Absorbing)
        };

        public override int Tier => 1;
        protected override void CallApply(dynamic obj)
        {
            return;
        }

        protected override bool CallCanUse(dynamic obj)
        {
            return true;
        }

        protected override void CallDiscard(dynamic obj)
        {
            return;
        }

        protected override TalantInfo CallTalantInfo(dynamic obj)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Absorbing : Talant<Noone>
    {
        public const string GroupName = "Активные поглощения";

        public Absorbing(int order) : base(order)
        {
        }

        public TalantInfo TalantInfo(ShieldSkill shieldSkill)
        {
            return new TalantInfo()
            {
                Name = "Базовое поглощение",
                Description = $"При активации увеличивает маг. защиту{Environment.NewLine} на Ур*1 ед защиты на Ур*1.5 секунд."
            };
        }

        public override string Name => "Поглощение";

        public override string Description => $"Позволяет поглощать предметы лута из врагов{Environment.NewLine} для того что бы открывать и улучшать другие{Environment.NewLine} таланты поглощения.";

        protected override void CallApply(dynamic obj)
        {
            return;
        }

        protected override bool CallCanUse(dynamic obj)
        {
            return true;
        }

        protected override void CallDiscard(dynamic obj)
        {
            return;
        }

        protected override TalantInfo CallTalantInfo(dynamic obj)
        {
            return this.TalantInfo(obj);
        }
    }
}
