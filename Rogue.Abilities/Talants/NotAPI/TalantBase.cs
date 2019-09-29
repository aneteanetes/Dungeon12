using Rogue.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.Abilities.Talants.NotAPI
{
    public abstract class TalantBase : TalantDraw
    {
        public TalantBase(int order) => Order = order;

        public int Order { get; set; }

        public virtual int Tier { get; set; }

        public int Level { get; set; }

        public virtual int MaxLevel { get; set; } = 5;

        public bool Available => Opened && (Activatable ? Active : true);

        public bool Opened => Level > 0;

        private bool active = false;
        public bool Active
        {
            get => active; set
            {
                active = value;
                if (value)
                {
                    GroupActive?.Invoke(this);
                }
                ActiveChanged?.Invoke(value);
            }
        }

        public virtual string Group { get; set; }

        public Action<bool> ActiveChanged { get; set; }

        public Action<TalantBase> GroupActive { get; set; }

        public virtual bool Activatable { get; set; }

        /// <summary>
        /// Массив наименований других талантов в дереве от которого зависит этот
        /// </summary>
        public virtual string[] DependsOn { get; set; }

        public List<TalantBase> DependentTalants { get; } = new List<TalantBase>();

        public virtual TalantInfo TalantInfo(object @object)
        {
            return this.CallTalantInfo(@object as dynamic);
        }

        protected abstract TalantInfo CallTalantInfo(dynamic obj);

        public TalantInfo[] TalantEffects(Character character)
        {
            var abils = character.PropertiesOfType<Ability>();

            var talantType = this.GetType();

            var infoes = new List<TalantInfo>();

            foreach (var abil in abils)
            {
                var dispatchExists = talantType.GetMethods().Any(m => m.Name == nameof(TalantInfo) && m.GetParameters()[0].ParameterType == abil.GetType());
                if (dispatchExists)
                {
                    var talantInfo = this.TalantInfo(abil);
                    talantInfo.AbilityName = abil.Name;
                    infoes.Add(talantInfo);
                }
            }

            return infoes.ToArray();
        }
    }
}