using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Abilities.Talants.NotAPI
{
    public abstract class TalantBase : TalantDraw
    {
        public TalantBase(int order) => Order = order;

        public int Order { get; set; }

        public virtual int Tier { get; set; }

        public int Level { get; set; }

        public bool Available => Level > 0;

        public bool Opened => Level > 0;

        public bool Active { get; set; }

        /// <summary>
        /// Массив наименований других талантов в дереве от которого зависит этот
        /// </summary>
        public virtual string[] DependsOn { get; set; }

        public List<TalantBase> DependentTalants { get; } = new List<TalantBase>();
    }
}
