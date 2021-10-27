namespace Dungeon12.Abilities.Talants
{
    using FastMember;
    using Dungeon12.Abilities.Talants.NotAPI;
    using Dungeon12.Classes;
    using Dungeon12.Map;
    using Dungeon12.Map.Objects;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Dungeon;

    /// <summary>
    /// Содержит поля которые должны быть типа <see cref="Talant<typeparamref name="TClass"/>"/>
    /// </summary>
    public abstract class TalantTree<TClass> : TalantTrees.TalantTree
        where TClass : Character
    {
        public TalantTree()
        {
            Talants
                .SelectMany(t => t)
                .GroupBy(t => t.Group)
                .ForEach(t =>
                {
                    Action<TalantBase> groupInactive = talant =>
                    {
                        t.Except(talant.InList()).ForEach(x =>
                        {
                            x.Active = false;
                        });
                    };
                    t.ForEach(x => x.GroupActive = groupInactive);
                });
        }

        public override List<IGrouping<int, TalantBase>> Talants
        {
            get
            {
                var talants = ThisTalants();

                foreach (var talant in talants)
                {
                    foreach (var dependant in talant?.DependsOn ?? new string[0])
                    {
                        var depTalant = talants.FirstOrDefault(x => x.GetType().Name == dependant);
                        if (depTalant != default && !talant.DependentTalants.Contains(depTalant))
                        {
                            talant.DependentTalants.Add(depTalant);
                        }
                    }
                }

                return talants
                    .Cast<TalantBase>()
                    .GroupBy(t => t.Tier)
                    .OrderBy(x => x.Key)
                    .ToList();
            }
        }

        private IEnumerable<Talant<TClass>> OpenedTalants => ThisTalants().Where(t => t.Available);

        private IEnumerable<Talant<TClass>> ThisTalants()
        {
            var accessor = TypeAccessor.Create(this.GetType());

            var members = accessor.GetMembers().Where(m => typeof(TalantBase).IsAssignableFrom(m.Type));

            var talants = members.Select(m => accessor[this, m.Name]).Cast<Talant<TClass>>();
            return talants;
        }

        public bool CanUse(TClass @class, Ability ability)
        {
            var talants = OpenedTalants;

            talants.ForEach(t =>
            {
                t.Bind(default, default, @class);
            });

            if (talants.Count() == 0)
            {
                return true;
            }
            else
            {
                return talants
                .Select(t => t.CanUse(ability))
                .Aggregate((x, y) => x && y);
            }
        }

        public void Use(GameMap gameMap, Avatar avatar, TClass @class, Action<GameMap, Avatar, TClass> @base, Ability ability)
        {
            var baseDontNeeded = false;

            var talants = OpenedTalants;
            talants.ForEach(t =>
            {
                t.Bind(gameMap, avatar, @class);
            });

            foreach (var talant in talants)
            {
                talant.Apply(ability);
            }

            if (!baseDontNeeded)
            {
                @base?.Invoke(gameMap, avatar, @class);
            }
        }

        public void Dispose(GameMap gameMap, Avatar avatar, TClass @class, Action<GameMap, Avatar, TClass> @base, Ability ability)
        {
            var talants = OpenedTalants;
            talants.ForEach(t =>
            {
                t.Bind(gameMap, avatar, @class);
            });

            foreach (var talant in talants)
            {
                talant.Discard(ability);
            }

            @base?.Invoke(gameMap, avatar, @class);
        }
    }
}