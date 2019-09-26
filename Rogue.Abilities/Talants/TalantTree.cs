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

            if (talants.Count() == 0)
            {
                return true;
            }
            else
            {
                return talants
                .Select(t => t.CanUse(@class, ability))
                .Aggregate((x, y) => x && y);
            }
        }

        public void Use(GameMap gameMap, Avatar avatar, TClass @class, Action<GameMap, Avatar, TClass> @base, Ability ability)
        {
            var baseDontNeeded = false;

            foreach (var talant in Talants)
            {
                baseDontNeeded = talant.Use(gameMap, avatar, @class, @base, ability);
            }

            if (!baseDontNeeded)
            {
                @base?.Invoke(gameMap, avatar, @class);
            }
        }

        public void Dispose(GameMap gameMap, Avatar avatar, TClass @class, Action<GameMap, Avatar, TClass> @base, Ability ability)
        {
            foreach (var talant in Talants)
            {
                talant.Dispose(gameMap, avatar, @class, @base, ability);
            }
        }
    }
}