namespace Rogue.Abilities.Talants
{
    using FastMember;
    using Rogue.Classes;
    using Rogue.Map;
    using Rogue.Map.Objects;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Содержит поля которые должны быть типа <see cref="Talant<typeparamref name="TClass"/>"/>
    /// </summary>
    public class TalantTree<TClass> where TClass : Character
    {
        private IEnumerable<Talant<TClass>> Talants
        {
            get
            {
                var accessor = TypeAccessor.Create(this.GetType());

                var members = accessor.GetMembers().Where(m =>
                {
                    if (m.Type.BaseType.IsGenericType)
                    {
                        return m.Type.BaseType.GetGenericTypeDefinition() == typeof(Talant<>);
                    }

                    return false;
                });

                var talants = members.Select(m => accessor[this, m.Name]).Cast<Talant<TClass>>();

                return talants.Where(t => t.Opened);
            }
        }

        public bool CanUse(TClass @class, Ability ability)
        {
            var talants = Talants;

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

            var talants = Talants;
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
            var talants = Talants;
            talants.ForEach(t =>
            {
                t.Bind(gameMap, avatar, @class);
            });

            foreach (var talant in talants)
            {
                talant.Discard(ability);
            }
        }
    }
}